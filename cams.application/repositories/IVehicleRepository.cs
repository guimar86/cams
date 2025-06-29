using System;
using cams.application.models;
using FluentResults;

namespace cams.application.repositories;

public interface IVehicleRepository
{
    Task<Vehicle> AddVehicleAsync(string vin, VehicleType vehicleType, string manufacturer, string model, int year);

    Task<Vehicle> GetVehicleByVinAsync(string vin);
    
    IEnumerable<Vehicle> Search(Func<Vehicle, bool> predicate);

    bool ExistsInActiveAuction(string vin);
}
