using cams.contracts.shared;

namespace cams.contracts.models;

public abstract class Vehicle
{
    public string Model { get; set; }
    public string Manufacturer { get; set; }
    public int Year { get; set; }

    public string Reference { get; set; }

    protected Vehicle(string model, string manufacturer, int year, string reference)
    {
        Model = model;
        Manufacturer = manufacturer;
        Year = year;
        Reference = reference;
    }
}