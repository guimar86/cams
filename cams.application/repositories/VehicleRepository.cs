using FluentResults;

namespace cams.application.repositories;

public class VehicleRepository : IVehicleRepository
{

    List<models.Vehicle> vehicles = [];
    public Task<Result<models.Vehicle>> CreateVehicleAsync(string vin, models.VehicleType vehicleType, string manufacturer, string model, int year)
    {
        if (vehicles.Any(v => v.Vin == vin))
        {
            return Task.FromResult(Result.Fail<models.Vehicle>(new Error("Vehicle with this VIN already exists.")));
        }

        var vehicle = new models.Vehicle(vin, vehicleType, manufacturer, model, year);

        vehicles.Add(vehicle);
        return Task.FromResult(Result.Ok(vehicle));
    }

    public Task<Result<IEnumerable<models.Vehicle>>> GetAllVehiclesAsync()
    {
        return Task.FromResult(Result.Ok<IEnumerable<models.Vehicle>>(vehicles));
    }

    public Task<Result<models.Vehicle>> GetVehicleByVinAsync(string vin)
    {
        var vehicle = vehicles.FirstOrDefault(v => v.Vin == vin);
        if (vehicle == null)
        {
            return Task.FromResult(Result.Fail<models.Vehicle>(new Error("Vehicle not found.")));
        }
        return Task.FromResult(Result.Ok(vehicle));
    }
}
