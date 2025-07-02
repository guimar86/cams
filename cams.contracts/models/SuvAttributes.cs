namespace cams.contracts.models;

/// <summary>
/// Represents the attributes specific to an SUV vehicle.
/// </summary>
public class SuvAttributes : BaseVehicleAttributes
{
    /// <summary>
    /// Gets or sets the number of seats in the SUV.
    /// </summary>
    public int NumberOfSeats { get; set; }
}