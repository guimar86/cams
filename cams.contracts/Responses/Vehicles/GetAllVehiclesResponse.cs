namespace cams.contracts.Responses.Vehicles;

public record GetAllVehiclesResponse(string Vin, string VehicleType, string Manufacturer, string Model, int Year);