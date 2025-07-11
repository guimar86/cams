namespace cams.contracts.Responses.Vehicles;


/// <summary>
/// Represents the response containing details of a vehicle.
/// </summary>
/// <param name="Vin">The vehicle identification number.</param>
/// <param name="VehicleType">The type of the vehicle.</param>
/// <param name="Manufacturer">The manufacturer of the vehicle.</param>
/// <param name="Model">The model of the vehicle.</param>
/// <param name="Year">The manufacturing year of the vehicle.</param>
/// <param name="Attribute">A key-value pair representing an additional attribute of the vehicle.</param>
public record GetAllVehiclesResponse(string Vin, string VehicleType, string Manufacturer, string Model, int Year,KeyValuePair<string,object> Attribute);