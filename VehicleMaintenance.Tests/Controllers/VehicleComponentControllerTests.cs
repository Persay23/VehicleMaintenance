// Controllers/VehicleComponentControllerTests.cs
using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VehicleMaintenance.Controllers;
using VehicleMaintenance.DTOs.VehicleComponents;
using VehicleMaintenance.Services.Interfaces;
using VehicleMaintenance.Services.Security;

namespace VehicleMaintenance.Tests.Controllers
{
    public class VehicleComponentControllerTests
    {
        private const string TestUserId = "user-1";
        private readonly Mock<IVehicleComponentService> _serviceMock;
        private readonly Mock<IVehicleOwnershipService> _ownershipMock;
        private readonly VehicleComponentController _controller;

        public VehicleComponentControllerTests()
        {
            _serviceMock = new Mock<IVehicleComponentService>();
            _ownershipMock = new Mock<IVehicleOwnershipService>();

            // Default: the test user owns whatever component is requested, so actions reach the service.
            _ownershipMock
                .Setup(o => o.OwnsComponentAsync(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            _controller = new VehicleComponentController(_serviceMock.Object, _ownershipMock.Object);

            // Give the controller an authenticated user so User.GetUserId() resolves.
            var principal = new ClaimsPrincipal(new ClaimsIdentity(
                new[] { new Claim(ClaimTypes.NameIdentifier, TestUserId) }, "TestAuth"));
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = principal }
            };
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
        public async Task GetVehicleComponentById_WhenNotOwned_ReturnsNotFound()
        {
            _ownershipMock
                .Setup(o => o.OwnsComponentAsync(TestUserId, 5))
                .ReturnsAsync(false);

            var result = await _controller.GetVehicleComponentById(5);

            result.Result.Should().BeOfType<NotFoundResult>();
            _serviceMock.Verify(s => s.GetVehicleComponentByIdAsync(It.IsAny<int>()), Times.Never);
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
