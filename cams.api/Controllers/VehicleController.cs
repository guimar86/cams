using System;
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

        [HttpPost]
        [Route("", Name = "AddVehicle")]
        public async Task<IActionResult> AddVehicle([FromBody] AddVehicleRequest request)
        {
            //map request to domain model Vehicle
            var vehicleType = EnumMapper.MapEnumByName<cams.contracts.shared.VehicleType, VehicleType>(request.vehicleType);
            Vehicle vehicle = new Vehicle(request.vin, vehicleType, request.manufacturer, request.model, request.year);
            var result = await _vehicleService.AddVehicleAsync(vehicle.Vin, vehicle.VehicleType, request.manufacturer, request.model, request.year);

            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }

            //map domain model Vehicle to response model
            var response = new CreateVehicleResponse(vehicle.Vin);

            return Ok(response);
        }

        [HttpGet]
        [Route("", Name = "SearchVehicles")]
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
