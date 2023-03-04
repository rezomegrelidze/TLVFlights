using Newtonsoft.Json;
using TLVFlights.Models;

namespace TLVFlights.Services;

public class FlightRecordsService
{
    private readonly ILogger _logger;
    private readonly string _baseUrl;
    private readonly string _baseDomainAddress;
    private int limit = 300;

    private DateTime _lastTimeFetched;

    private List<FlightRecord>? _cachedFlights;

    public FlightRecordsService(IConfiguration configuration,ILogger<FlightRecordsService> logger)
    {
        _logger = logger;
        _baseUrl = configuration?["ApiBaseUrl"] ??
                   "https://data.gov.il/api/3/action/datastore_search?resource_id=e83f763b-b7d7-479e-b172-ae981ddc6de5";
        _baseDomainAddress = configuration?["BaseDomainAddress"] ?? "https://data.gov.il";
    }

    private async Task<List<FlightRecord>?> GetAllFlights()
    {
        // cache expires every minute!
        if (_cachedFlights is not null && ((DateTime.Now - _lastTimeFetched).Minutes <= 1)) return _cachedFlights;

        _cachedFlights = new List<FlightRecord>();
        var rootResult = await GetApiResult($"{_baseUrl}&limit={limit}",isRelative: false); // Initial call which is full URL
        if (rootResult == null) return null;
        int total = rootResult.result.total;

        while (_cachedFlights.Count != total && rootResult is {})
        {
            _cachedFlights.AddRange(rootResult.result.records.Select(FlightRecord.FromJsonRecord));
            rootResult = await GetApiResult(rootResult.result._links.next);
        }

        _lastTimeFetched = DateTime.Now;
        return _cachedFlights;
    }

    private async Task<RootJsonResult?> GetApiResult(string url,bool isRelative = true)
    {
        if (isRelative)
        {
            url = $"{_baseDomainAddress}/{url}"; // when next urls returned from api we get relative urls
        }
        try
        {
            HttpClient client = new();
            var response = await client.GetAsync(url);
            var rootResult = JsonConvert.DeserializeObject<RootJsonResult>(await response.Content.ReadAsStringAsync());
            return rootResult;
        }
        catch (Exception ex)
        {
            _logger.LogInformation($"Failed to obtain root result from API! - Exception Message: {ex.Message}");
            return null;
        }
    }

    public async Task<int?> NumberOfFlights()
    {
        var allFlights = await GetAllFlights();
        return allFlights?.Count;
    }

    public async Task<int?> NumberOfOutboundFlights() => 
        (await GetAllFlights())?.Count(x => x.IsOutbound);

    public async Task<int?> NumberOfInboundFlights()  =>
        (await GetAllFlights())?.Count(x => x.IsInbound);

    private static bool IsFromCountry(FlightRecord record, string country) =>
        record.CountryNameEnglish.ToLower() == country.ToLower()
        || record.CountryNameHebrew.ToLower() == country.ToLower();

    public async Task<int?> NumberOfFlightsFromCountry(string country)  => 
        (await GetAllFlights())?.Count(x => IsFromCountry(x, country));

    public async Task<int?> NumberOfOutboundFlightsFromCountry(string country)  =>
        (await GetAllFlights())?.Count(x => IsFromCountry(x, country) && x.IsOutbound);

    public async Task<int?> NumberOfInboundFlightsFromCountry(string country) =>
        (await GetAllFlights())?.Count(x => (IsFromCountry(x,country) && x.IsInbound));

    public async Task<int?> NumberOfDelayedFlights() => 
        (await GetAllFlights())?.Count(x => x.EstimatedDepartureTime != x.RealDepartureTime);

    public async Task<string?> MostPopularDestination() => 
        (await GetAllFlights())?.MaxBy(x => x.IsOutbound)?.CityNameEnglish;

    public async Task<GetawayFlights?> GetGetawayFlights()
    {
        var allFlights = await GetAllFlights();
        var departure = allFlights?.Where(x => x.IsOutbound).MinBy(x => x.RealDepartureTime)?.FlightNumer;
        var arrival = allFlights?.Where(x => x.IsInbound).MinBy(x => x.RealDepartureTime)?.FlightNumer;
        return new()
        {
            Departure = departure,
            Arrival = arrival
        };
    }
}