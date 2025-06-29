using System;

namespace cams.application.models;

public interface IVehicleRepository
{
    Task<Vehicle> CreateVehicleAsync(string vin, VehicleType vehicleType, string manufacturer, string model, int year);
    
    Task<Vehicle> GetVehicleByVinAsync(string vin);
    
    Task<IEnumerable<Vehicle>> GetAllVehiclesAsync();
}
