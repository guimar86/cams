using System;

namespace cams.application.models;

public class SedanAttributes : BaseVehicleAttributes
{
    public int NumberOfDoors { get; set; }
    public override decimal StartingBid { get; set; }= 10000m;
}
