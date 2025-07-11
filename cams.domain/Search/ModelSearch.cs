using cams.contracts.models;

namespace cams.contracts.Search;

public class ModelSearch(string model) : ISearch<Vehicle>
{
    public bool Match(Vehicle item)
    {
        return string.IsNullOrWhiteSpace(item.Model) || item.Model.Contains(model, StringComparison.CurrentCultureIgnoreCase);
    }
}