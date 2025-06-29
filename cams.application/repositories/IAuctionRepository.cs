using cams.application.models;
using FluentResults;

namespace cams.application.repositories;

public interface IAuctionRepository
{
    Task<Result<Auction>> CreateAuctionAsync(Guid auctionId, Vehicle vehicle, DateTime startTime, DateTime endTime,
        List<Bidder
        > bidders);

    Task StartAuctionAsync(Auction auction);
    Task EndAuctionAsync(Auction auction);
    Task<Auction> GetAuctionById(Guid auctionId);
    Task PlaceBidAsync(Auction auction, Bidder bidder, decimal bidAmount);

    Task<bool> ExistingAuctionAsync(Guid auctionId);
}