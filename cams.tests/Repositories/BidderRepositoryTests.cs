using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using cams.application.models;
using cams.application.repositories;
using FluentAssertions;
using Xunit;

namespace cams.tests.Repositories
{
    public class BidderRepositoryTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task CreateBidderAsync_ShouldAddAndReturnBidder()
        {
            var repo = new BidderRepository();
            var id = _fixture.Create<Guid>();
            var name = _fixture.Create<string>();
            var bidder = await repo.CreateBidderAsync(id, name);
            bidder.Id.Should().Be(id);
            bidder.Name.Should().Be(name);
        }

        [Fact]
        public async Task GetBidderByIdAsync_ShouldReturnBidder_WhenExists()
        {
            var repo = new BidderRepository();
            var id = Guid.Parse("f7d8b9fb-22ec-4ad6-a272-0540865c7b8c");
            var bidder = await repo.GetBidderByIdAsync(id);
            bidder.Should().NotBeNull();
            bidder.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetBidderByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            var repo = new BidderRepository();
            var bidder = await repo.GetBidderByIdAsync(_fixture.Create<Guid>());
            bidder.Should().BeNull();
        }

        [Fact]
        public async Task GetAllBiddersAsync_ShouldReturnAllBidders()
        {
            var repo = new BidderRepository();
            var bidders = await repo.GetAllBiddersAsync();
            bidders.Should().NotBeNull();
            bidders.Should().NotBeEmpty();
        }
    }
}
