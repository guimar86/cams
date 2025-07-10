using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using cams.application.services;
using cams.contracts.models;
using cams.contracts.Requests.Auctions;
using cams.contracts.Responses.Auctions;
using Microsoft.AspNetCore.Mvc;

namespace cams.api.Controllers;

/// <summary>
/// Controller for managing auction-related endpoints.
/// </summary>
[Produces("application/json")]
[Consumes("application/json")]
[ApiController]
[Route("auctions")]
public class AuctionsController : ControllerBase
{
    private readonly IAuctionService _auctionService;
    private readonly IVehicleService _vehicleService;
    private readonly IBidderService _bidderService;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuctionsController"/> class.
    /// </summary>
    /// <param name="auctionService">Service for auction operations.</param>
    /// <param name="vehicleService">Service for vehicle operations.</param>
    /// <param name="bidderService">Service for bidder operations.</param>
    public AuctionsController(IAuctionService auctionService, IVehicleService vehicleService,
        IBidderService bidderService)
    {
        _auctionService = auctionService ??
                          throw new ArgumentNullException(nameof(auctionService), "Auction service cannot be null.");
        _vehicleService = vehicleService ??
                          throw new ArgumentNullException(nameof(vehicleService), "Vehicle service cannot be null.");
        _bidderService = bidderService ??
                         throw new ArgumentNullException(nameof(bidderService), "Bidder service cannot be null.");
    }


    /// <summary>
    /// Retrieves a list of auctions based on the specified search criteria.
    /// </summary>
    /// <param name="request">The search criteria for auctions.</param>
    /// <returns>A list of auctions matching the search criteria.</returns>
    [HttpGet]
    [Route("", Name = "GetAuctions")]
    [ProducesResponseType(typeof(IEnumerable<Auction>), 200)]
    public async Task<IActionResult> GetAuctionsAsync([FromQuery] SearchAuctionRequest request)
    {
        var auctions = await _auctionService.Search(v =>
            (request.AuctionId == Guid.Empty || v.Id == request.AuctionId) &&
            (string.IsNullOrEmpty(request.Vin) ||
             v.Vehicle.Reference.Equals(request.Vin, StringComparison.OrdinalIgnoreCase)));

        return Ok(auctions);
    }

    /// <summary>
    /// Creates a new auction with the specified vehicle and bidders.
    /// </summary>
    /// <param name="request">The request containing auction details, including VIN and bidder IDs.</param>
    /// <returns>
    /// Returns <see cref="CreateAuctionResponse"/> with the new auction ID and starting bid if successful; 
    /// otherwise, returns a 400 Bad Request with error details.
    /// </returns>
    [HttpPost]
    [Route("", Name = "CreateAuction")]
    [ProducesResponseType(typeof(CreateAuctionResponse), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IActionResult> CreateAuctionAsync([Required] [FromBody] CreateAuctionRequest request)
    {
        var vehicle = await _vehicleService.GetVehicleByVinAsync(request.Vin);
        if (vehicle.IsFailed)
        {
            return BadRequest(vehicle.Errors);
        }

        List<Bidder> bidders =
            request.Bidders.ConvertAll(p => _bidderService.GetBidderByIdAsync(Guid.Parse(p)).Result.Value);

        Guid auctionId = Guid.NewGuid();
        var result = await _auctionService.CreateAuctionAsync(auctionId, vehicle.Value, bidders);

        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }

        return Ok(new CreateAuctionResponse(auctionId, result.Value.StartingBid));
    }

    /// <summary>
    /// Starts an auction with the specified auction ID.
    /// </summary>
    /// <param name="auctionId">The unique identifier of the auction to start.</param>
    /// <returns>
    /// Returns 200 OK if the auction is started successfully; otherwise, returns 400 Bad Request with error details.
    /// </returns>
    [HttpPut]
    [Route("start/{auctionId}", Name = "StartAuction")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IActionResult> StartAuctionAsync([Required] [FromRoute] Guid auctionId)
    {
        var result = await _auctionService.StartAuctionAsync(auctionId);

        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }

        return Ok();
    }

    /// <summary>
    /// Ends an auction with the specified auction ID.
    /// </summary>
    /// <param name="auctionId">The unique identifier of the auction to end.</param>
    /// <returns>
    /// Returns 200 OK if the auction is ended successfully; otherwise, returns 400 Bad Request with error details.
    /// </returns>
    [HttpPut]
    [Route("end/{auctionId}", Name = "EndAuction")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(string), 400)]
    public IActionResult EndAuctionAsync([Required] [FromRoute] Guid auctionId)
    {
        var result = _auctionService.EndAuctionAsync(auctionId);

        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }

        return Ok();
    }

    /// <summary>
    /// Places a bid on an auction for a specified bidder and bid amount.
    /// </summary>
    /// <param name="request">The request containing the auction ID, bidder ID, and bid amount.</param>
    /// <returns>
    /// Returns 200 OK if the bid is placed successfully; otherwise, returns 400 Bad Request with error details.
    /// </returns>
    [HttpPost]
    [Route("place-bid", Name = "PlaceBid")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IActionResult> PlaceBidAsync([Required] [FromBody] PlaceBidRequest request)
    {
        var bidder = await _bidderService.GetBidderByIdAsync(request.BidderId);
        if (bidder.IsFailed)
        {
            return BadRequest(bidder.Errors);
        }

        var result = await _auctionService.PlaceBidAsync(request.AuctionId, bidder.Value, request.BidAmount);

        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }

        return Ok();
    }
}