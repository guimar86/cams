using cams.contracts.models;
using cams.contracts.shared;
using FluentResults;

namespace cams.application.services;

public interface IVehicleService
{
    /// <summary>
    /// Asynchronously adds a new vehicle to the system.
    /// </summary>
    /// <param name="vehicle">The vehicle to add.</param>
    /// <returns>A result containing the added vehicle or an error.</returns>
    Task<Result<Vehicle>> AddVehicleAsync(Vehicle vehicle);


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
    /// <returns>An enumerable of vehicles that match the predicate.</returns>
    IEnumerable<Vehicle> Search(Func<Vehicle, bool> predicate);

    /// <summary>
    /// Retrieves all vehicles asynchronously.
    /// </summary>
    /// <returns>A list of all vehicles.</returns>
    Task<List<Vehicle>> GetAllVehicles();
}