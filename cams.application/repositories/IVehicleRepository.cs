using cams.application.models;
using cams.contracts.shared;

namespace cams.application.repositories;

public interface IVehicleRepository
{
    /// <summary>
    /// Adds a new vehicle to the repository.
    /// </summary>
    /// <param name="vin">The vehicle identification number.</param>
    /// <param name="vehicleType">The type of the vehicle.</param>
    /// <param name="manufacturer">The manufacturer of the vehicle.</param>
    /// <param name="model">The model of the vehicle.</param>
    /// <param name="year">The manufacturing year of the vehicle.</param>
    /// <returns>The added <see cref="Vehicle"/>.</returns>
    Task<Vehicle> AddVehicleAsync(string vin, VehicleType vehicleType, string manufacturer, string model, int year);

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