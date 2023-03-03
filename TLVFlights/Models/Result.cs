namespace TLVFlights.Models;

public class Result
{
    public bool include_total { get; set; }
    public int limit { get; set; }
    public string records_format { get; set; }
    public string resource_id { get; set; }
    public object total_estimation_threshold { get; set; }
    public List<JsonFlightRecord> records { get; set; }
    public Links _links { get; set; }
    public int total { get; set; }
    public bool total_was_estimated { get; set; }
}