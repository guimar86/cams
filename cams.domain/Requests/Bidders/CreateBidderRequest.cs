namespace cams.contracts.Requests.Bidders;

/// <summary>
/// Request to create a new bidder.
/// </summary>
/// <param name="Name">The name of the bidder.</param>
public record CreateBidderRequest(string Name);
