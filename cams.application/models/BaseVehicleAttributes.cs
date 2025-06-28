namespace cams.application.models;

public abstract class BaseVehicleAttributes
{
    public string Manufacturer { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }

    public abstract decimal StartingBid { get; set; }

}
