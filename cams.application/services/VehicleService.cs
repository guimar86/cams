using cams.contracts.models;
using cams.contracts.Repositories;
using cams.contracts.Requests.Vehicles;
using cams.contracts.Search;
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
        bool isCarInActiveAuction = await _repository.ExistsInActiveAuction(vehicle.Reference);
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
    public Result<IEnumerable<Vehicle>> Search(SearchVehicleRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Model) && string.IsNullOrWhiteSpace(request.Manufacturer) && !request.Year.HasValue)
        {
            return Result.Fail("At least one search parameter must be provided.");
        }

        if (request.Year.HasValue && request.Year.ToString().Length < 4)
        {
            return Result.Fail(new Error("Year must be a valid 4-digit number."));
        }
        
        var searches = new List<ISearch<Vehicle>>();

        if (!string.IsNullOrWhiteSpace(request.Model))
        {
            searches.Add(new ModelSearch(request.Model));
        }

        if (!string.IsNullOrWhiteSpace(request.Manufacturer))
        {
            searches.Add(new ManufacturerSearch(request.Manufacturer));
        }

        if (request.Year != 0 && request.Year.HasValue)
        {
            searches.Add(new YearSearch(request.Year.Value));
        }

        var search = new SearchAggregator<Vehicle>(searches);

        return Result.Ok(_repository.Search(search.Match));
    }

    /// <inheritdoc/>
    public async Task<List<Vehicle>> GetAllVehicles()
    {
        return await _repository.GetAllVehicles();
    }

   
}