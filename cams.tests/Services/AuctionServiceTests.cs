using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel;
using cams.application.services;
using cams.contracts.models;
using cams.contracts.Repositories;
using cams.contracts.Requests.Auctions;
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
                    typeof(Sedan)));

            _service = new AuctionService(_vehicleRepository, _auctionRepository, _bidderRepository);
        }

        [Fact]
        public async Task CreateAuctionAsync_ShouldFail_WhenNoBidders()
        {
            var vehicle = _fixture.Create<Vehicle>();

            var result = await _service.CreateAuctionAsync(new contracts.Requests.Auctions.CreateAuctionRequest("VIN1234567890", new List<Guid>()));

            result.IsFailed.Should().BeTrue();
            result.Errors[0].Message.Should().Contain("At least one bidder");
        }

        [Fact]
        public async Task CreateAuctionAsync_ShouldFail_WhenBidderInvalid()
        {

            var request = new contracts.Requests.Auctions.CreateAuctionRequest("VIN1234567890", new List<Guid> { Guid.Empty });
            var result = await _service.CreateAuctionAsync(request);

            result.IsFailed.Should().BeTrue();
            result.Errors[0].Message.Should().Contain("valid ID and name");
        }

        [Fact]
        public async Task CreateAuctionAsync_ShouldFail_WhenBidderDoesNotExist()
        {
            var bidder = _fixture.Create<Bidder>();
            _bidderRepository.GetBidderByIdAsync(bidder.Id).Returns((Bidder)null);

            var result = await _service.CreateAuctionAsync(_fixture.Create<contracts.Requests.Auctions.CreateAuctionRequest>());

            result.IsFailed.Should().BeTrue();
            result.Errors[0].Message.Should().Contain("does not exist");
        }

        [Fact]
        public async Task CreateAuctionAsync_ShouldFail_WhenVehicleDoesNotExist()
        {
            var bidder = SetupValidBidder();
            _vehicleRepository.GetVehicleByVinAsync(Arg.Any<string>()).Returns((Vehicle)null);

            var result = await _service.CreateAuctionAsync(_fixture.Create<contracts.Requests.Auctions.CreateAuctionRequest>());

            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public async Task CreateAuctionAsync_ShouldFail_WhenVehicleInActiveAuction()
        {
            var bidder = SetupValidBidder();
            var vehicle = _fixture.Create<Vehicle>();
            _vehicleRepository.GetVehicleByVinAsync(vehicle.Reference).Returns(vehicle);
            _auctionRepository.ExistingAuctionByVehicle(vehicle.Reference).Returns(true);

            var result = await _service.CreateAuctionAsync(_fixture.Create<contracts.Requests.Auctions.CreateAuctionRequest>());

            result.IsFailed.Should().BeTrue();
        }

        [Fact]
        public async Task CreateAuctionAsync_ShouldSucceed_WhenValid()
        {
            CreateAuctionRequest request=new CreateAuctionRequest
            (
                "VIN1234567890",
                [Guid.Parse("f7d8b9fb-22ec-4ad6-a272-0540865c7b8c")]
            );
            var bidder = SetupValidBidder();
            var vehicle = _fixture.Create<Vehicle>();
            var auction = _fixture.Build<Auction>()
                                  .With(a => a.Vehicle, vehicle)
                                  .With(a => a.Bidders, new List<Bidder> { bidder })
                                  .Create();
            _bidderRepository.GetBidderByIdAsync(Arg.Any<Guid>()).Returns(bidder);
            _vehicleRepository.GetVehicleByVinAsync(Arg.Any<string>()).Returns(vehicle);
            _auctionRepository.ExistingAuctionByVehicle(Arg.Any<string>()).Returns(false);
            _auctionRepository.CreateAuctionAsync(Arg.Any<Guid>(), vehicle, Arg.Any<List<Bidder>>())
                              .Returns(auction);
            
            var result = await _service.CreateAuctionAsync(request);

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
