namespace TLVFlights.Models;

public class JsonFlightRecord
{
    public int _id { get; set; }
    public string CHOPER { get; set; }
    public string CHFLTN { get; set; }
    public string CHOPERD { get; set; }
    public DateTime CHSTOL { get; set; }
    public DateTime CHPTOL { get; set; }
    public string CHAORD { get; set; }
    public string CHLOC1 { get; set; }
    public string CHLOC1D { get; set; }
    public string CHLOC1TH { get; set; }
    public string CHLOC1T { get; set; }
    public string CHLOC1CH { get; set; }
    public string CHLOCCT { get; set; }
    public string CHTERM { get; set; }
    public string? CHCINT { get; set; }
    public string? CHCKZN { get; set; }
    public string CHRMINE { get; set; }
    public string CHRMINH { get; set; }
}