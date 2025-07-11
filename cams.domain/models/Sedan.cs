namespace cams.contracts.models;

public class Sedan: Car
{
    public int NumberOfDoors { get; set; }
    public Sedan(string manufacturer, string model, int year, string vin,int? numberOfDoors) : base(manufacturer, model, year, vin)
    {
        Manufacturer = manufacturer;
        Model = model;
        Year = year;
        NumberOfDoors=numberOfDoors.HasValue? numberOfDoors.Value : 4; // Default to 4 doors if not specified
    }
}