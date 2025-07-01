using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel;
using cams.application.config;
using cams.application.models;
using cams.application.repositories;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Xunit;

namespace cams.tests.Repositories
{
    public class AuctionRepositoryTests
    {
        private readonly Fixture _fixture = new();

        private AuctionRepository CreateRepoWithDefaults()
        {
            var settings = new VehicleBidSettings
            {
                StartingBids = new Dictionary<string, decimal> { { "Sedan", 1000 }, { "Suv", 2000 } }
            };
            var options = Options.Create(settings);
            _fixture.Customizations.Add(
                new TypeRelay(
                    typeof(cams.application.models.BaseVehicleAttributes),
                    typeof(cams.application.models.SedanAttributes)));
            return new AuctionRepository(options);
        }

        [Fact]
        public async Task CreateAuctionAsync_ShouldAddAuction()
        {
            var repo = CreateRepoWithDefaults();
            var auctionId = _fixture.Create<Guid>();
            var vehicle = _fixture.Create<Vehicle>();
            var bidders = _fixture.CreateMany<Bidder>(2).ToList();
            var auction = await repo.CreateAuctionAsync(auctionId, vehicle, bidders);
            auction.Id.Should().Be(auctionId);
            auction.Vehicle.Should().Be(vehicle);
            auction.Bidders.Should().BeEquivalentTo(bidders);
        }

        [Fact]
        public async Task EndAuctionAsync_ShouldSetIsActiveFalse()
        {
            var repo = CreateRepoWithDefaults();
            var auction = _fixture.Build<Auction>().With(a => a.IsActive, true).With(a => a.CurrentBid, 1000).Create();
            await repo.EndAuctionAsync(auction);
            auction.IsActive.Should().BeFalse();
        }

        [Fact]
        public async Task GetAuctionById_ShouldReturnAuction_WhenExists()
        {
            var repo = CreateRepoWithDefaults();
            var auctionId = _fixture.Create<Guid>();
            var vehicle = _fixture.Create<Vehicle>();
            var bidders = _fixture.CreateMany<Bidder>(2).ToList();
            await repo.CreateAuctionAsync(auctionId, vehicle, bidders);
            var found = await repo.GetAuctionById(auctionId);
            found.Should().NotBeNull();
            found.Id.Should().Be(auctionId);
        }

        [Fact]
        public async Task PlaceBidAsync_ShouldUpdateCurrentBidAndHighestBidder()
        {
            var repo = CreateRepoWithDefaults();
            var auction = _fixture.Build<Auction>().With(a => a.CurrentBid, 1000).Create();
            var bidder = _fixture.Create<Bidder>();
            await repo.PlaceBidAsync(auction, bidder, 2000);
            auction.CurrentBid.Should().Be(2100);
            auction.HighestBidder.Should().Be(bidder);
        }

        [Fact]
        public async Task ExistingAuctionByVehicle_ShouldReturnFalse_WhenNoActiveAuction()
        {
            var repo = CreateRepoWithDefaults();
            var exists = await repo.ExistingAuctionByVehicle(_fixture.Create<string>());
            exists.Should().BeFalse();
        }

        [Fact]
        public async Task Search_ShouldReturnMatchingAuctions()
        {
            var repo = CreateRepoWithDefaults();
            var auctionId = _fixture.Create<Guid>();
            var vehicle = _fixture.Create<Vehicle>();
            var bidders = _fixture.CreateMany<Bidder>(2).ToList();
            await repo.CreateAuctionAsync(auctionId, vehicle, bidders);
            var results = await repo.Search(a => a.Vehicle.Vin == vehicle.Vin);
            results.Should().ContainSingle(a => a.Vehicle.Vin == vehicle.Vin);
        }

        [Fact]
        public async Task StartAuctionAsync_ShouldSetIsActiveTrue()
        {
            var repo = CreateRepoWithDefaults();
            var auction = _fixture.Build<Auction>().With(a => a.IsActive, false).Create();
            await repo.StartAuctionAsync(auction);
            auction.IsActive.Should().BeTrue();
        }
    }
}
