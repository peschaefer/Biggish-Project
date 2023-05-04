using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using MVC.Controllers;
using MVC.Models;
using MVC.Repositories;
using MVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Tests;

public class DriverControllerTests
{
    Driver driver = new Driver()
    {
        FirstName = "Test",
        LastName = "Test",
        IsManager = false,
        IsActive = true
    };

    Bus bus = new Bus()
    {
        BusNumber = 1
    };

    Route route = new Route()
    {
        Order = 1
    };

    Loop loop = new Loop()
    {
        Name = "Test"
    };

    Stop stop = new Stop()
    {
        Name = "Test",
        Latitude = 0.0,
        Longitude = 0.0
    };
    private ILogger<DriverController> GetLogger()
    {
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        return loggerFactory.CreateLogger<DriverController>();
    }

    private BigishProjContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<BigishProjContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb{Guid.NewGuid()}")
            .Options;

        return new BigishProjContext(options);
    }

    private ILoopRepository GetInMemoryLoopRepository(BigishProjContext dbContext)
    {
        return new LoopRepository(dbContext);
    }

    private IBusRepository GetInMemoryBusRepository(BigishProjContext dbContext)
    {
        return new BusRepository(dbContext);
    }

    private IRouteRepository GetInMemoryRouteRepository(BigishProjContext dbContext)
    {
        return new RouteRepository(dbContext);
    }

    private IDriverRepository GetInMemoryDriverRepository(BigishProjContext dbContext)
    {
        return new DriverRepository(dbContext);
    }

    [Fact]
    public async Task TestStartDriving()
    {
        var dbContext = GetInMemoryDbContext();
        var driverRepository = GetInMemoryDriverRepository(dbContext);
        var loopRepository = GetInMemoryLoopRepository(dbContext);
        var routeRepository = GetInMemoryRouteRepository(dbContext);
        var busRepository = GetInMemoryBusRepository(dbContext);
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        var context = new DefaultHttpContext();
        mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);
        var driverController = new DriverController(loopRepository, busRepository, mockHttpContextAccessor.Object, routeRepository, GetLogger(), new FakeUserManager(), new FakeSignInManager());
        var busId = await busRepository.AddBus(bus);
        var loopId = await loopRepository.AddLoop(loop);

        var actionResult = await driverController.StartDriving(busId, loopId) as RedirectToActionResult;

        Assert.NotNull(actionResult);
        Assert.Equal("EntryCreator", actionResult.ActionName);
    }

    [Fact]
    public async Task TestEntryCreator()
    {
        var dbContext = GetInMemoryDbContext();
        var driverRepository = GetInMemoryDriverRepository(dbContext);
        var loopRepository = GetInMemoryLoopRepository(dbContext);
        var routeRepository = GetInMemoryRouteRepository(dbContext);
        var busRepository = GetInMemoryBusRepository(dbContext);
        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        var context = new DefaultHttpContext();
        mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);
        var driverController = new DriverController(loopRepository, busRepository, mockHttpContextAccessor.Object, routeRepository, GetLogger(), new FakeUserManager(), new FakeSignInManager());
        var busId = await busRepository.AddBus(bus);
        var loopId = await loopRepository.AddLoop(loop);

        var actionResult = await driverController.EntryCreator(busId, loopId) as ViewResult;

        var model = actionResult.ViewData.Model as EntryCreatorViewModel;

        Assert.NotNull(actionResult);
        Assert.True(model.BusId == busId && model.LoopId == loopId);
    }
}
