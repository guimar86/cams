using System;

namespace cams.contracts.Responses.Auctions;

/// <summary>
/// Response returned after creating an auction.
/// </summary>
/// <param name="AuctionId">The unique identifier of the created auction.</param>
/// <param name="StartingBid">The starting bid amount for the auction.</param>
public record CreateAuctionResponse(Guid AuctionId, decimal StartingBid);
