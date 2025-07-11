using System;
using cams.contracts.models;
using cams.contracts.Responses.Vehicles;

namespace cams.application.Mappers;

public static class VehicleMapper
{
    /// <summary>
    /// Maps a vehicle to a response model.
    /// </summary>
    /// <param name="vehicle">The vehicle to map.</param>
    /// <returns>A response model containing the vehicle data.</returns>
    public static VehicleResponse ToResponse(this Vehicle vehicle)
    {
        if (vehicle == null) throw new ArgumentNullException(nameof(vehicle));

        return new VehicleResponse(vehicle.Reference, vehicle.Manufacturer, vehicle.Model, vehicle.Year, vehicle.GetType().Name);
    }

}
