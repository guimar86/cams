using cams.application.models;
using FluentResults;

namespace cams.application.repositories;

public class VehicleRepository : IVehicleRepository
{
    private static List<Vehicle> _auctionInventory = [];

    public Task<Vehicle> AddVehicleAsync(string vin, VehicleType vehicleType, string manufacturer, string model,
        int year)
    {
        var vehicle = new Vehicle(vin, vehicleType, manufacturer, model, year);
        _auctionInventory.Add(vehicle);
        return Task.FromResult(vehicle);
    }


    public Task<Vehicle> GetVehicleByVinAsync(string vin)
    {
        var vehicle = _auctionInventory.FirstOrDefault(v => v.Vin == vin);
        return Task.FromResult(vehicle);
    }

    public IEnumerable<Vehicle> Search(Func<Vehicle, bool> predicate)
    {
        return _auctionInventory.Where(predicate);
    }

    public bool ExistsInActiveAuction(string vin)
    {
        return _auctionInventory.Any(v => v.Vin == vin);
    }
}