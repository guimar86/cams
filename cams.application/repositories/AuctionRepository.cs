using cams.application.config;
using cams.application.models;
using FluentResults;
using Microsoft.Extensions.Options;

namespace cams.application.repositories;

public class AuctionRepository : IAuctionRepository
{
    List<Auction> _auctions = [];
    private readonly IVehicleRepository _vehicleRepository;
    private readonly Dictionary<string, decimal> _startingBids;

    public AuctionRepository(IVehicleRepository vehicleRepository, IOptions<VehicleBidSettings> vehicleBidSettings)
    {
        _startingBids = vehicleBidSettings.Value.StartingBids;
        if (_startingBids == null || !_startingBids.Any())
        {
            throw new ArgumentException("Starting bids configuration is missing or empty.");
        }

        _vehicleRepository = vehicleRepository ??
                             throw new ArgumentNullException(nameof(vehicleRepository),
                                 "Vehicle repository cannot be null.");
    }

    public async Task<Result<Auction>> CreateAuctionAsync(Guid auctionId, Vehicle vehicle, DateTime startTime,
        DateTime endTime, List<Bidder> bidders)
    {
        var isValidVehicle = await _vehicleRepository.GetVehicleByVinAsync(vehicle.Vin);
        if (!isValidVehicle.IsFailed)
        {
            return Result.Fail<Auction>(new Error("Invalid vehicle for auction. Vehicle must exist in the inventory."))
                .Value;
        }

        bool isVehicleInActiveAuction = _auctions.Any(a => a.Vehicle.Vin == vehicle.Vin && a.IsActive);

        if (isVehicleInActiveAuction)
        {
            return Result.Fail<Auction>(new Error("Vehicle is already in an active auction.")).Value;
        }


        Auction newAuction = new Auction
        {
            Id = auctionId,
            Name = $"Auction for {vehicle.VehicleAttributes.Manufacturer} {vehicle.VehicleAttributes.Model}",
            StartingBid = GetStartingBid(vehicle.VehicleAttributes),
            Vehicle = vehicle,
            Bidders = bidders,
            IsActive = false,
            StartTime = startTime,
            EndTime = endTime
        };

        _auctions.Add(newAuction);

        return Result.Ok(newAuction);
    }

    public Result EndAuctionAsync(Guid auctionId)
    {
        Auction auction = _auctions.FirstOrDefault(a => a.Id == auctionId && a.IsActive == true);
        if (auction == null)
        {
            return Result.Fail(new Error("Auction not found or already ended."));
        }

        auction.IsActive = false;
        return Result.Ok();
    }

    public Task<Result> PlaceBidAsync(Guid auctionId, Bidder bidder, decimal bidAmount)
    {
        Auction auction = _auctions.FirstOrDefault(a => a.Id == auctionId && a.IsActive == true);

        if (auction == null)
        {
            return Task.FromResult(Result.Fail(new Error("Auction not found or not active.")));
        }

        bool isBidderRegistered = _auctions.Any(a => a.Id == auctionId && a.Bidders.Any(b => b.Id == bidder.Id));
        if (!isBidderRegistered)
        {
            return Task.FromResult(Result.Fail(new Error("Bidder is not registered for this auction.")));
        }

        bool isBidAmountValid = bidAmount > auction.StartingBid && bidAmount > auction.CurrentBid;

        if (!isBidAmountValid)
        {
            return Task.FromResult(Result.Fail(
                new Error("Bid amount must be greater than the starting bid and the current highest bid.")));
        }

        auction.CurrentBid = bidAmount + 100; // value should be configurable. Set in code for now
        auction.HighestBidder = bidder;

        return Task.FromResult(Result.Ok());
    }

    public Result StartAuctionAsync(Guid auctionId)
    {
        Auction auction = _auctions.FirstOrDefault(a => a.Id == auctionId && a.IsActive == true);
        if (auction == null)
        {
            return Result.Fail(new Error("Auction not found or already active."));
        }

        auction.IsActive = true;
        return Result.Ok();
    }

    private decimal GetStartingBid(BaseVehicleAttributes attributes)
    {
        var typeName = attributes.GetType().Name.Replace("Attributes", "");

        if (_startingBids.TryGetValue(typeName, out var bid))
            return bid;

        throw new InvalidOperationException($"No starting bid defined for vehicle type: {typeName}");
    }
}