using cams.application.models;
using FluentResults;

namespace cams.application.repositories;

public class VehicleRepository : IVehicleRepository
{
    private static List<Vehicle> _auctionInventory =
    [
        new Vehicle("VIN1234567890", VehicleType.Sedan, "Toyota", "Camry", 2020),
        new Vehicle("VIN2345678901", VehicleType.Suv, "Honda", "CR-V", 2021),
        new Vehicle("VIN3456789012", VehicleType.Hatchback, "Volkswagen", "Golf", 2019),
        new Vehicle("VIN4567890123", VehicleType.Truck, "Ford", "F-150", 2022),
        new Vehicle("VIN5678901234", VehicleType.Sedan, "BMW", "3 Series", 2023)
    ];

    /// <inheritdoc/>
    public Task<Vehicle> AddVehicleAsync(string vin, VehicleType vehicleType, string manufacturer, string model,
        int year)
    {
        var vehicle = new Vehicle(vin, vehicleType, manufacturer, model, year);
        _auctionInventory.Add(vehicle);
        return Task.FromResult(vehicle);
    }

    /// <inheritdoc/>
    public Task<Vehicle> GetVehicleByVinAsync(string vin)
    {
        var vehicle = _auctionInventory.FirstOrDefault(v => v.Vin == vin);
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
        return Task.FromResult(_auctionInventory.Any(v => v.Vin == vin));
    }

    /// <inheritdoc/>
    public Task<List<Vehicle>> GetAllVehicles()
    {
        return Task.FromResult(_auctionInventory);
    }
}