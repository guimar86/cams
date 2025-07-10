using cams.contracts.models;
using cams.contracts.Requests.Vehicles;
using cams.contracts.Responses.Vehicles;
using cams.contracts.shared;
using FluentResults;

namespace cams.application.services;

public interface IVehicleService
{
    /// <summary>
    /// Asynchronously adds a new vehicle to the system.
    /// </summary>
    /// <param name="request">Request object with all parameters to create a vehicle</param>
    /// <returns>A result containing the added vehicle or an error.</returns>
    Task<Result<Vehicle>> AddVehicleAsync(AddVehicleRequest request);


    /// <summary>
    /// Retrieves a vehicle by its VIN asynchronously.
    /// </summary>
    /// <param name="vin">The vehicle identification number.</param>
    /// <returns>A result containing the vehicle if found, or an error.</returns>
    Task<Result<Vehicle>> GetVehicleByVinAsync(string vin);

    /// <summary>
    /// Searches for vehicles matching the specified predicate.
    /// </summary>
    /// <param name="predicate">A function to test each vehicle for a condition.</param>
    /// <param name="request">Request</param>
    /// <returns>An enumerable of vehicles that match the predicate.</returns>
    Result<IEnumerable<VehicleResponse>> Search(SearchVehicleRequest request);

    /// <summary>
    /// Retrieves all vehicles asynchronously.
    /// </summary>
    /// <returns>A list of all vehicles.</returns>
    Task<IEnumerable<VehicleResponse>> GetAllVehicles();
}