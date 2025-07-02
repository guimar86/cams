using cams.contracts.models;
using cams.contracts.Repositories;
using FluentResults;

namespace cams.application.services;

public class BidderService : IBidderService
{
    private readonly IBidderRepository _bidderRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="BidderService"/> class.
    /// </summary>
    /// <param name="bidderRepository">The repository used for bidder data access.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="bidderRepository"/> is null.</exception>
    public BidderService(IBidderRepository bidderRepository)
    {
        _bidderRepository = bidderRepository ?? throw new ArgumentNullException(nameof(bidderRepository));
    }

    /// <inheritdoc/>
    public async Task<Result<Bidder>> GetBidderByIdAsync(Guid bidderId)
    {
        if (bidderId == Guid.Empty)
        {
            return Result.Fail<Bidder>(new Error("Bidder ID cannot be empty."));
        }

        var bidder = await _bidderRepository.GetBidderByIdAsync(bidderId);
        if (bidder == null)
        {
            return Result.Fail<Bidder>(new Error($"Bidder with ID {bidderId} does not exist."));
        }

        return Result.Ok(bidder);
    }

    /// <inheritdoc/>
    public async Task<Result<Bidder>> CreateBidderAsync(Guid bidderId, string name)
    {
        if (bidderId == Guid.Empty)
        {
            return Result.Fail<Bidder>(new Error("Bidder ID cannot be empty."));
        }

        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Fail<Bidder>(new Error("Bidder name cannot be empty."));
        }

        var bidder = new Bidder(bidderId, name);
        await _bidderRepository.CreateBidderAsync(bidderId, name);
        return Result.Ok(bidder);
    }

    /// <inheritdoc/>
    public async Task<Result<List<Bidder>>> GetAllBiddersAsync()
    {
        var bidders = await _bidderRepository.GetAllBiddersAsync();
        
        return bidders.Any()
            ? Result.Ok(bidders)
            : Result.Fail<List<Bidder>>(new Error("No bidders found."));
    }
}