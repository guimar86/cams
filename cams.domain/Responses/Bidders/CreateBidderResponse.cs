namespace cams.contracts.Responses.Bidders;

/// <summary>
/// Response returned after creating a new bidder.
/// </summary>
/// <param name="BidderId">The unique identifier of the created bidder.</param>
/// <param name="Name">The name of the created bidder.</param>
public record CreateBidderResponse (Guid BidderId, string Name);