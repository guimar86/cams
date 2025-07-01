using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using cams.application.models;
using cams.application.services;
using cams.contracts.Requests.Vehicles;
using cams.contracts.Responses.Vehicles;
using cams.contracts.shared;
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
            var enumerable = vehicles.ToList();
            if (enumerable.Count == 0)
            {
                return NotFound("No vehicles found.");
            }


            var response = enumerable.ConvertAll(p =>
            {
                KeyValuePair<string, object> attribute = p.VehicleAttributes switch
                {
                    SedanAttributes sedan => new KeyValuePair<string, object>("NumberOfDoors", sedan.NumberOfDoors),
                    SuvAttributes suv => new KeyValuePair<string, object>("NumberOfSeats",
                        suv.NumberOfSeats),
                    HatchbackAttributes hatchback => new KeyValuePair<string, object>("NumberOfDoors",
                        hatchback.NumberOfDoors),
                    TruckAttributes truck => new KeyValuePair<string, object>("LoadCapacity",
                        truck.LoadCapacity),
                    _ => new KeyValuePair<string, object>("", null)
                };

                return new GetAllVehiclesResponse(
                    p.Vin,
                    p.VehicleType.ToString(),
                    p.VehicleAttributes.Manufacturer,
                    p.VehicleAttributes.Model,
                    p.VehicleAttributes.Year,
                    attribute
                );
            });

            return Ok(response);
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
            Vehicle vehicle = new Vehicle(request.Vin, request.VehicleType, request.Manufacturer, request.Model,
                request.Year);
            switch (request.VehicleType)
            {
                case VehicleType.Hatchback:
                    ((HatchbackAttributes)vehicle.VehicleAttributes).NumberOfDoors = request.NumberOfDoors ?? 5;
                    break;
                case VehicleType.Sedan:
                    ((SedanAttributes)vehicle.VehicleAttributes).NumberOfDoors = request.NumberOfDoors ?? 4;
                    break;
                case VehicleType.Suv:
                    ((SuvAttributes)vehicle.VehicleAttributes).NumberOfSeats = request.NumberOfDoors ?? 5;
                    break;
                case VehicleType.Truck:
                    ((TruckAttributes)vehicle.VehicleAttributes).LoadCapacity = request.NumberOfDoors ?? 1000;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(request.VehicleType), "Unsupported vehicle type.");
            }

            var result = await _vehicleService.AddVehicleAsync(vehicle);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            //map domain model Vehicle to response model
            var response = new CreateVehicleResponse(vehicle.Vin);

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
        public Task<IActionResult> SearchVehicles([FromQuery] string model, [FromQuery] string manufacturer,
            [FromQuery] int? year)
        {
            if (string.IsNullOrWhiteSpace(model) && string.IsNullOrWhiteSpace(manufacturer) && !year.HasValue)
            {
                return Task.FromResult<IActionResult>(BadRequest("At least one search parameter must be provided."));
            }


            var vehicles = _vehicleService.Search(v =>
                (string.IsNullOrWhiteSpace(model) ||
                 v.VehicleAttributes.Model.Contains(model, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(manufacturer) ||
                 v.VehicleAttributes.Manufacturer.Contains(manufacturer, StringComparison.OrdinalIgnoreCase)) &&
                (!year.HasValue || v.VehicleAttributes.Year == year.Value));

            return Task.FromResult<IActionResult>(Ok(vehicles));
        }
    }
}