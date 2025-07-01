using System;
using cams.contracts.shared;

namespace cams.contracts.Requests.Vehicles;

public record AddVehicleRequest(string Vin, VehicleType VehicleType, string Manufacturer, string Model, int Year);
