using cams.application.config;
using cams.application.models;
using FluentResults;
using Microsoft.Extensions.Options;

namespace cams.application.repositories;

public class AuctionRepository : IAuctionRepository
{
    private static List<Auction> _auctions = [];
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

    public Task<Auction> CreateAuctionAsync(Guid auctionId, Vehicle vehicle, List<Bidder> bidders)
    {
        Auction newAuction = new Auction
        {
            Id = auctionId,
            Name = $"Auction for {vehicle.VehicleAttributes.Manufacturer} {vehicle.VehicleAttributes.Model}",
            StartingBid = GetStartingBid(vehicle.VehicleAttributes),
            Vehicle = vehicle,
            Bidders = bidders,
            IsActive = false,
            CurrentBid = GetStartingBid(vehicle.VehicleAttributes)
        };

        _auctions.Add(newAuction);

        return Task.FromResult(newAuction);
    }

    public Task EndAuctionAsync(Auction auction)
    {
        auction.HammerPrice = auction.CurrentBid-100;//TODO: value should be configurable. Set in code for now
        auction.IsActive = false;
        return Task.CompletedTask;
    }

    public Task<Auction> GetAuctionById(Guid auctionId)
    {
        return Task.FromResult(_auctions.FirstOrDefault(a => a.Id == auctionId));
    }

    public Task PlaceBidAsync(Auction auction, Bidder bidder, decimal bidAmount)
    {
        auction.CurrentBid = bidAmount + 100; //TODO:value should be configurable. Set in code for now
        auction.HighestBidder = bidder;

        return Task.CompletedTask;
    }

    public Task<bool> ExistingAuctionByVehicle(string vin)
    {
        return Task.FromResult(_auctions.Any(a => a.IsActive && a.Vehicle.Vin == vin));
    }

    public Task<IEnumerable<Auction>> Search(Func<Auction, bool> predicate)
    {
        return Task.FromResult(_auctions.Where(predicate));
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