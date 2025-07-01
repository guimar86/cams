using System;

namespace cams.contracts.Requests.Auctions;

public record PlaceBidRequest(Guid AuctionId, Guid BidderId, decimal BidAmount);
