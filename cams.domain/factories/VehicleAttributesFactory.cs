using cams.contracts.models;
using cams.contracts.shared;

namespace cams.contracts.factories;

/// <summary>
/// Factory class for creating instances of <see cref="BaseVehicleAttributes"/> based on <see cref="VehicleType"/>.
/// </summary>
public static class VehicleAttributesFactory
{
    /// <summary>
    /// Creates a <see cref="BaseVehicleAttributes"/> instance corresponding to the specified <paramref name="vehicleType"/>.
    /// </summary>
    /// <param name="vehicleType">The type of vehicle for which to create attributes.</param>
    /// <returns>An instance of a class derived from <see cref="BaseVehicleAttributes"/>.</returns>
    /// <exception cref="ArgumentException">Thrown when an invalid vehicle type is provided.</exception>
    public static BaseVehicleAttributes CreateVehicleAttributes(VehicleType vehicleType)
    {
        return vehicleType switch
        {
            VehicleType.Sedan => new SedanAttributes(),
            VehicleType.Hatchback => new HatchbackAttributes(),
            VehicleType.Suv => new SuvAttributes(),
            VehicleType.Truck => new TruckAttributes(),
            _ => throw new ArgumentException("Invalid vehicle type")
        };
    }
}