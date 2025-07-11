namespace cams.contracts.models;

public class Car : Vehicle
{
    public Car(string manufacturer, string model, int year, string vin) : base(model, manufacturer, year,vin)
    {
        Reference = vin;
    }
}