using cams.application.models;

namespace cams.application.repositories;

public interface IBidderRepository
{
    Task<Bidder> CreateBidderAsync(Guid bidderId, string name);
    Task<Bidder> GetBidderByIdAsync(Guid bidderId);
    Task<List<Bidder>> GetAllBiddersAsync();
}