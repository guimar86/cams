using System;
using System.Threading.Tasks;
using AutoFixture;
using cams.application.models;
using cams.application.repositories;
using cams.application.services;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace cams.tests.Services
{
    public class BidderServiceTests
    {
        private readonly IBidderRepository _bidderRepository = Substitute.For<IBidderRepository>();
        private readonly BidderService _service;
        private readonly Fixture _fixture = new();

        public BidderServiceTests()
        {
            _service = new BidderService(_bidderRepository);
        }

        [Fact]
        public async Task GetBidderByIdAsync_ShouldReturnFail_WhenIdIsEmpty()
        {
            var result = await _service.GetBidderByIdAsync(Guid.Empty);
            result.IsFailed.Should().BeTrue();
            result.Errors[0].Message.Should().Be("Bidder ID cannot be empty.");
        }

        [Fact]
        public async Task GetBidderByIdAsync_ShouldReturnFail_WhenBidderNotFound()
        {
            _bidderRepository.GetBidderByIdAsync(Arg.Any<Guid>()).Returns((Bidder)null);
            var result = await _service.GetBidderByIdAsync(_fixture.Create<Guid>());
            result.IsFailed.Should().BeTrue();
            result.Errors[0].Message.Should().Contain("does not exist");
        }

        [Fact]
        public async Task GetBidderByIdAsync_ShouldReturnOk_WhenBidderFound()
        {
            var bidder = _fixture.Create<Bidder>();
            _bidderRepository.GetBidderByIdAsync(bidder.Id).Returns(bidder);
            var result = await _service.GetBidderByIdAsync(bidder.Id);
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(bidder);
        }

        [Fact]
        public async Task CreateBidderAsync_ShouldReturnFail_WhenIdIsEmpty()
        {
            var result = await _service.CreateBidderAsync(Guid.Empty, "Test");
            result.IsFailed.Should().BeTrue();
            result.Errors[0].Message.Should().Be("Bidder ID cannot be empty.");
        }

        [Fact]
        public async Task CreateBidderAsync_ShouldReturnFail_WhenNameIsEmpty()
        {
            var result = await _service.CreateBidderAsync(_fixture.Create<Guid>(), "");
            result.IsFailed.Should().BeTrue();
            result.Errors[0].Message.Should().Be("Bidder name cannot be empty.");
        }
    }
}
