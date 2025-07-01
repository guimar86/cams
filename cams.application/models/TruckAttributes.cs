using System;

namespace cams.application.models;

/// <summary>
/// Represents the attributes specific to a truck.
/// Inherits from <see cref="BaseVehicleAttributes"/>.
/// </summary>
public class TruckAttributes : BaseVehicleAttributes
{
    /// <summary>
    /// Gets or sets the load capacity of the truck.
    /// </summary>
    public int LoadCapacity { get; set; }
}
