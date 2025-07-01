using System;

namespace cams.contracts.Requests.Auctions;

public record CreateAuctionRequest (string Vin,  List<string> Bidders);
