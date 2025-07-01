using System;
using System.Linq;
using System.Threading.Tasks;
using cams.api.Mappers;
using cams.application.models;
using cams.application.services;
using cams.contracts.Requests.Vehicles;
using cams.contracts.Responses.Vehicles;
using Microsoft.AspNetCore.Mvc;

namespace cams.api.Controllers
{
    [Route("vehicles")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }
        
        [HttpGet]
        [Route("", Name = "GetAllVehicles")]
        public async Task<IActionResult> GetAllVehicles()
        {
            var vehicles = await _vehicleService.GetAllVehicles();
            var enumerable = vehicles.ToList();
            if (enumerable.Count == 0)
            {
                return NotFound("No vehicles found.");
            }

            var response = enumerable.ConvertAll(p => new GetAllVehiclesResponse(p.Vin, p.VehicleType.ToString(),
                p.VehicleAttributes.Manufacturer, p.VehicleAttributes.Model, p.VehicleAttributes.Year));
            
            return Ok(response);
        }

        [HttpPost]
        [Route("", Name = "AddVehicle")]
        public async Task<IActionResult> AddVehicle([FromBody] AddVehicleRequest request)
        {
            //map request to domain model Vehicle
            var vehicleType = EnumMapper.MapEnumByName<cams.contracts.shared.VehicleType, VehicleType>(request.VehicleType);
            Vehicle vehicle = new Vehicle(request.Vin, vehicleType, request.Manufacturer, request.Model, request.Year);
            var result = await _vehicleService.AddVehicleAsync(vehicle.Vin, vehicle.VehicleType, request.Manufacturer, request.Model, request.Year);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            //map domain model Vehicle to response model
            var response = new CreateVehicleResponse(vehicle.Vin);

            return Ok(response);
        }

        [HttpGet]
        [Route("search", Name = "SearchVehicles")]
        public Task<IActionResult> SearchVehicles([FromQuery] string model, [FromQuery] string manufacturer, [FromQuery] int? year)
        {
            if (string.IsNullOrWhiteSpace(model) && string.IsNullOrWhiteSpace(manufacturer) && !year.HasValue)
            {
                return Task.FromResult<IActionResult>(BadRequest("At least one search parameter must be provided."));
            }


            var vehicles = _vehicleService.Search(v =>
                (string.IsNullOrWhiteSpace(model) || v.VehicleAttributes.Model.Contains(model, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(manufacturer) || v.VehicleAttributes.Manufacturer.Contains(manufacturer, StringComparison.OrdinalIgnoreCase)) &&
                (!year.HasValue || v.VehicleAttributes.Year == year.Value));

            return Task.FromResult<IActionResult>(Ok(vehicles));

        }
    }
}
