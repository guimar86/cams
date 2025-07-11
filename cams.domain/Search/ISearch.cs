namespace cams.contracts.Search;

public interface ISearch<T> where T : class
{
    bool Match(T item);
}