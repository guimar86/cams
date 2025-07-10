using System.Collections.Generic;
using System.Threading.Tasks;
using cams.application.Factory;
using cams.application.services;
using cams.contracts.models;
using cams.contracts.Requests.Vehicles;
using cams.contracts.Responses.Vehicles;
using Microsoft.AspNetCore.Mvc;

namespace cams.api.Controllers
{
    /// <summary>
    /// Controller for managing vehicle-related operations.
    /// </summary>
    [Produces("application/json")]
    [Consumes("application/json")]
    [Route("vehicles")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleController"/> class.
        /// </summary>
        /// <param name="vehicleService">The vehicle service dependency.</param>
        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        /// <summary>
        /// Retrieves all vehicles.
        /// </summary>
        /// <returns>A list of all vehicles or 404 if none found.</returns>
        [HttpGet]
        [Route("", Name = "GetAllVehicles")]
        [ProducesResponseType(typeof(GetAllVehiclesResponse), 200)]
        [ProducesResponseType(typeof(string), 404)]
        public async Task<IActionResult> GetAllVehicles()
        {
            var vehicles = await _vehicleService.GetAllVehicles();

            return Ok(vehicles);
        }

        /// <summary>
        /// Adds a new vehicle.
        /// </summary>
        /// <param name="request">The vehicle data to add.</param>
        /// <returns>The created vehicle response or 400 if failed.</returns>
        [HttpPost]
        [Route("", Name = "AddVehicle")]
        [ProducesResponseType(typeof(CreateVehicleResponse), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> AddVehicle([FromBody] AddVehicleRequest request)
        {
            Vehicle vehicle = VehicleFactory.CreateVehicle(request);

            var result = await _vehicleService.AddVehicleAsync(vehicle);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            //map domain model Vehicle to response model
            var response = new CreateVehicleResponse(vehicle.Reference);

            return Ok(response);
        }

        /// <summary>
        /// Searches for vehicles by model, manufacturer, or year.
        /// </summary>
        /// <param name="model">The vehicle model to search for.</param>
        /// <param name="manufacturer">The vehicle manufacturer to search for.</param>
        /// <param name="year">The vehicle year to search for.</param>
        /// <returns>A list of vehicles matching the search criteria or 400 if no parameters provided.</returns>
        [HttpGet]
        [Route("search", Name = "SearchVehicles")]
        [ProducesResponseType(typeof(IEnumerable<Vehicle>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public IActionResult SearchVehicles([FromQuery] SearchVehicleRequest request)
        {;
            var vehicles = _vehicleService.Search(request);
            if (vehicles.IsFailed)
            {
                return BadRequest(vehicles.Errors);
            }
            return Ok(vehicles.Value);
        }
    }
}