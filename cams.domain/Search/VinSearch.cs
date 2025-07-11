using System;
using cams.contracts.models;

namespace cams.contracts.Search;

public class VinSearch(string Vin) : ISearch<Auction>
{
    public bool Match(Auction item)
    {
        return string.IsNullOrWhiteSpace(Vin) || 
               item.Vehicle.Reference.Equals(Vin, StringComparison.OrdinalIgnoreCase);
    }
}
