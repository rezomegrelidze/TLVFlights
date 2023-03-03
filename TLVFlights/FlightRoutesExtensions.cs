using Microsoft.AspNetCore.Mvc;
using TLVFlights.Services;

namespace TLVFlights;

public static class FlightRoutesExtensions
{
    static async Task<IResult> GetResult<T>(Func<Task<T?>> f)
    {
        var result = await f();
        return result == null ? Results.BadRequest() : Results.Ok(result);
    }

    public static void MapFlightRoutes(this WebApplication app)
    {
        // Total number of flights
        app.MapGet("/flights",
            async ([FromServices] FlightRecordsService service) => 
                await GetResult(service.NumberOfFlights));

        // Number of inbound flights
        app.MapGet("/inboundFlights",
            async ([FromServices] FlightRecordsService service) =>
                await GetResult(service.NumberOfInboundFlights));

        app.MapGet("/outboundFlights",
            async ([FromServices] FlightRecordsService service) =>
                await GetResult(service.NumberOfOutboundFlights));

        // Number of flights from a specific country (inbound & outbound)
        app.MapGet("/countryFlights",
            async (string country, [FromServices] FlightRecordsService service) =>
                await GetResult(() => service.NumberOfFlightsFromCountry(country)));

        // Number of outbound flights from a specific country
        app.MapGet("/countryFlightsOutbound",
            async (string country, [FromServices] FlightRecordsService service) =>
                await GetResult(() => service.NumberOfOutboundFlightsFromCountry(country)));

        // Number of inbound flights from a specific country.
        app.MapGet("/countryFlightsInbound",
            async (string country, [FromServices] FlightRecordsService service) =>
                await GetResult(() => service.NumberOfInboundFlightsFromCountry(country)));

        // Number of delayed flights
        app.MapGet("/delayedFlights",
            async ([FromServices] FlightRecordsService service) =>
                await GetResult(service.NumberOfDelayedFlights));

        // Most popular destination
        app.MapGet("/mostPopularDestination",
            async ([FromServices] FlightRecordsService service) =>
                await GetResult(service.MostPopularDestination));

        // BONUS* - Getaway flights
        app.MapGet("/getaway",
            async ([FromServices] FlightRecordsService service) =>
                await GetResult(service.GetGetawayFlights));
    }
}