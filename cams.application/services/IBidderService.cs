using cams.application.models;
using FluentResults;

namespace cams.application.services;

public interface IBidderService
{
    Task<Result<Bidder>> GetBidderByIdAsync(Guid bidderId);
    Task<Result<Bidder>> CreateBidderAsync(Guid bidderId, string name);
    Task<Result<List<Bidder>>> GetAllBiddersAsync();
}