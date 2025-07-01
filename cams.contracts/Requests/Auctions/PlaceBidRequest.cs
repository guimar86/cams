using System;

namespace cams.contracts.Requests.Auctions;

/// <summary>
/// Represents a request to place a bid in an auction.
/// </summary>
/// <param name="AuctionId">The unique identifier of the auction.</param>
/// <param name="BidderId">The unique identifier of the bidder.</param>
/// <param name="BidAmount">The amount of the bid.</param>
public record PlaceBidRequest(Guid AuctionId, Guid BidderId, decimal BidAmount);
