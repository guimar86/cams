namespace cams.contracts.models;

/// <summary>
/// Represents an auction for a vehicle, including its details, bidders, and current state.
/// </summary>
public class Auction
{
    /// <summary>
    /// Unique identifier for the auction.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Name or title of the auction.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The starting bid amount for the auction.
    /// </summary>
    public decimal StartingBid { get; set; }

    /// <summary>
    /// The vehicle being auctioned.
    /// </summary>
    public Vehicle Vehicle { get; set; }

    /// <summary>
    /// List of bidders participating in the auction.
    /// </summary>
    public List<Bidder> Bidders { get; set; }

    /// <summary>
    /// The bidder with the highest bid.
    /// </summary>
    public Bidder HighestBidder { get; set; }

    /// <summary>
    /// Indicates whether the auction is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// The current highest bid in the auction.
    /// </summary>
    public decimal CurrentBid { get; set; }

    /// <summary>
    /// The final sale price when the auction ends.
    /// </summary>
    public decimal HammerPrice { get; set; }
}