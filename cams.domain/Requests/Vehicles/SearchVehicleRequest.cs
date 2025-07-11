namespace cams.contracts.Requests.Vehicles;

public class SearchVehicleRequest
{
    public string Model { get;  set; }
    public string Manufacturer { get; set; }
    public int? Year { get; set; }
}