using cams.application.models;

namespace cams.application.factories;

public static class VehicleAttributesFactory
{
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