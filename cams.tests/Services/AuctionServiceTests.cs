using System;
using System.Collections.Generic;
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
    public class AuctionServiceTests
    {
        private readonly IVehicleRepository _vehicleRepository = Substitute.For<IVehicleRepository>();
        private readonly IAuctionRepository _auctionRepository = Substitute.For<IAuctionRepository>();
        private readonly IBidderRepository _bidderRepository = Substitute.For<IBidderRepository>();
        private readonly AuctionService _service;
        private readonly Fixture _fixture;

        public AuctionServiceTests()
        {
            _fixture = new Fixture();
            _fixture.Customizations.Add(
                new TypeRelay(
                    typeof(Vehicle),
                    typeof(Sedans)));

            _service = new AuctionService(_vehicleRepository, _auctionRepository, _bidderRepository);
        }

        [Fact]
        public async Task CreateAuctionAsync_ShouldFail_WhenNoBidders()
        {
            var vehicle = _fixture.Create<Vehicle>();

            var result = await _service.CreateAuctionAsync(Guid.NewGuid(), vehicle, new List<Bidder>());

            result.IsFailed.Should().BeTrue();
            result.Errors[0].Message.Should().Contain("At least one bidder");
        }

        [Fact]
        public async Task CreateAuctionAsync_ShouldFail_WhenBidderInvalid()
        {
            var bidders = new List<Bidder> { new(Guid.Empty, "") };

            var result = await _service.CreateAuctionAsync(Guid.NewGuid(), _fixture.Create<Vehicle>(), bidders);

            result.IsFailed.Should().BeTrue();
            result.Errors[0].Message.Should().Contain("valid ID and name");
        }

        [Fact]
        public async Task CreateAuctionAsync_ShouldFail_WhenBidderDoesNotExist()
        {
            var bidder = _fixture.Create<Bidder>();
            _bidderRepository.GetBidderByIdAsync(bidder.Id).Returns((Bidder)null);

            var result = await _service.CreateAuctionAsync(Guid.NewGuid(), _fixture.Create<Vehicle>(), new List<Bidder> { bidder });

            result.IsFailed.Should().BeTrue();
            result.Errors[0].Message.Should().Contain("does not exist");
        }

        [Fact]
        public async Task CreateAuctionAsync_ShouldFail_WhenVehicleDoesNotExist()
        {
            var bidder = SetupValidBidder();
            _vehicleRepository.GetVehicleByVinAsync(Arg.Any<string>()).Returns((Vehicle)null);

            var result = await _service.CreateAuctionAsync(Guid.NewGuid(), _fixture.Create<Vehicle>(), new List<Bidder> { bidder });

            result.IsFailed.Should().BeTrue();
            result.Errors[0].Message.Should().Contain("Vehicle must exist");
        }

        [Fact]
        public async Task CreateAuctionAsync_ShouldFail_WhenVehicleInActiveAuction()
        {
            var bidder = SetupValidBidder();
            var vehicle = _fixture.Create<Vehicle>();
            _vehicleRepository.GetVehicleByVinAsync(vehicle.Reference).Returns(vehicle);
            _auctionRepository.ExistingAuctionByVehicle(vehicle.Reference).Returns(true);

            var result = await _service.CreateAuctionAsync(Guid.NewGuid(), vehicle, new List<Bidder> { bidder });

            result.IsFailed.Should().BeTrue();
            result.Errors[0].Message.Should().Contain("already in an active auction");
        }

        [Fact]
        public async Task CreateAuctionAsync_ShouldSucceed_WhenValid()
        {
            var bidder = SetupValidBidder();
            var vehicle = _fixture.Create<Vehicle>();
            var auction = _fixture.Build<Auction>()
                                  .With(a => a.Vehicle, vehicle)
                                  .With(a => a.Bidders, new List<Bidder> { bidder })
                                  .Create();

            _vehicleRepository.GetVehicleByVinAsync(vehicle.Reference).Returns(vehicle);
            _auctionRepository.ExistingAuctionByVehicle(vehicle.Reference).Returns(false);
            _auctionRepository.CreateAuctionAsync(Arg.Any<Guid>(), vehicle, Arg.Any<List<Bidder>>())
                              .Returns(auction);

            var result = await _service.CreateAuctionAsync(Guid.NewGuid(), vehicle, new List<Bidder> { bidder });

            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(auction);
        }

        private Bidder SetupValidBidder()
        {
            var bidder = _fixture.Create<Bidder>();
            _bidderRepository.GetBidderByIdAsync(bidder.Id).Returns(bidder);
            return bidder;
        }
    }
}
