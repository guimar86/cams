using cams.contracts.models;

namespace cams.contracts.Repositories;

public interface IVehicleRepository
{
    /// <summary>
    /// Adds a new vehicle to the repository asynchronously.
    /// </summary>
    /// <param name="vehicle">The vehicle to add.</param>
    /// <returns>The added <see cref="Vehicle"/> entity.</returns>
    Task<Vehicle> AddVehicleAsync(Vehicle vehicle);

    /// <summary>
    /// Retrieves a vehicle by its VIN.
    /// </summary>
    /// <param name="vin">The vehicle identification number.</param>
    /// <returns>The <see cref="Vehicle"/> with the specified VIN, or null if not found.</returns>
    Task<Vehicle> GetVehicleByVinAsync(string vin);

    /// <summary>
    /// Searches for vehicles matching the specified predicate.
    /// </summary>
    /// <param name="predicate">A function to test each vehicle for a condition.</param>
    /// <returns>An enumerable of vehicles that match the predicate.</returns>
    IEnumerable<Vehicle> Search(Func<Vehicle, bool> predicate);

    /// <summary>
    /// Checks if a vehicle with the specified VIN exists in an active auction.
    /// </summary>
    /// <param name="vin">The vehicle identification number.</param>
    /// <returns>True if the vehicle exists in an active auction; otherwise, false.</returns>
    Task<bool> ExistsInActiveAuction(string vin);

    /// <summary>
    /// Retrieves all vehicles from the repository.
    /// </summary>
    /// <returns>A list of all vehicles.</returns>
    Task<List<Vehicle>> GetAllVehicles();
}