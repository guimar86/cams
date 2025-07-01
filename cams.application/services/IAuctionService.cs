using cams.application.models;
using FluentResults;

namespace cams.application.services;

public interface IAuctionService
{
    Task<Result<Auction>> CreateAuctionAsync(Guid auctionId, Vehicle vehicle, List<Bidder> bidders);

    Task<Result> StartAuctionAsync(Guid auctionId);
    Result EndAuctionAsync(Guid auctionId);
    Task<Result> PlaceBidAsync(Guid auctionId, Bidder bidder, decimal bidAmount);
    Task<IEnumerable<Auction>> Search(Func<Auction, bool> predicate);
}