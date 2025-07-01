using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using cams.application.models;
using cams.application.services;
using cams.contracts.Requests.Auctions;
using cams.contracts.Responses.Auctions;
using Microsoft.AspNetCore.Mvc;

namespace cams.api.Controllers;

[ApiController]
[Route("auctions")]
public class AuctionsController : ControllerBase
{
    private readonly IAuctionService _auctionService;
    private readonly IVehicleService _vehicleService;
    private readonly IBidderService _bidderService;

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


    [HttpGet]
    [Route("", Name = "GetAuctions")]
    public async Task<IActionResult> GetAuctionsAsync([FromQuery] SearchAuctionRequest request)
    {
        var auctions= await _auctionService.Search(v=>
            (request.AuctionId == Guid.Empty || v.Id == request.AuctionId) &&
            (string.IsNullOrEmpty(request.Vin) || v.Vehicle.Vin.Equals(request.Vin, StringComparison.OrdinalIgnoreCase)));
        
        return Ok(auctions);
    }

    [HttpPost]
    [Route("", Name = "CreateAuction")]
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

        return Ok(new CreateAuctionResponse(auctionId,result.Value.StartingBid));
    }

    [HttpPut]
    [Route("start/{auctionId}", Name = "StartAuction")]
    public async Task<IActionResult> StartAuctionAsync([Required] [FromRoute] Guid auctionId)
    {
        var result = await _auctionService.StartAuctionAsync(auctionId);

        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }

        return Ok();
    }

    [HttpPut]
    [Route("end/{auctionId}", Name = "EndAuction")]
    public IActionResult EndAuctionAsync([Required] [FromRoute] Guid auctionId)
    {
        var result = _auctionService.EndAuctionAsync(auctionId);

        if (result.IsFailed)
        {
            return BadRequest(result.Errors);
        }

        return Ok();
    }

    [HttpPost]
    [Route("place-bid", Name = "PlaceBid")]
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