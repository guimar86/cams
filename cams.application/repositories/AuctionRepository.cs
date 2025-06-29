using cams.application.models;
using FluentResults;

namespace cams.application.repositories;

public class AuctionRepository : IAuctionRepository
{

    List<Auction> auctions = [];
    private readonly IVehicleRepository vehicleRepository;

    public AuctionRepository(IVehicleRepository vehicleRepository)
    {
        this.vehicleRepository = vehicleRepository;
    }

    public async Task<Result<Auction>> CreateAuctionAsync(Guid auctionId, Vehicle vehicle, DateTime startTime, DateTime endTime, List<Bidder> bidders)
    {
        var isValidVehicle = await vehicleRepository.GetVehicleByVinAsync(vehicle.Vin);
        if (!isValidVehicle.IsFailed)
        {
            return Result.Fail<Auction>(new Error("Invalid vehicle for auction. Vehicle must exist in the inventory.")).Value;

        }
        bool isVehicleInActiveAuction = auctions.Any(a => a.Vehicle.Vin == vehicle.Vin && a.IsActive);

        if (isVehicleInActiveAuction)
        {
            return Result.Fail<Auction>(new Error("Vehicle is already in an active auction.")).Value;
        }


        Auction newAuction = new Auction
        {
            Id = auctionId,
            Name = $"Auction for {vehicle.VehicleAttributes.Manufacturer} {vehicle.VehicleAttributes.Model}",
            StartingBid = vehicle.VehicleAttributes.StartingBid, //TODO: create factory method to set starting bid based on vehicle attributes
            Vehicle = vehicle,
            Bidders = bidders,
            IsActive = false,
            StartTime = startTime,
            EndTime = endTime
        };

        auctions.Add(newAuction);

        return Result.Ok(newAuction);
    }

    public Task<Result> EndAuctionAsync(Guid auctionId)
    {
        Auction auction = auctions.FirstOrDefault(a => a.Id == auctionId && a.IsActive == true);
        if (auction == null)
        {
            return Task.FromResult(Result.Fail(new Error("Auction not found or already ended.")));
        }
        auction.IsActive = false;
        return Task.FromResult(Result.Ok());
    }

    public Task<Result> PlaceBidAsync(Guid auctionId, Bidder bidder, decimal bidAmount)
    {
        Auction auction = auctions.FirstOrDefault(a => a.Id == auctionId && a.IsActive == true);

        if (auction == null)
        {
            return Task.FromResult(Result.Fail(new Error("Auction not found or not active.")));
        }

        bool isBidderRegistered = auctions.Any(a => a.Id == auctionId && a.Bidders.Any(b => b.Id == bidder.Id));
        if (!isBidderRegistered)
        {
            return Task.FromResult(Result.Fail(new Error("Bidder is not registered for this auction.")));
        }

        bool isBidAmountValid = bidAmount > auction.StartingBid && bidAmount > auction.CurrentBid;

        if (!isBidAmountValid)
        {
            return Task.FromResult(Result.Fail(new Error("Bid amount must be greater than the starting bid and the current highest bid.")));
        }

        auction.CurrentBid = bidAmount + 100; // value should be configurable. Set in code for now
        auction.HighestBidder = bidder;

        return Task.FromResult(Result.Ok());

    }

    public Task<Result> StartAuctionAsync(Guid auctionId)
    {
        Auction auction = auctions.FirstOrDefault(a => a.Id == auctionId && a.IsActive == true);
        if (auction == null)
        {
            return Task.FromResult(Result.Fail(new Error("Auction not found or already active.")));
        }
        auction.IsActive = true;
        return Task.FromResult(Result.Ok());

    }

    
}
