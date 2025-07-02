using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel;
using cams.application.services;
using cams.contracts.models;
using cams.contracts.Repositories;
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
                    typeof(BaseVehicleAttributes),
                    typeof(SedanAttributes)));
            _service = new VehicleService(_vehicleRepository);
        }

        [Fact]
        public async Task AddVehicleAsync_ShouldReturnFail_WhenVehicleExistsInActiveAuction()
        {
            var vehicle = _fixture.Create<Vehicle>();
            _vehicleRepository.ExistsInActiveAuction(vehicle.Vin).Returns(true);
            var result = await _service.AddVehicleAsync(vehicle);
            result.IsFailed.Should().BeTrue();
            result.Errors[0].Message.Should().Be("Vehicle already exists.");
        }

        [Fact]
        public async Task AddVehicleAsync_ShouldReturnOk_WhenVehicleDoesNotExist()
        {
            var vehicle = _fixture.Create<Vehicle>();
            _vehicleRepository.ExistsInActiveAuction(vehicle.Vin).Returns(false);
            _vehicleRepository.AddVehicleAsync(vehicle).Returns(vehicle);
            var result = await _service.AddVehicleAsync(vehicle);
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(vehicle);
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
            _vehicleRepository.GetVehicleByVinAsync(vehicle.Vin).Returns(vehicle);
            var result = await _service.GetVehicleByVinAsync(vehicle.Vin);
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(vehicle);
        }

        [Fact]
        public void Search_ShouldReturnVehiclesMatchingPredicate()
        {
            var vehicles = _fixture.CreateMany<Vehicle>(3).ToList();
            _vehicleRepository.Search(Arg.Any<Func<Vehicle, bool>>())
                .Returns(callInfo => vehicles.Where(callInfo.Arg<Func<Vehicle, bool>>()));
            var result = _service.Search(v => v.VehicleAttributes.Manufacturer == vehicles[0].VehicleAttributes.Manufacturer);
            result.Should().Contain(v => v.VehicleAttributes.Manufacturer == vehicles[0].VehicleAttributes.Manufacturer);
        }
    }
}
