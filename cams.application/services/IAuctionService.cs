using cams.application.models;
using FluentResults;

namespace cams.application.services;

public interface IAuctionService
{
    /// <summary>
    /// Creates a new auction with the specified auction ID, vehicle, and list of bidders.
    /// </summary>
    /// <param name="auctionId">The unique identifier for the auction.</param>
    /// <param name="vehicle">The vehicle to be auctioned.</param>
    /// <param name="bidders">The list of bidders participating in the auction.</param>
    /// <returns>A result containing the created auction or an error.</returns>
    Task<Result<Auction>> CreateAuctionAsync(Guid auctionId, Vehicle vehicle, List<Bidder> bidders);

    /// <summary>
    /// Starts the auction with the specified auction ID.
    /// </summary>
    /// <param name="auctionId">The unique identifier for the auction to start.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<Result> StartAuctionAsync(Guid auctionId);

    /// <summary>
    /// Ends the auction with the specified auction ID.
    /// </summary>
    /// <param name="auctionId">The unique identifier for the auction to end.</param>
    /// <returns>A result indicating success or failure.</returns>
    Result EndAuctionAsync(Guid auctionId);

    /// <summary>
    /// Places a bid in the specified auction.
    /// </summary>
    /// <param name="auctionId">The unique identifier for the auction.</param>
    /// <param name="bidder">The bidder placing the bid.</param>
    /// <param name="bidAmount">The amount of the bid.</param>
    /// <returns>A result indicating success or failure.</returns>
    Task<Result> PlaceBidAsync(Guid auctionId, Bidder bidder, decimal bidAmount);

    /// <summary>
    /// Searches for auctions that match the given predicate.
    /// </summary>
    /// <param name="predicate">A function to test each auction for a condition.</param>
    /// <returns>An enumerable of auctions that match the predicate.</returns>
    Task<IEnumerable<Auction>> Search(Func<Auction, bool> predicate);
}