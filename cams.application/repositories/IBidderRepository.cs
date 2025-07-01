using cams.application.models;

namespace cams.application.repositories;

public interface IBidderRepository
{
    /// <summary>
    /// Creates a new bidder with the specified ID and name.
    /// </summary>
    /// <param name="bidderId">The unique identifier for the bidder.</param>
    /// <param name="name">The name of the bidder.</param>
    /// <returns>The created <see cref="Bidder"/> object.</returns>
    Task<Bidder> CreateBidderAsync(Guid bidderId, string name);

    /// <summary>
    /// Retrieves a bidder by their unique identifier.
    /// </summary>
    /// <param name="bidderId">The unique identifier of the bidder.</param>
    /// <returns>The <see cref="Bidder"/> object if found; otherwise, null.</returns>
    Task<Bidder> GetBidderByIdAsync(Guid bidderId);

    /// <summary>
    /// Retrieves a list of all bidders.
    /// </summary>
    /// <returns>A list of all <see cref="Bidder"/> objects.</returns>
    Task<List<Bidder>> GetAllBiddersAsync();
}