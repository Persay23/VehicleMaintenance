// Controllers/VehicleComponentControllerTests.cs
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VehicleMaintenance.Controllers;
using VehicleMaintenance.DTOs.VehicleComponents;
using VehicleMaintenance.Services.Interfaces;

namespace VehicleMaintenance.Tests.Controllers
{
    public class VehicleComponentControllerTests
    {
        private readonly Mock<IVehicleComponentService> _serviceMock;
        private readonly VehicleComponentController _controller;

        public VehicleComponentControllerTests()
        {
            _serviceMock = new Mock<IVehicleComponentService>();

            // This will work once you fix the controller to inject IVehicleComponentService
            _controller = new VehicleComponentController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetVehicleComponents_ReturnsOkWithList()
        {
            var fakeComponents = new List<VehicleComponentDto>
            {
                new() { VehicleComponentId = 1, ComponentType = "Brakes" },
                new() { VehicleComponentId = 2, ComponentType = "Engine" }
            };

            _serviceMock
                .Setup(s => s.GetAllVehicleComponentsAsync())
                .ReturnsAsync(fakeComponents);

            var result = await _controller.GetVehicleComponents();

            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returned = okResult.Value.Should().BeAssignableTo<List<VehicleComponentDto>>().Subject;
            returned.Should().HaveCount(2);
        }

        [Fact]
        public async Task GetVehicleComponentById_WhenExists_ReturnsOk()
        {
            var fakeComponent = new VehicleComponentDto { VehicleComponentId = 1, ComponentType = "Tires" };

            _serviceMock
                .Setup(s => s.GetVehicleComponentByIdAsync(1))
                .ReturnsAsync(fakeComponent);

            var result = await _controller.GetVehicleComponentById(1);

            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var returned = okResult.Value.Should().BeAssignableTo<VehicleComponentDto>().Subject;
            returned.VehicleComponentId.Should().Be(1);
        }

        [Fact]
        public async Task GetVehicleComponentById_WhenNotExists_ReturnsNotFound()
        {
            _serviceMock
                .Setup(s => s.GetVehicleComponentByIdAsync(999))
                .ReturnsAsync((VehicleComponentDto?)null);

            var result = await _controller.GetVehicleComponentById(999);

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task DeleteVehicleComponent_WhenExists_ReturnsNoContent()
        {
            _serviceMock
                .Setup(s => s.DeleteVehicleComponentByIdAsync(1))
                .ReturnsAsync(true);

            var result = await _controller.DeleteVehicleComponent(1);

            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteVehicleComponent_WhenNotExists_ReturnsNotFound()
        {
            _serviceMock
                .Setup(s => s.DeleteVehicleComponentByIdAsync(999))
                .ReturnsAsync(false);

            var result = await _controller.DeleteVehicleComponent(999);

            result.Should().BeOfType<NotFoundResult>();
        }
    }
}