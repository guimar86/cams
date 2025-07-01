using System;
using System.Linq;
using System.Threading.Tasks;
using cams.application.models;
using cams.application.services;
using cams.contracts.Requests.Bidders;
using Microsoft.AspNetCore.Mvc;

namespace cams.api.Controllers;

[Produces("application/json")]
[Consumes("application/json")]
[ApiController]
[Route("bidders")]
public class BiddersController : ControllerBase
{
    private readonly IBidderService _bidderService;

    public BiddersController(IBidderService bidderService)
    {
        _bidderService = bidderService;
    }

    [HttpGet]
    [Route("", Name = "GetAllBidders")]
    [ProducesResponseType(typeof(Bidder), 200)]
    [ProducesResponseType(typeof(string), 404)]
    [ProducesResponseType(204)]
    public async Task<IActionResult> GetAllBiddersAsync()
    {
        var bidders = await _bidderService.GetAllBiddersAsync();
        if (bidders.IsFailed)
        {
            return NotFound(bidders.Errors);
        }
        if (bidders.Value == null || !bidders.Value.Any())
        {
            return NoContent();
        }

        return Ok(bidders.Value);
    }

    [HttpGet]
    [Route("{id}", Name = "GetBidderById")]
    [ProducesResponseType(typeof(Bidder), 200)]
    [ProducesResponseType(typeof(string), 404)]
    public async Task<IActionResult> GetBidderByIdAsync([FromRoute] Guid id)
    {
        var result = await _bidderService.GetBidderByIdAsync(id);
        if (result.IsFailed)
        {
            return NotFound(result.Errors.FirstOrDefault()?.Message);
        }

        return Ok(result.Value);
    }

    [HttpPost]
    [Route("", Name = "CreateBidder")]
    [ProducesResponseType(typeof(Bidder), 200)]
    [ProducesResponseType(typeof(string), 400)]
    public async Task<IActionResult> CreateBidderAsync([FromBody] CreateBidderRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Bidder name cannot be empty.");
        }

        var bidder = new Bidder(Guid.NewGuid(), request.Name);
        var result = await _bidderService.CreateBidderAsync(bidder.Id, bidder.Name);
        if (result.IsFailed)
        {
            return BadRequest(result.Errors.FirstOrDefault()?.Message);
        }

        return Ok(bidder);
    }
}