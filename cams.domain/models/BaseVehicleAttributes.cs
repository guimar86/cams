namespace cams.contracts.models;

/// <summary>
/// Abstract base class representing common vehicle attributes.
/// </summary>
public abstract class BaseVehicleAttributes
{
    /// <summary>
    /// Gets or sets the vehicle manufacturer.
    /// </summary>
    public string Manufacturer { get; set; }

    /// <summary>
    /// Gets or sets the vehicle model.
    /// </summary>
    public string Model { get; set; }

    /// <summary>
    /// Gets or sets the vehicle manufacturing year.
    /// </summary>
    public int Year { get; set; }
}