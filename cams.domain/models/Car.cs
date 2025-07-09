namespace cams.contracts.models;

public class Car : Vehicle
{
    public string Vin { get; set; }

    public Car(string manufacturer, string model, int year, string vin)
        : base(model, manufacturer, year)
    {
        Vin = vin;
    }
}