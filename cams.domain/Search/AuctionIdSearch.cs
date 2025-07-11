using System;
using cams.contracts.models;

namespace cams.contracts.Search;

public class AuctionIdSearch(Guid Id) : ISearch<Auction>
{
    public bool Match(Auction item)
    {
        return item.Id == Id;
    }
}
