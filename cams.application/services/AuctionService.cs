using cams.application.models;
using cams.application.repositories;
using FluentResults;

namespace cams.application.services;

public class AuctionService : IAuctionService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IAuctionRepository _auctionRepository;
    private readonly IBidderRepository _bidderRepository;

    public AuctionService(IVehicleRepository vehicleRepository, IAuctionRepository auctionRepository,
        IBidderRepository bidderRepository)
    {
        _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        _auctionRepository = auctionRepository ?? throw new ArgumentNullException(nameof(auctionRepository));
        _bidderRepository = bidderRepository ?? throw new ArgumentNullException(nameof(bidderRepository));
    }

    public async Task<Result<Auction>> CreateAuctionAsync(Guid auctionId, Vehicle vehicle, List<Bidder> bidders)
    {
        if (bidders == null || bidders.Count == 0)
        {
            return Result.Fail<Auction>(new Error("At least one bidder must be registered for the auction."));
        }

        //check bidders existence
        if (bidders.Any(b => b == null || b.Id == Guid.Empty || string.IsNullOrWhiteSpace(b.Name)))
        {
            return Result.Fail<Auction>(new Error("All bidders must have a valid ID and name."));
        }

        foreach (var bidder in bidders)
        {
            var existingBidder = await _bidderRepository.GetBidderByIdAsync(bidder.Id);
            if (existingBidder == null)
            {
                return Result.Fail<Auction>(new Error($"Bidder with ID {bidder.Id} does not exist."));
            }
        }
        //check vehicle existence

        var selectedVehicle = await _vehicleRepository.GetVehicleByVinAsync(vehicle.Vin);
        if (selectedVehicle == null)
        {
            return Result.Fail<Auction>(new Error("Invalid vehicle for auction. Vehicle must exist in the inventory."))
                .Value;
        }

        bool isVehicleInActiveAuction = await _auctionRepository.ExistingAuctionByVehicle(vehicle.Vin);
        if (isVehicleInActiveAuction)
        {
            return Result.Fail<Auction>(new Error("Vehicle is already in an active auction."));
        }

        var newAuction = await _auctionRepository.CreateAuctionAsync(auctionId, vehicle, bidders);
        if (newAuction == null)
        {
            return Result.Fail<Auction>(new Error("Failed to create auction. Please try again."));
        }

        return Result.Ok(newAuction);
    }

    public async Task<Result> StartAuctionAsync(Guid auctionId)
    {
        try
        {
            var auction = await _auctionRepository.GetAuctionById(auctionId);
            if (auction == null)
            {
                return Result.Fail(new Error("Auction does not exist."));
            }

            if (auction.IsActive)
            {
                return Result.Fail(new Error("Auction is already active."));
            }

            var doesVehicleExist = await _vehicleRepository.GetVehicleByVinAsync(auction.Vehicle.Vin);
            var isCarInActiveAuction = await _auctionRepository.ExistingAuctionByVehicle(auction.Vehicle.Vin);
            if (doesVehicleExist != null && isCarInActiveAuction)
            {
                return Result.Fail(new Error("Vehicle is already in an active auction."));
            }


            await _auctionRepository.StartAuctionAsync(auction);
        }
        catch (Exception e)
        {
            return Result.Fail(new Error(e.Message));
        }

        return Result.Ok();
    }

    public Result EndAuctionAsync(Guid auctionId)
    {
        try
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

            _auctionRepository.EndAuctionAsync(existing.Result);
        }
        catch (Exception e)
        {
            return Result.Fail(new Error(e.Message));
        }

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
            return Result.Fail(new Error(
                $"Bid amount must be greater than the starting bid {auction.StartingBid} and current bid - {auction.CurrentBid}."));
        }

        await _auctionRepository.PlaceBidAsync(auction, bidder, bidAmount);

        return Result.Ok();
    }

    public async Task<IEnumerable<Auction>> Search(Func<Auction, bool> predicate)
    {
        return await _auctionRepository.Search(predicate);
    }
}