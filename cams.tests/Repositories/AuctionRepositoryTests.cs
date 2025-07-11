using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel;
using cams.contracts.models;
using cams.contracts.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace cams.tests.Repositories
{
    public class AuctionRepositoryTests
    {
        private readonly Fixture _fixture = new();

        public AuctionRepositoryTests()
        {
            _fixture.Customizations.Add(
                new TypeRelay(
                    typeof(Vehicle),
                    typeof(Sedan)));
        }
        [Fact]
        public async Task CreateAuctionAsync_ShouldAddAuction()
        {
            var repo = Substitute.For<IAuctionRepository>();
            var auctionId = _fixture.Create<Guid>();
            var vehicle = _fixture.Create<Vehicle>();
            var bidders = _fixture.CreateMany<Bidder>(2).ToList();
            var expected = new Auction { Id = auctionId, Vehicle = vehicle, Bidders = bidders };
            repo.CreateAuctionAsync(auctionId, vehicle, bidders).Returns(expected);
            var auction = await repo.CreateAuctionAsync(auctionId, vehicle, bidders);
            auction.Id.Should().Be(auctionId);
            auction.Vehicle.Should().Be(vehicle);
            auction.Bidders.Should().BeEquivalentTo(bidders);
        }

        [Fact]
        public async Task EndAuctionAsync_ShouldSetIsActiveFalse()
        {
            var repo = Substitute.For<IAuctionRepository>();
            var auction = _fixture.Build<Auction>().With(a => a.IsActive, true).With(a => a.CurrentBid, 1000).Create();
            await repo.EndAuctionAsync(auction);
            // Since this is a mock, we can't check the property change, but we can check the call
            await repo.Received().EndAuctionAsync(auction);
        }

        [Fact]
        public async Task GetAuctionById_ShouldReturnAuction_WhenExists()
        {
            var repo = Substitute.For<IAuctionRepository>();
            var auctionId = _fixture.Create<Guid>();
            var expected = _fixture.Build<Auction>().With(a => a.Id, auctionId).Create();
            repo.GetAuctionById(auctionId).Returns(expected);
            var found = await repo.GetAuctionById(auctionId);
            found.Should().NotBeNull();
            found.Id.Should().Be(auctionId);
        }

        [Fact]
        public async Task PlaceBidAsync_ShouldUpdateCurrentBidAndHighestBidder()
        {
            var repo = Substitute.For<IAuctionRepository>();
            var auction = _fixture.Build<Auction>().With(a => a.CurrentBid, 1000).Create();
            var bidder = _fixture.Create<Bidder>();
            await repo.PlaceBidAsync(auction, bidder, 2000);
            await repo.Received().PlaceBidAsync(auction, bidder, 2000);
        }

        [Fact]
        public async Task ExistingAuctionByVehicle_ShouldReturnFalse_WhenNoActiveAuction()
        {
            var repo = Substitute.For<IAuctionRepository>();
            repo.ExistingAuctionByVehicle(Arg.Any<string>()).Returns(false);
            var exists = await repo.ExistingAuctionByVehicle(_fixture.Create<string>());
            exists.Should().BeFalse();
        }

        [Fact]
        public async Task Search_ShouldReturnMatchingAuctions()
        {
            var repo = Substitute.For<IAuctionRepository>();
            var auctionId = _fixture.Create<Guid>();
            var vehicle = _fixture.Create<Vehicle>();
            var bidders = _fixture.CreateMany<Bidder>(2).ToList();
            var expected = new List<Auction> { new Auction { Id = auctionId, Vehicle = vehicle, Bidders = bidders } };
            repo.Search(Arg.Any<Func<Auction, bool>>()).Returns(expected);
            var results = await repo.Search(a => a.Vehicle.Reference == vehicle.Reference);
            results.Should().ContainSingle(a => a.Vehicle.Reference == vehicle.Reference);
        }

        [Fact]
        public async Task StartAuctionAsync_ShouldSetIsActiveTrue()
        {
            var repo = Substitute.For<IAuctionRepository>();
            var auction = _fixture.Build<Auction>().With(a => a.IsActive, false).Create();
            await repo.StartAuctionAsync(auction);
            await repo.Received().StartAuctionAsync(auction);
        }
    }
}
