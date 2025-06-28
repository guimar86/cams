using System;

namespace cams.application.models;

public class HatchbackAttributes : BaseVehicleAttributes
{
    public int NumberOfDoors { get; set; }
    public override decimal StartingBid { get; set;  } = 2000.00M;
}
