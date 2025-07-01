using cams.application.models;
using FluentResults;

namespace cams.application.services;

public interface IBidderService
{
    /// <summary>
    /// Retrieves a bidder by their unique identifier.
    /// </summary>
    /// <param name="bidderId">The unique identifier of the bidder.</param>
    /// <returns>A result containing the bidder if found, or an error if not found.</returns>
    Task<Result<Bidder>> GetBidderByIdAsync(Guid bidderId);

    /// <summary>
    /// Creates a new bidder with the specified ID and name.
    /// </summary>
    /// <param name="bidderId">The unique identifier for the new bidder.</param>
    /// <param name="name">The name of the bidder.</param>
    /// <returns>A result containing the created bidder or an error.</returns>
    Task<Result<Bidder>> CreateBidderAsync(Guid bidderId, string name);

    /// <summary>
    /// Retrieves all registered bidders.
    /// </summary>
    /// <returns>A result containing a list of all bidders.</returns>
    Task<Result<List<Bidder>>> GetAllBiddersAsync();
}