using System;
using cams.application.models;
using FluentResults;

namespace cams.application.repositories;

public interface IVehicleRepository
{
    Task<Result<Vehicle>> CreateVehicleAsync(string vin, VehicleType vehicleType, string manufacturer, string model, int year);

    Task<Result<Vehicle>> GetVehicleByVinAsync(string vin);
    
    IEnumerable<Vehicle> Search(Func<Vehicle, bool> predicate);
}
