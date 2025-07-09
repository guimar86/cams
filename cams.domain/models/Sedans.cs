namespace cams.contracts.models;

public class Sedans: Car
{
    public int NumberOfDoors { get; set; }
    public Sedans(string manufacturer, string model, int year, string vin,int numberOfDoors) : base(manufacturer, model, year, vin)
    {
        Manufacturer = manufacturer;
        Model = model;
        Year = year;
        Vin = vin;
        NumberOfDoors=numberOfDoors;
    }
}