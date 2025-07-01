namespace cams.contracts.Requests.Auctions;

/// <summary>
/// Request object for searching auctions.
/// </summary>
public class SearchAuctionRequest
{
    /// <summary>
    /// The unique identifier of the auction.
    /// </summary>
    public Guid AuctionId { get; set; }

    /// <summary>
    /// The vehicle identification number (VIN) to search for.
    /// </summary>
    public string Vin { get; set; }
}