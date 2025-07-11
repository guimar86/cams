namespace cams.contracts.Search;

public class SearchAggregator<T>(List<ISearch<T>> searches) : ISearch<T> where T : class
{
    public bool Match(T item)
    {
        return searches.All(search => search.Match(item));
    }
}