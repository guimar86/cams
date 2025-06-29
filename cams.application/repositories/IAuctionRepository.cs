using cams.application.models;
using FluentResults;

namespace cams.application.repositories;

public interface IAuctionRepository
{
    Task<Result<Auction>> CreateAuctionAsync(Guid auctionId, Vehicle vehicle, DateTime startTime, DateTime endTime, List<Bidder
    > bidders);
    Result StartAuctionAsync(Guid auctionId);
    Result EndAuctionAsync(Guid auctionId);
    Task<Result> PlaceBidAsync(Guid auctionId, Bidder bidder, decimal bidAmount);
}
