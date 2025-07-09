namespace cams.contracts.Responses.Vehicles;

/// <summary>
/// Response returned after creating a vehicle.
/// </summary>
/// <param name="vin">The vehicle identification number.</param>
public record class CreateVehicleResponse(string vin);
