using cams.application.models;
using FluentResults;

namespace cams.application.services;

public interface IVehicleService
{
    Task<Result<Vehicle>> AddVehicleAsync(string vin, VehicleType vehicleType, string manufacturer, string model, int year);

    Task<Result<Vehicle>> GetVehicleByVinAsync(string vin);
    
    IEnumerable<Vehicle> Search(Func<Vehicle, bool> predicate);
    
    Task<List<Vehicle>> GetAllVehicles();
}