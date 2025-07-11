using cams.contracts.models;
using cams.contracts.Repositories;

namespace cams.infrastructure.repositories;

public class VehicleRepository : IVehicleRepository
{
    private static List<Vehicle> _auctionInventory =
    [
        new Sedan(vin:"VIN1234567890",  manufacturer:"Toyota", model:"Camry", year:2020,numberOfDoors:5),
        new Suv(vin:"VIN1234567891",  manufacturer:"Toyota", model:"Camry", year:2020,numberOfSeats:5),
        new Truck(vin:"VIN1234567892",  manufacturer:"Toyota", model:"Hylux", year:2020,loadCapacity:520),
        new Hatchback(vin:"VIN1234567893",  manufacturer:"Toyota", model:"Camry", year:2020,numberOfDoors:5),
       
    ];

    /// <inheritdoc/>
    public Task<Vehicle> AddVehicleAsync(Vehicle vehicle)
    {
        _auctionInventory.Add(vehicle);
        return Task.FromResult(vehicle);
    }

    /// <inheritdoc/>
    public Task<Vehicle> GetVehicleByVinAsync(string vin)
    {
        var vehicle = _auctionInventory.FirstOrDefault(v => v.Reference == vin);
        return Task.FromResult(vehicle);
    }

    /// <inheritdoc/>
    public IEnumerable<Vehicle> Search(Func<Vehicle, bool> predicate)
    {
        return _auctionInventory.Where(predicate);
    }

    /// <inheritdoc/>
    public Task<bool> ExistsInActiveAuction(string vin)
    {
        return Task.FromResult(_auctionInventory.Any(v => v.Reference == vin));
    }

    /// <inheritdoc/>
    public Task<List<Vehicle>> GetAllVehicles()
    {
        return Task.FromResult(_auctionInventory);
    }
}