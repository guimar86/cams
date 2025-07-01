namespace cams.contracts.Requests.Auctions;

public class SearchAuctionRequest
{
    public Guid AuctionId { get; set; }
    public string Vin { get; set; }
}