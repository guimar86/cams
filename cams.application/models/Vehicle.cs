using cams.application.factories;

namespace cams.application.models;

public class Vehicle
{

    public string Vin { get; set; } //Vehicle Identification Number

    public VehicleType VehicleType { get; set; }

    public BaseVehicleAttributes VehicleAttributes { get; set; }

    public Vehicle(string vin, VehicleType vehicleType)
    {
        Vin = vin;
        VehicleType = vehicleType;
        VehicleAttributes = VehicleAttributesFactory.CreateVehicleAttributes(vehicleType);
    }
}
