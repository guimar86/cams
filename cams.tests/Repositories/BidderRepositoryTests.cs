using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using cams.application.models;
using cams.application.repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace cams.tests.Repositories
{
    public class BidderRepositoryTests
    {
        private readonly Fixture _fixture = new();

        [Fact]
        public async Task CreateBidderAsync_ShouldAddAndReturnBidder()
        {
            var repo = Substitute.For<IBidderRepository>();
            var id = _fixture.Create<Guid>();
            var name = _fixture.Create<string>();
            var expected = new Bidder(id, name);
            repo.CreateBidderAsync(id, name).Returns(expected);
            var bidder = await repo.CreateBidderAsync(id, name);
            bidder.Id.Should().Be(id);
            bidder.Name.Should().Be(name);
            bidder.Should().NotBeNull();
        }

        [Fact]
        public async Task GetBidderByIdAsync_ShouldReturnBidder_WhenExists()
        {
            var repo = Substitute.For<IBidderRepository>();
            var id = _fixture.Create<Guid>();
            var expected = new Bidder(id, _fixture.Create<string>());
            repo.GetBidderByIdAsync(id).Returns(expected);
            var bidder = await repo.GetBidderByIdAsync(id);
            bidder.Should().NotBeNull();
            bidder.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetBidderByIdAsync_ShouldReturnNull_WhenNotExists()
        {
            var repo = Substitute.For<IBidderRepository>();
            repo.GetBidderByIdAsync(Arg.Any<Guid>()).Returns((Bidder)null);
            var bidder = await repo.GetBidderByIdAsync(_fixture.Create<Guid>());
            bidder.Should().BeNull();
        }

        [Fact]
        public async Task GetAllBiddersAsync_ShouldReturnAllBidders()
        {
            var repo = Substitute.For<IBidderRepository>();
            var bidders = _fixture.CreateMany<Bidder>(3).ToList();
            repo.GetAllBiddersAsync().Returns(bidders);
            var result = await repo.GetAllBiddersAsync();
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(bidders);
        }
    }
}
