using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel;
using cams.application.services;
using cams.contracts.models;
using cams.contracts.Repositories;
using cams.contracts.Requests.Vehicles;
using cams.contracts.shared;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace cams.tests.Services
{
    public class VehicleServiceTests
    {
        private readonly IVehicleRepository _vehicleRepository = Substitute.For<IVehicleRepository>();
        private readonly VehicleService _service;
        private readonly Fixture _fixture = new();

        public VehicleServiceTests()
        {
            _fixture.Customizations.Add(
                new TypeRelay(
                    typeof(Vehicle),
                    typeof(Sedan)));
            _service = new VehicleService(_vehicleRepository);
        }

        [Fact]
        public async Task AddVehicleAsync_ShouldReturnFail_WhenVehicleExists()
        {
            var vehicle = _fixture.Create<Vehicle>();
            _vehicleRepository.GetVehicleByVinAsync(Arg.Any<string>()).Returns(vehicle);
            var result = await _service.AddVehicleAsync(_fixture.Create<AddVehicleRequest>());
            result.IsFailed.Should().BeTrue();
            result.Errors[0].Message.Should().Be("Vehicle already exists.");
        }

        [Fact]
        public async Task AddVehicleAsync_ShouldReturnOk_WhenVehicleDoesNotExist()
        {
            var vehicle = _fixture.Create<Sedan>();
            vehicle.Reference = "1HGCM82633A123456"; // Example VIN
            var request = new AddVehicleRequest
          (
             vehicle.Reference,
             VehicleType.Sedan,
             vehicle.Manufacturer,
              vehicle.Model,
               vehicle.Year,
               vehicle.NumberOfDoors, null,
               null
          );
            _vehicleRepository.AddVehicleAsync(vehicle).Returns(vehicle);
            _vehicleRepository.GetVehicleByVinAsync(vehicle.Reference).Returns((Vehicle)null);


            var result = await _service.AddVehicleAsync(request);
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task GetVehicleByVinAsync_ShouldReturnFail_WhenVinIsNullOrEmpty()
        {
            var result = await _service.GetVehicleByVinAsync("");
            result.IsFailed.Should().BeTrue();
            result.Errors[0].Message.Should().Be("VIN cannot be null or empty.");
        }

        [Fact]
        public async Task GetVehicleByVinAsync_ShouldReturnOk_WhenVehicleFound()
        {
            var vehicle = _fixture.Create<Vehicle>();
            _vehicleRepository.GetVehicleByVinAsync(vehicle.Reference).Returns(vehicle);
            var result = await _service.GetVehicleByVinAsync(vehicle.Reference);
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(vehicle);
        }

    }
}
