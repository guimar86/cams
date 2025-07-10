namespace cams.contracts.models;

public class Truck : Car
{
    public decimal LoadCapacity { get; set; }

    public Truck(string manufacturer, string model, int year, string vin, decimal? loadCapacity) : base(manufacturer,
        model, year, vin)
    {
        Manufacturer = manufacturer;
        Model = model;
        Year = year;
        LoadCapacity = loadCapacity.HasValue ? loadCapacity.Value : 0;
    }
}