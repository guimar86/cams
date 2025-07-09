using cams.contracts.models;

namespace cams.contracts.Repositories;

public interface IAuctionRepository
{
    /// <summary>
    /// Creates a new auction with the specified auction ID, vehicle, and list of bidders.
    /// </summary>
    /// <param name="auctionId">The unique identifier for the auction.</param>
    /// <param name="vehicle">The vehicle to be auctioned.</param>
    /// <param name="bidders">The list of bidders participating in the auction.</param>
    /// <returns>The created <see cref="Auction"/> object.</returns>
    Task<Auction> CreateAuctionAsync(Guid auctionId, Vehicle vehicle, List<Bidder> bidders);

    /// <summary>
    /// Starts the specified auction.
    /// </summary>
    /// <param name="auction">The auction to start.</param>
    Task StartAuctionAsync(Auction auction);

    /// <summary>
    /// Ends the specified auction.
    /// </summary>
    /// <param name="auction">The auction to end.</param>
    Task EndAuctionAsync(Auction auction);

    /// <summary>
    /// Retrieves an auction by its unique identifier.
    /// </summary>
    /// <param name="auctionId">The unique identifier of the auction.</param>
    /// <returns>The <see cref="Auction"/> object if found.</returns>
    Task<Auction> GetAuctionById(Guid auctionId);

    /// <summary>
    /// Places a bid on the specified auction by the given bidder and amount.
    /// </summary>
    /// <param name="auction">The auction to place a bid on.</param>
    /// <param name="bidder">The bidder placing the bid.</param>
    /// <param name="bidAmount">The amount of the bid.</param>
    Task PlaceBidAsync(Auction auction, Bidder bidder, decimal bidAmount);

    /// <summary>
    /// Checks if an auction exists for the specified vehicle VIN.
    /// </summary>
    /// <param name="vin">The vehicle identification number.</param>
    /// <returns>True if an auction exists for the vehicle; otherwise, false.</returns>
    Task<bool> ExistingAuctionByVehicle(string vin);

    /// <summary>
    /// Searches for auctions that match the specified predicate.
    /// </summary>
    /// <param name="predicate">A function to test each auction for a condition.</param>
    /// <returns>An enumerable of <see cref="Auction"/> objects that match the predicate.</returns>
    Task<IEnumerable<Auction>> Search(Func<Auction, bool> predicate);
}