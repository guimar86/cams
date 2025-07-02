using cams.contracts.models;
using cams.contracts.Repositories;

namespace cams.infrastructure.repositories;

public class BidderRepository: IBidderRepository
{
    private static readonly List<Bidder> Bidders =
    [
        new Bidder(Guid.Parse("f7d8b9fb-22ec-4ad6-a272-0540865c7b8c"), "John Mclane"),
        new Bidder(Guid.Parse("727fcc8a-3fb6-4dfb-a7aa-b73aee80cac9"), "John Rambo"),
        new Bidder(Guid.Parse("328e1812-cc63-4de1-b715-c3e4684577d5"), "John Wick")
    ];
    
    /// <inheritdoc/>
    public Task<Bidder> CreateBidderAsync(Guid bidderId, string name)
    {
        var bidder = new Bidder(bidderId, name);
        Bidders.Add(bidder);
        return Task.FromResult(bidder);
    }

    /// <inheritdoc/>
    public Task<Bidder> GetBidderByIdAsync(Guid bidderId)
    {
         return Task.FromResult(Bidders.FirstOrDefault(p => p.Id == bidderId));
    }

    /// <inheritdoc/>
    public Task<List<Bidder>> GetAllBiddersAsync()
    {
        return Task.FromResult(Bidders);
    }
}