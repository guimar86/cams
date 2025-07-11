using cams.application.Factory;
using cams.application.Mappers;
using cams.contracts.models;
using cams.contracts.Repositories;
using cams.contracts.Requests.Vehicles;
using cams.contracts.Responses.Vehicles;
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
    public async Task<Result<Vehicle>> AddVehicleAsync(AddVehicleRequest request)
    {
        Vehicle vehicle = VehicleFactory.CreateVehicle(request);

        //does the vehicle already exist in the system?
        var vehicleExists = await _repository.GetVehicleByVinAsync(vehicle.Reference);
        if (vehicleExists != null)
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
    public Result<IEnumerable<VehicleResponse>> Search(SearchVehicleRequest request)
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

        var result = _repository.Search(search.Match);
        var response= result.Select(v => v.ToResponse());
        return Result.Ok(response);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<VehicleResponse>> GetAllVehicles()
    {
        var vehicles = await _repository.GetAllVehicles();

        return vehicles.Select(v => v.ToResponse());
    }


}