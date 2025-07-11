using System;

namespace cams.contracts.Responses.Vehicles;

public class VehicleResponse
{
    public VehicleResponse(string vin, string manufacturer, string model, int year, string type)
    {
        Vin = vin;
        Manufacturer = manufacturer;
        Model = model;
        Year = year;
        Type = type;
    }

    public string Vin { get; private set; }
    public string Manufacturer { get; private set; }
    public string Model { get; private set; }
    public int Year { get; private set; }
    public string Type { get; private set; }
}
