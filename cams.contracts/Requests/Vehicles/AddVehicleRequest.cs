using System;
using cams.contracts.shared;

namespace cams.contracts.Requests.Vehicles;

public record AddVehicleRequest(string vin, VehicleType vehicleType, string manufacturer, string model, int year);
