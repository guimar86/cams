using cams.application.factories;
using cams.contracts.shared;

namespace cams.application.models;

/// <summary>
/// Represents a vehicle with a VIN, type, and associated attributes.
/// </summary>
public class Vehicle
{
    /// <summary>
    /// Gets or sets the Vehicle Identification Number.
    /// </summary>
    public string Vin { get; set; }

    /// <summary>
    /// Gets or sets the type of the vehicle.
    /// </summary>
    public VehicleType VehicleType { get; set; }

    /// <summary>
    /// Gets or sets the vehicle's attributes.
    /// </summary>
    public BaseVehicleAttributes VehicleAttributes { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Vehicle"/> class.
    /// </summary>
    /// <param name="vin">The vehicle identification number.</param>
    /// <param name="vehicleType">The type of the vehicle.</param>
    /// <param name="manufacturer">The manufacturer of the vehicle.</param>
    /// <param name="model">The model of the vehicle.</param>
    /// <param name="year">The year the vehicle was manufactured.</param>
    public Vehicle(string vin, VehicleType vehicleType, string manufacturer, string model, int year)
    {
        Vin = vin;
        VehicleType = vehicleType;
        VehicleAttributes = VehicleAttributesFactory.CreateVehicleAttributes(vehicleType);

        VehicleAttributes.Manufacturer = manufacturer;
        VehicleAttributes.Model = model;
        VehicleAttributes.Year = year;
    }
}
