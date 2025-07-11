using cams.contracts.models;
using cams.contracts.Repositories;
using cams.contracts.Requests.Auctions;
using cams.contracts.Search;
using FluentResults;

namespace cams.application.services;

public class AuctionService : IAuctionService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IAuctionRepository _auctionRepository;
    private readonly IBidderRepository _bidderRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuctionService"/> class.
    /// </summary>
    /// <param name="vehicleRepository">The vehicle repository.</param>
    /// <param name="auctionRepository">The auction repository.</param>
    /// <param name="bidderRepository">The bidder repository.</param>
    public AuctionService(IVehicleRepository vehicleRepository, IAuctionRepository auctionRepository,
        IBidderRepository bidderRepository)
    {
        _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        _auctionRepository = auctionRepository ?? throw new ArgumentNullException(nameof(auctionRepository));
        _bidderRepository = bidderRepository ?? throw new ArgumentNullException(nameof(bidderRepository));
    }

    /// <inheritdoc/>
    public async Task<Result<Auction>> CreateAuctionAsync(CreateAuctionRequest request)
    {
        if (request.Bidders == null || request.Bidders.Count == 0)
        {
            return Result.Fail<Auction>(new Error("At least one bidder must be registered for the auction."));
        }

        //check bidders existence
        if (request.Bidders.Any(b => b.Equals(Guid.Empty)))
        {
            return Result.Fail<Auction>(new Error("All bidders must have a valid ID and name."));
        }

        bool anyInvalidBidder = false;
        List<Bidder> bidders = [];
        foreach (var bidder in request.Bidders)
        {
            var bidderEntity = await _bidderRepository.GetBidderByIdAsync(bidder);
            if (bidderEntity == null)
            {
                anyInvalidBidder = true;
                break;
            }
            bidders.Add(bidderEntity);

        }

        if (anyInvalidBidder)
        {
            return Result.Fail<Auction>(new Error($"Bidder with ID {anyInvalidBidder} does not exist."));
        }
        //check vehicle existence

        var selectedVehicle = await _vehicleRepository.GetVehicleByVinAsync(request.Vin);
        if (selectedVehicle == null)
        {
            return Result.Fail<Auction>(new Error("Invalid vehicle for auction. Vehicle must exist in the inventory."));
        }

        bool isVehicleInActiveAuction = await _auctionRepository.ExistingAuctionByVehicle(request.Vin);
        if (isVehicleInActiveAuction)
        {
            return Result.Fail<Auction>(new Error("Vehicle is already in an active auction."));
        }

        var newAuction = await _auctionRepository.CreateAuctionAsync(Guid.NewGuid(), selectedVehicle, bidders);
        if (newAuction == null)
        {
            return Result.Fail<Auction>(new Error("Failed to create auction. Please try again."));
        }

        return Result.Ok(newAuction);
    }

    /// <inheritdoc/>
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

            var doesVehicleExist = await _vehicleRepository.GetVehicleByVinAsync(auction.Vehicle.Reference);
            var isCarInActiveAuction = await _auctionRepository.ExistingAuctionByVehicle(auction.Vehicle.Reference);
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

    /// <inheritdoc/>
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

    /// <inheritdoc/>
    public async Task<Result> PlaceBidAsync(PlaceBidRequest request)
    {

        Auction auction = await _auctionRepository.GetAuctionById(request.AuctionId);
        Bidder bidder = await _bidderRepository.GetBidderByIdAsync(request.BidderId);

        if (auction == null || !auction.IsActive)
        {
            return Result.Fail(new Error("Auction not found or not active."));
        }

        if (!auction.Bidders.Contains(bidder))
        {
            return Result.Fail(new Error("Bidder is not registered for this auction."));
        }

        bool isBidAmountValid = request.BidAmount > auction.StartingBid && request.BidAmount > auction.CurrentBid;
        if (!isBidAmountValid)
        {
            return Result.Fail(new Error(
                $"Bid amount must be greater than the starting bid {auction.StartingBid} and current bid - {auction.CurrentBid}."));
        }

        await _auctionRepository.PlaceBidAsync(auction, bidder, request.BidAmount);

        return Result.Ok();
    }

    /// <inheritdoc/>
    public async Task<Result<IEnumerable<Auction>>> Search(SearchAuctionRequest request)
    {
        var searches = new List<ISearch<Auction>>();

        if (request.AuctionId != Guid.Empty)
        {
            searches.Add(new AuctionIdSearch(request.AuctionId));
        }
        if (!string.IsNullOrWhiteSpace(request.Vin))
        {
            searches.Add(new VinSearch(request.Vin));
        }

        var search = new SearchAggregator<Auction>(searches);

        var result = await _auctionRepository.Search(search.Match);

        return Result.Ok(result);
    }


}