using AutoMapper;
using FluentAssertions;
using VehicleMaintenance.DTOs.VehicleComponents;
using VehicleMaintenance.Mappings;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Models.Enums;

namespace VehicleMaintenance.Tests.Mappings
{
    public class MappingProfileTests
    {
        private readonly IMapper _mapper;

        public MappingProfileTests()
        {
            var config = new AutoMapper.MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();
        }

        [Fact]
        public void MappingProfile_Configuration_IsValid()
        {
            // The constructor already calls AssertConfigurationIsValid()
            // If we reach here, all mappings are correctly configured
            Assert.True(true);
        }

        [Fact]
        public void VehicleComponent_To_VehicleComponentDto_MapsCorrectly()
        {
            var component = new VehicleComponent
            {
                // VehicleComponentId set via reflection since it's private set
                VehicleId = 1,
                ComponentType = ComponentType.Brakes,
                InstallationDate = new DateTime(2023, 1, 1),
                State = State.Good,
                CurrentMileage = 50000,
                ExpectedLifetimeKm = 100000,
                ExpectedLifetimeYears = 5,
                Notes = "Test notes"
            };

            var dto = _mapper.Map<VehicleComponentDto>(component);

            dto.VehicleId.Should().Be(1);
            dto.ComponentType.Should().Be("Brakes");
            dto.State.Should().Be("Good");
            dto.CurrentMileage.Should().Be(50000);
            dto.Notes.Should().Be("Test notes");
        }

        [Fact]
        public void CreateVehicleComponentDto_To_VehicleComponent_MapsCorrectly()
        {
            var dto = new CreateVehicleComponentDto
            {
                VehicleId = 2,
                ComponentType = "Brakes",
                InstallationDate = new DateTime(2023, 6, 1),
                CurrentMileage = 30000,
                ExpectedLifetimeKm = 80000,
                ExpectedLifetimeYears = 4
            };

            var entity = _mapper.Map<VehicleComponent>(dto);

            entity.VehicleId.Should().Be(2);
            entity.ComponentType.Should().Be(ComponentType.Brakes);
            entity.CurrentMileage.Should().Be(30000);
        }
    }
}
