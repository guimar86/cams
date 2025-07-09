namespace cams.contracts.models;

public class Hatchback: Car
{
    public int NumberOfDoors { get; set; }
    public Hatchback(string manufacturer, string model, int year, string vin,int numberOfDoors) : base(manufacturer, model, year, vin)
    {
        Manufacturer = manufacturer;
        Model = model;
        Year = year;
        Vin = vin;
        NumberOfDoors=numberOfDoors;
    }
}