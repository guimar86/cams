using System;

namespace cams.contracts.Requests.Auctions;

/// <summary>
/// Request to create a new auction.
/// </summary>
/// <param name="Vin">The vehicle identification number.</param>
/// <param name="Bidders">A list of bidder identifiers.</param>
public record CreateAuctionRequest (string Vin,  List<string> Bidders);
