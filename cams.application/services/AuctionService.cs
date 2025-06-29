using cams.application.models;
using cams.application.repositories;
using FluentResults;

namespace cams.application.services;

public class AuctionService : IAuctionService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IAuctionRepository _auctionRepository;

    public AuctionService(IVehicleRepository vehicleRepository, IAuctionRepository auctionRepository)
    {
        _vehicleRepository = vehicleRepository;
        _auctionRepository = auctionRepository;
    }

    public Task<Result<Auction>> CreateAuctionAsync(Guid auctionId, Vehicle vehicle, DateTime startTime,
        DateTime endTime, List<Bidder> bidders)
    {
        throw new NotImplementedException();
    }

    public Result StartAuctionAsync(Guid auctionId)
    {
        var existing = _auctionRepository.GetAuctionById(auctionId);
        if (existing.Result == null)
        {
            return Result.Fail(new Error("Auction does not exist."));
        }

        if (existing.Result.IsActive == true)
        {
            return Result.Fail(new Error("Auction is already active."));
        }

        if (existing.Result.StartTime > DateTime.UtcNow)
        {
            return Result.Fail(new Error("Auction cannot be started before the start time."));
        }

        _auctionRepository.StartAuctionAsync(existing.Result);
        return Result.Ok();
    }

    public Result EndAuctionAsync(Guid auctionId)
    {
        var existing = _auctionRepository.GetAuctionById(auctionId);
        if (existing.Result == null)
        {
            return Result.Fail(new Error("Auction does not exist."));
        }

        if (!existing.Result.IsActive)
        {
            return Result.Fail(new Error("Auction is not active."));
        }

        if (existing.Result.EndTime < DateTime.UtcNow)
        {
            return Result.Fail(new Error("Auction cannot be ended before the end time."));
        }

        _auctionRepository.EndAuctionAsync(existing.Result);
        return Result.Ok();
    }

    public async Task<Result> PlaceBidAsync(Guid auctionId, Bidder bidder, decimal bidAmount)
    {
        Auction auction = await _auctionRepository.GetAuctionById(auctionId);

        if (auction == null || auction.IsActive == false)
        {
            return Result.Fail(new Error("Auction not found or not active."));
        }

        if (!auction.Bidders.Contains(bidder))
        {
            return Result.Fail(new Error("Bidder is not registered for this auction."));
        }

        bool isBidAmountValid = bidAmount > auction.StartingBid && bidAmount > auction.CurrentBid;
        if (!isBidAmountValid)
        {
            return Result.Fail(new Error("Bid amount must be greater than the starting bid and current bid."));
        }
        await _auctionRepository.PlaceBidAsync(auction, bidder, bidAmount);

        return Result.Ok();
    }
}