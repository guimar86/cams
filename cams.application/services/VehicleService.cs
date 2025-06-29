using cams.application.models;
using cams.application.repositories;
using FluentResults;

namespace cams.application.services;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _repository;

    public VehicleService(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Vehicle>> AddVehicleAsync(string vin, VehicleType vehicleType, string manufacturer,
        string model, int year)
    {
        bool inActiveAuction = _repository.ExistsInActiveAuction(vin);
        if (inActiveAuction)
        {
            return Result.Fail<Vehicle>(new Error("Vehicle already exists."));
        }

        var vehicle = await _repository.AddVehicleAsync(vin, vehicleType, manufacturer, model, year);
        return Result.Ok(vehicle);
    }

    public async Task<Result<Vehicle>> GetVehicleByVinAsync(string vin)
    {
        if (string.IsNullOrWhiteSpace(vin))
        {
            return Result.Fail<Vehicle>(new Error("VIN cannot be null or empty."));
        }

        Vehicle vehicle = await _repository.GetVehicleByVinAsync(vin);
        return Result.Ok(vehicle);
    }

    public IEnumerable<Vehicle> Search(Func<Vehicle, bool> predicate)
    {
        if (predicate == null)
        {
            throw new ArgumentNullException(nameof(predicate), "Predicate cannot be null.");
        }

        return _repository.Search(predicate);
    }
}