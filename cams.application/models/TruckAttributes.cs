using System;

namespace cams.application.models;

public class TruckAttributes : BaseVehicleAttributes
{
    public int LoadCapacity { get; set; }
    public override decimal StartingBid { get; set; }= 20000m;
}
