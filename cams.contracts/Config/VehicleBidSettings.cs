namespace cams.contracts.Config;

/// <summary>
/// Represents the bid settings for vehicles, including starting bids for each vehicle type.
/// </summary>
public class VehicleBidSettings
{
    /// <summary>
    /// Gets or sets the starting bids for vehicles, keyed by vehicle type.
    /// </summary>
    public Dictionary<string, decimal> StartingBids { get; set; }
}