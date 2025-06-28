namespace cams.application.models;

public class Auction
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public decimal StartingBid { get; set; }
    public Vehicle Vehicle { get; set; }
    public List<Bidder> Bidders { get; set; }
    public Bidder HighestBidder { get; set; }
    public bool IsActive { get; set; }
}