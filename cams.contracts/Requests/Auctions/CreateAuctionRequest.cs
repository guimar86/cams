using System;

namespace cams.contracts.Requests.Auctions;

public record CreateAuctionRequest (Guid auctionId, string vin, DateTime startTime,
        DateTime endTime, List<Bidder> bidders);

public record Bidder(Guid id, string name);
