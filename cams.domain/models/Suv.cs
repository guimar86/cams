namespace cams.contracts.models;

public class Suv: Car
{
    public int NumberOfSeats { get; set; }
    public Suv(string manufacturer, string model, int year, string vin,int numberOfSeats) : base(manufacturer, model, year, vin)
    {
        Manufacturer = manufacturer;
        Model = model;
        Year = year;
        Vin = vin;
        NumberOfSeats=numberOfSeats;
    }
}