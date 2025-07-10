using cams.contracts.Config;
using cams.contracts.models;
using cams.contracts.Repositories;
using Microsoft.Extensions.Options;

namespace cams.infrastructure.repositories;

public class AuctionRepository : IAuctionRepository
{
    private static List<Auction> _auctions = [];
    private readonly Dictionary<string, decimal> _startingBids;
    private readonly AuctionSettings _auctionSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuctionRepository"/> class.
    /// </summary>
    /// <param name="vehicleBidSettings">The vehicle bid settings containing starting bids configuration.</param>
    /// <param name="auctionSettings">Settings used by auctions</param>
    /// <exception cref="ArgumentException">Thrown when starting bids configuration is missing or empty.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the vehicle repository is null.</exception>
    public AuctionRepository(IOptions<VehicleBidSettings> vehicleBidSettings, IOptions<AuctionSettings> auctionSettings)
    {
        _auctionSettings = auctionSettings.Value;
        _startingBids = vehicleBidSettings.Value.StartingBids;
        if (_startingBids == null || !_startingBids.Any())
        {
            throw new ArgumentException("Starting bids configuration is missing or empty.");
        }
    }

    /// <inheritdoc/>
    public Task<Auction> CreateAuctionAsync(Guid auctionId, Vehicle vehicle, List<Bidder> bidders)
    {
        Auction newAuction = new Auction
        {
            Id = auctionId,
            Name = $"Auction for {vehicle.Manufacturer} {vehicle.Model}",
            StartingBid = GetStartingBid(vehicle),
            Vehicle = vehicle,
            Bidders = bidders,
            IsActive = false,
            CurrentBid = GetStartingBid(vehicle)
        };

        _auctions.Add(newAuction);

        return Task.FromResult(newAuction);
    }

    /// <inheritdoc/>
    public Task EndAuctionAsync(Auction auction)
    {
        auction.HammerPrice = auction.CurrentBid - _auctionSettings.BidIncrement;
        auction.IsActive = false;
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task<Auction> GetAuctionById(Guid auctionId)
    {
        return Task.FromResult(_auctions.FirstOrDefault(a => a.Id == auctionId));
    }

    /// <inheritdoc/>
    public Task PlaceBidAsync(Auction auction, Bidder bidder, decimal bidAmount)
    {
        auction.CurrentBid = bidAmount + _auctionSettings.BidIncrement;
        auction.HighestBidder = bidder;

        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public Task<bool> ExistingAuctionByVehicle(string vin)
    {
        return Task.FromResult(_auctions.Any(a => a.IsActive && a.Vehicle.Reference == vin));
    }

    /// <inheritdoc/>
    public Task<IEnumerable<Auction>> Search(Func<Auction, bool> predicate)
    {
        return Task.FromResult(_auctions.Where(predicate));
    }

    /// <inheritdoc/>
    public Task StartAuctionAsync(Auction auction)
    {
        auction.IsActive = true;
        return Task.CompletedTask;
    }


    /// <summary>
    /// Get starting bid for a vehicle based on its type.
    /// </summary>
    /// <param name="vehicle"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private decimal GetStartingBid(Vehicle vehicle)
    {
        var typeName = vehicle.GetType().Name;

        if (_startingBids.TryGetValue(typeName, out var bid))
            return bid;

        throw new InvalidOperationException($"No starting bid defined for vehicle type: {typeName}");
    }
}