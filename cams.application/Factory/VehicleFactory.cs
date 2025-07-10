using cams.contracts.models;
using cams.contracts.Requests.Vehicles;
using cams.contracts.shared;

namespace cams.application.Factory;

public static class VehicleFactory
{
    public static Vehicle CreateVehicle(AddVehicleRequest request)
    {
        return request.VehicleType switch
        {
            VehicleType.Hatchback => new Hatchback(request.Manufacturer, request.Model, request.Year, request.Vin,
                request.NumberOfDoors),
            VehicleType.Sedan => new Sedans(request.Manufacturer, request.Model, request.Year, request.Vin,
                request.NumberOfDoors),
            VehicleType.Suv => new Suv(request.Manufacturer, request.Model, request.Year, request.Vin,
                request.NumberOfSeats),
            VehicleType.Truck => new Truck(request.Manufacturer, request.Model, request.Year, request.Vin,
                request.LoadCapacity),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}