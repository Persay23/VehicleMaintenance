// Services/VehicleComponentServiceTests.cs
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore.Internal;
using VehicleMaintenance.DTOs.VehicleComponents;
using VehicleMaintenance.Mappings;
using VehicleMaintenance.Models.Entities;
using VehicleMaintenance.Models.Enums;
using VehicleMaintenance.Services;
using VehicleMaintenance.Tests.Helpers;

namespace VehicleMaintenance.Tests.Services
{
    public class VehicleComponentServiceTests
    {
        private readonly IMapper _mapper;

        public VehicleComponentServiceTests()
        {
            var config = new AutoMapper.MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            // Ensure mappings are valid for the tests
            config.AssertConfigurationIsValid();
            _mapper = config.CreateMapper();
        }

        // Each test creates its own isolated DB
        private VehicleComponentService CreateService(string dbName)
        {
            var context = DbContextFactory.Create(dbName);
            return new VehicleComponentService(context, _mapper);
        }

        [Fact]
        public async Task CreateVehicleComponentAsync_ReturnsCreatedDto()
        {
            var service = CreateService(nameof(CreateVehicleComponentAsync_ReturnsCreatedDto));

            var dto = new CreateVehicleComponentDto
            {
                VehicleId = 1,
                ComponentType = "Brakes",
                InstallationDate = DateTime.UtcNow,
                CurrentMileage = 10000,
                ExpectedLifetimeKm = 60000,
                ExpectedLifetimeYears = 3
            };

            var result = await service.CreateVehicleComponentAsync(dto);

            result.Should().NotBeNull();
            result.VehicleId.Should().Be(1);
            result.ComponentType.Should().Be("Brakes");
            result.CurrentMileage.Should().Be(10000);
        }

        [Fact]
        public async Task GetAllVehicleComponentsAsync_ReturnsAllComponents()
        {
            var dbName = nameof(GetAllVehicleComponentsAsync_ReturnsAllComponents);
            var context = DbContextFactory.Create(dbName);

            // Seed two components directly
            context.VehicleComponents.AddRange(
                new VehicleComponent { VehicleId = 1, ComponentType = ComponentType.Brakes, InstallationDate = DateTime.UtcNow, State = State.Good },
                new VehicleComponent { VehicleId = 1, ComponentType = ComponentType.Engine, InstallationDate = DateTime.UtcNow, State = State.Normal }
            );
            await context.SaveChangesAsync();

            var service = new VehicleComponentService(context, _mapper);
            var result = await service.GetAllVehicleComponentsAsync();

            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetVehicleComponentByIdAsync_WhenExists_ReturnsDto()
        {
            var dbName = nameof(GetVehicleComponentByIdAsync_WhenExists_ReturnsDto);
            var context = DbContextFactory.Create(dbName);

            var component = new VehicleComponent
            {
                VehicleId = 1,
                ComponentType = ComponentType.Tyres,
                InstallationDate = DateTime.UtcNow,
                State = State.Good
            };
            context.VehicleComponents.Add(component);
            await context.SaveChangesAsync();

            var service = new VehicleComponentService(context, _mapper);
            var result = await service.GetVehicleComponentByIdAsync(component.VehicleComponentId);

            result.Should().NotBeNull();
            result!.ComponentType.Should().Be("Tyres");
        }

        [Fact]
        public async Task GetVehicleComponentByIdAsync_WhenNotExists_ReturnsNull()
        {
            var service = CreateService(nameof(GetVehicleComponentByIdAsync_WhenNotExists_ReturnsNull));

            var result = await service.GetVehicleComponentByIdAsync(999);

            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteVehicleComponentByIdAsync_WhenExists_ReturnsTrue()
        {
            var dbName = nameof(DeleteVehicleComponentByIdAsync_WhenExists_ReturnsTrue);
            var context = DbContextFactory.Create(dbName);

            var component = new VehicleComponent
            {
                VehicleId = 1,
                ComponentType = ComponentType.Brakes,
                InstallationDate = DateTime.UtcNow,
                State = State.Good
            };
            context.VehicleComponents.Add(component);
            await context.SaveChangesAsync();

            var service = new VehicleComponentService(context, _mapper);
            var deleted = await service.DeleteVehicleComponentByIdAsync(component.VehicleComponentId);

            deleted.Should().BeTrue();

            var remaining = await service.GetVehicleComponentByIdAsync(component.VehicleComponentId);
            remaining.Should().BeNull();
        }

        [Fact]
        public async Task DeleteVehicleComponentByIdAsync_WhenNotExists_ReturnsFalse()
        {
            var service = CreateService(nameof(DeleteVehicleComponentByIdAsync_WhenNotExists_ReturnsFalse));

            var result = await service.DeleteVehicleComponentByIdAsync(999);

            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateVehicleComponentByIdAsync_WhenExists_UpdatesFields()
        {
            var dbName = nameof(UpdateVehicleComponentByIdAsync_WhenExists_UpdatesFields);
            var context = DbContextFactory.Create(dbName);

            var component = new VehicleComponent
            {
                VehicleId = 1,
                ComponentType = ComponentType.Brakes,
                InstallationDate = DateTime.UtcNow,
                State = State.Good,
                CurrentMileage = 10000
            };
            context.VehicleComponents.Add(component);
            await context.SaveChangesAsync();

            var service = new VehicleComponentService(context, _mapper);

            var updateDto = new UpdateVehicleComponentDto
            {
                CurrentMileage = 25000,
                State = "NeedsService",
                Notes = "Updated during test"
            };

            var result = await service.UpdateVehicleComponentByIdAsync(component.VehicleComponentId, updateDto);

            result.Should().NotBeNull();
            result!.CurrentMileage.Should().Be(25000);
            result.Notes.Should().Be("Updated during test");
        }

        [Fact]
        public async Task UpdateVehicleComponentByIdAsync_WhenNotExists_ReturnsNull()
        {
            var service = CreateService(nameof(UpdateVehicleComponentByIdAsync_WhenNotExists_ReturnsNull));

            var result = await service.UpdateVehicleComponentByIdAsync(999, new UpdateVehicleComponentDto());

            result.Should().BeNull();
        }
    }
}