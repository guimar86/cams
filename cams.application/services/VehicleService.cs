using cams.contracts.models;
using cams.contracts.Repositories;
using cams.contracts.shared;
using FluentResults;

namespace cams.application.services;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _repository;

    /// <summary>
    /// Initializes a new instance of the <see cref="VehicleService"/> class.
    /// </summary>
    /// <param name="repository">The vehicle repository to use for data access.</param>
    public VehicleService(IVehicleRepository repository)
    {
        _repository = repository;
    }

    /// <inheritdoc/>
    public async Task<Result<Vehicle>> AddVehicleAsync(Vehicle vehicle)
    {
        bool isCarInActiveAuction = await _repository.ExistsInActiveAuction(vehicle.Vin);
        if (isCarInActiveAuction)
        {
            return Result.Fail<Vehicle>(new Error("Vehicle already exists."));
        }

        await _repository.AddVehicleAsync(vehicle);
        return Result.Ok(vehicle);
    }

    /// <inheritdoc/>
    public async Task<Result<Vehicle>> GetVehicleByVinAsync(string vin)
    {
        if (string.IsNullOrWhiteSpace(vin))
        {
            return Result.Fail<Vehicle>(new Error("VIN cannot be null or empty."));
        }

        Vehicle vehicle = await _repository.GetVehicleByVinAsync(vin);
        return Result.Ok(vehicle);
    }

    /// <inheritdoc/>
    public IEnumerable<Vehicle> Search(Func<Vehicle, bool> predicate)
    {
        if (predicate == null)
        {
            throw new ArgumentNullException(nameof(predicate), "Predicate cannot be null.");
        }

        return _repository.Search(predicate);
    }

    /// <inheritdoc/>
    public async Task<List<Vehicle>> GetAllVehicles()
    {
        return await _repository.GetAllVehicles();
    }
}