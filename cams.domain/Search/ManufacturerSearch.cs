using cams.contracts.models;

namespace cams.contracts.Search;

public class ManufacturerSearch(string manufacturer) : ISearch<Vehicle>
{
    public bool Match(Vehicle item)
    {
        return string.IsNullOrWhiteSpace(item.Manufacturer) ||
               item.Manufacturer.Contains(manufacturer, StringComparison.OrdinalIgnoreCase);
    }
}