using System;

namespace cams.contracts.Requests.Auctions;

public record PlaceBidRequest(Guid auctionId, Bidder bidder, decimal bidAmount);
