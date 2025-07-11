using System;
using cams.contracts.shared;

namespace cams.contracts.Requests.Vehicles;


/// <summary>
/// Request to add a new vehicle.
/// </summary>
/// <param name="Vin">Vehicle Identification Number.</param>
/// <param name="VehicleType">Type of the vehicle.</param>
/// <param name="Manufacturer">Vehicle manufacturer.</param>
/// <param name="Model">Vehicle model.</param>
/// <param name="Year">Year of manufacture.</param>
/// <param name="NumberOfDoors">Number of doors (optional).</param>
/// <param name="NumberOfSeats">Number of seats (optional).</param>
/// <param name="LoadCapacity">Load capacity (optional).</param>
public record AddVehicleRequest(string Vin, VehicleType VehicleType, string Manufacturer, string Model, int Year, int? NumberOfDoors,int? NumberOfSeats,int? LoadCapacity);
