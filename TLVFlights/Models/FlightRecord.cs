namespace TLVFlights.Models;

public class FlightRecord
{
    private FlightRecord()
    {

    }

    public static FlightRecord FromJsonRecord(JsonFlightRecord record)
    {
        return new()
        {
            Id = record._id,
            FlightCode = record.CHOPER,
            FlightNumer = record.CHFLTN,
            AirlineCompany = record.CHOPERD,
            EstimatedDepartureTime = record.CHSTOL,
            RealDepartureTime = record.CHPTOL,
            Gate = record.CHAORD,
            DestinationAirportShort = record.CHLOC1,
            DestinationAirportFull = record.CHLOC1D,
            CityNameHebrew = record.CHLOC1TH,
            CityNameEnglish = record.CHLOC1T,
            CountryNameHebrew = record.CHLOC1CH,
            CountryNameEnglish = record.CHLOCCT,
            TLVTerminal = record.CHTERM,
            TLVCheckInCounter = record.CHCINT,
            TLVCheckInZone = record.CHCKZN,
            StatusEnglish = record.CHRMINE,
            StatusHebrew = record.CHRMINH
        };
    }

    public string StatusHebrew { get; set; }

    public string StatusEnglish { get; set; }

    public string? TLVCheckInZone { get; set; }

    public string? TLVCheckInCounter { get; set; }

    public string TLVTerminal { get; set; }

    public string CountryNameEnglish { get; set; }

    public string CountryNameHebrew { get; set; }

    public string CityNameEnglish { get; set; }

    public string CityNameHebrew { get; set; }

    public string DestinationAirportFull { get; set; }

    public string DestinationAirportShort { get; set; }

    public string Gate { get; set; }

    public DateTime RealDepartureTime { get; set; }

    public DateTime EstimatedDepartureTime { get; set; }

    public string AirlineCompany { get; set; }

    public string FlightNumer { get; set; }

    public string FlightCode { get; set; }

    public int Id { get; set; }

    public bool IsInbound => string.IsNullOrEmpty(TLVCheckInCounter) || string.IsNullOrEmpty(TLVCheckInZone);
    public bool IsOutbound => !IsInbound;
}