namespace cams.contracts.models;

public class Suv: Car
{
    public int NumberOfSeats { get; set; }
    public Suv(string manufacturer, string model, int year, string vin,int? numberOfSeats) : base(manufacturer, model, year, vin)
    {
        Manufacturer = manufacturer;
        Model = model;
        Year = year;
        NumberOfSeats=numberOfSeats.HasValue ? numberOfSeats.Value : 5; // Default to 5 seats if not specified
    }
}