using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel;
using cams.contracts.models;
using cams.infrastructure.repositories;
using FluentAssertions;
using Xunit;

namespace cams.tests.Repositories
{
    public class VehicleRepositoryTests
    {
        private readonly Fixture _fixture = new();

        public VehicleRepositoryTests()
        {
            _fixture.Customizations.Add(
                new TypeRelay(
                    typeof(Vehicle),
                    typeof(Sedans)));
        }

        [Fact]
        public async Task AddVehicleAsync_ShouldAddAndReturnVehicle()
        {
            var repo = new VehicleRepository();
            var vehicle = _fixture.Create<Vehicle>();
            var result = await repo.AddVehicleAsync(vehicle);
            result.Should().Be(vehicle);
        }

        [Fact]
        public async Task GetVehicleByVinAsync_ShouldReturnVehicle_WhenExists()
        {
            var repo = new VehicleRepository();
            var result = await repo.GetVehicleByVinAsync("VIN1234567890");
            result.Should().NotBeNull();
            result.Reference.Should().Be("VIN1234567890");
        }

        [Fact]
        public async Task GetVehicleByVinAsync_ShouldReturnNull_WhenNotExists()
        {
            var repo = new VehicleRepository();
            var result = await repo.GetVehicleByVinAsync(_fixture.Create<string>());
            result.Should().BeNull();
        }

        [Fact]
        public void Search_ShouldReturnMatchingVehicles()
        {
            var repo = new VehicleRepository();
            var results = repo.Search(v => v.Manufacturer == "Toyota");
            results.Should().NotBeEmpty();
            results.All(v => v.Manufacturer == "Toyota").Should().BeTrue();
        }

        [Fact]
        public async Task ExistsInActiveAuction_ShouldReturnTrue_WhenExists()
        {
            var repo = new VehicleRepository();
            var result = await repo.ExistsInActiveAuction("VIN1234567890");
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsInActiveAuction_ShouldReturnFalse_WhenNotExists()
        {
            var repo = new VehicleRepository();
            var result = await repo.ExistsInActiveAuction(_fixture.Create<string>());
            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetAllVehicles_ShouldReturnAllVehicles()
        {
            var repo = new VehicleRepository();
            var result = await repo.GetAllVehicles();
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
        }
    }
}
