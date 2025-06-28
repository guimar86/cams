using System;

namespace cams.application.models;

public class SuvAttributes : BaseVehicleAttributes
{
    public int NumberOfSeats { get; set; }
    public override decimal StartingBid { get; set; }= 15000m;
}
