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
        if (isValidVehicle == null)
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

    public Task EndAuctionAsync(Auction auction)
    {
        auction.IsActive = false;
        return Task.CompletedTask;
    }

    public Task<Auction> GetAuctionById(Guid auctionId)
    {
        return Task.FromResult(_auctions.FirstOrDefault(a => a.Id == auctionId));
    }

    public Task PlaceBidAsync(Auction auction, Bidder bidder, decimal bidAmount)
    {
        auction.CurrentBid = bidAmount + 100; // value should be configurable. Set in code for now
        auction.HighestBidder = bidder;

        return Task.CompletedTask;
    }

    public Task<bool> ExistingAuctionAsync(Guid auctionId)
    {
        return Task.FromResult(_auctions.Any(a => a.IsActive && a.Id == auctionId));
    }

    public Task StartAuctionAsync(Auction auction)
    {
        auction.IsActive = true;
        return Task.CompletedTask;
    }



    private decimal GetStartingBid(BaseVehicleAttributes attributes)
    {
        var typeName = attributes.GetType().Name.Replace("Attributes", "");

        if (_startingBids.TryGetValue(typeName, out var bid))
            return bid;

        throw new InvalidOperationException($"No starting bid defined for vehicle type: {typeName}");
    }
}