using cams.contracts.models;

namespace cams.contracts.Search;

public class YearSearch(int? year):ISearch<Vehicle>
{
    public bool Match(Vehicle item)
    {
        return year!= 0 && item.Year == year;
    }
}