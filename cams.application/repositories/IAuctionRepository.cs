using System;
using cams.application.models;
using FluentResults;

namespace cams.application.repositories;

public interface IAuctionRepository
{
    Task<Result<Auction>> CreateAuctionAsync(Guid auctionId, Vehicle vehicle, DateTime startTime, DateTime endTime, List<Bidder
    > bidders);
    Task<Result> StartAuctionAsync(Guid auctionId);
    Task<Result> EndAuctionAsync(Guid auctionId);
    Task<Result> PlaceBidAsync(Guid auctionId, Bidder bidder, decimal bidAmount);
}
