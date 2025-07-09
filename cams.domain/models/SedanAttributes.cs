namespace cams.contracts.models;

/// <summary>
/// Represents the attributes specific to a sedan vehicle.
/// </summary>
public class SedanAttributes : BaseVehicleAttributes
{
    /// <summary>
    /// Gets or sets the number of doors for the sedan.
    /// </summary>
    public int NumberOfDoors { get; set; }
}
