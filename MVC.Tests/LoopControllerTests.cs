using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MVC.Controllers;
using MVC.Models;
using MVC.Repositories;
using MVC.ViewModels;
using static System.Formats.Asn1.AsnWriter;

namespace MVC.Tests;

public class LoopControllerTests
{
    private ILogger<LoopController> GetLogger()
    {
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        return loggerFactory.CreateLogger<LoopController>();
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

    private IStopRepository GetInMemoryStopRepository(BigishProjContext dbContext)
    {
        return new StopRepository(dbContext);
    }

    private IRouteRepository GetInMemoryRouteRepository(BigishProjContext dbContext)
    {
        return new RouteRepository(dbContext);
    }

    [Fact]
    public async Task TestIndexReturnsViewWithLoopIndexViewModel()
    {
        var dbContext = GetInMemoryDbContext();
        var loopRepository = GetInMemoryLoopRepository(dbContext);
        var stopRepository = GetInMemoryStopRepository(dbContext);
        var routeRepository = GetInMemoryRouteRepository(dbContext);
        var loopController = new LoopController(loopRepository, stopRepository, routeRepository, GetLogger());

        var result = await loopController.Index() as ViewResult;

        var viewModel = result.ViewData.Model as LoopIndexViewModel;
        Assert.NotNull(viewModel);
        Assert.NotNull(viewModel.CreateLoopViewModel);
        Assert.NotNull(viewModel.MapViewModel);
        Assert.NotNull(viewModel.Loops);
        Assert.NotNull(viewModel.LoopStops);
    }

    [Fact]
    public async Task TestCreateReturnsRedirectToActionIndexOnSuccess()
    {
        var dbContext = GetInMemoryDbContext();
        var loopRepository = GetInMemoryLoopRepository(dbContext);
        var stopRepository = GetInMemoryStopRepository(dbContext);
        var routeRepository = GetInMemoryRouteRepository(dbContext);
        var loopController = new LoopController(loopRepository, stopRepository, routeRepository, GetLogger());

        var stop1 = new Stop { Name = "Stop 1" };
        var stop2 = new Stop { Name = "Stop 2" };
        var stop3 = new Stop { Name = "Stop 3" };
        await stopRepository.AddStop(stop1);
        await stopRepository.AddStop(stop2);
        await stopRepository.AddStop(stop3);

        var createLoopViewModel = new CreateLoopViewModel
        {
            Loop = new Loop { Name = "Test Loop" },
            Routes = new List<RouteViewModel>
            {
                new RouteViewModel
                    { Order = 1, SelectedStopId = stop1.Id },
                new RouteViewModel
                    { Order = 2, SelectedStopId = stop2.Id },
                new RouteViewModel
                    { Order = 3, SelectedStopId = stop3.Id }
            }
        };

        var result = await loopController.Create(createLoopViewModel) as RedirectToActionResult;

        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        Assert.Single(loopRepository.GetLoops().Result);
    }

    [Fact]
    public async Task TestCreateReturnsRedirectToActionIndexOnFailure()
    {
        var dbContext = GetInMemoryDbContext();
        var loopRepository = GetInMemoryLoopRepository(dbContext);
        var stopRepository = GetInMemoryStopRepository(dbContext);
        var routeRepository = GetInMemoryRouteRepository(dbContext);
        var loopController = new LoopController(loopRepository, stopRepository, routeRepository, GetLogger());

        var createLoopViewModel = new CreateLoopViewModel();

        loopController.ModelState.AddModelError("Loop.Name", "The Name field is required.");
        loopController.ModelState.AddModelError("Routes", "At least one Route is required.");

        var result = await loopController.Create(createLoopViewModel) as RedirectToActionResult;

        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        Assert.Empty(loopRepository.GetLoops().Result);
    }

    [Fact]
    public async Task TestEditConfirmed()
    {
        var dbContext = GetInMemoryDbContext();
        var loopRepository = GetInMemoryLoopRepository(dbContext);
        var stopRepository = GetInMemoryStopRepository(dbContext);
        var routeRepository = GetInMemoryRouteRepository(dbContext);
        var loopController = new LoopController(loopRepository, stopRepository, routeRepository, GetLogger());
        var routeVM = new RouteViewModel()
        {
            SelectedStopId = 0,
            Order = 0
        };
        var stop = new Stop() { Name = "test", Latitude = 0.0, Longitude = 0.0 };
        Loop loop = new Loop
        {
            Name = "Test Loop",
            Routes = new List<Route>()
            {
                new Route(){ Order = 1 }
            }
        };
        var loopId = await loopRepository.AddLoop(loop);

        var loopVM = new CreateLoopViewModel() { Loop = loop, Routes = new List<RouteViewModel>() { routeVM }, Stops = new List<Stop>() { stop } };

        var result = await loopController.EditConfirmed(loopId, loopVM) as RedirectToActionResult;

        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        Assert.Single(loopRepository.GetLoops().Result);
    }

    [Fact]
    public async Task TestEditConfirmedInvalidState()
    {
        var dbContext = GetInMemoryDbContext();
        var loopRepository = GetInMemoryLoopRepository(dbContext);
        var stopRepository = GetInMemoryStopRepository(dbContext);
        var routeRepository = GetInMemoryRouteRepository(dbContext);
        var loopController = new LoopController(loopRepository, stopRepository, routeRepository, GetLogger());
        loopController.ModelState.AddModelError("Test", "Something malicious is brewing");
        var routeVM = new RouteViewModel()
        {
            SelectedStopId = 0,
            Order = 0
        };
        var stop = new Stop() { Name = "test", Latitude = 0.0, Longitude = 0.0 };
        Loop loop = new Loop
        {
            Name = "Test Loop",
            Routes = new List<Route>()
            {
                new Route(){ Order = 1 }
            }
        };
        var loopId = await loopRepository.AddLoop(loop);

        var loopVM = new CreateLoopViewModel() { Loop = loop, Routes = new List<RouteViewModel>() { routeVM }, Stops = new List<Stop>() { stop } };

        var result = await loopController.EditConfirmed(loopId, loopVM) as RedirectToActionResult;

        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        Assert.Single(loopRepository.GetLoops().Result);
    }

    [Fact]
    public async Task TestDelete()
    {
        var dbContext = GetInMemoryDbContext();
        var loopRepository = GetInMemoryLoopRepository(dbContext);
        var stopRepository = GetInMemoryStopRepository(dbContext);
        var routeRepository = GetInMemoryRouteRepository(dbContext);
        var loopController = new LoopController(loopRepository, stopRepository, routeRepository, GetLogger());
        var routeVM = new RouteViewModel()
        {
            SelectedStopId = 0,
            Order = 0
        };
        var stop = new Stop() { Name = "test", Latitude = 0.0, Longitude = 0.0 };
        Loop loop = new Loop
        {
            Name = "Test Loop",
            Routes = new List<Route>()
            {
                new Route(){ Order = 1 }
            }
        };
        var loopId = await loopRepository.AddLoop(loop);

        var loopVM = new CreateLoopViewModel() { Loop = loop, Routes = new List<RouteViewModel>() { routeVM }, Stops = new List<Stop>() { stop } };

        var result = await loopController.DeleteConfirmed(new int[] { loopId }) as RedirectToActionResult;

        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        Assert.Empty(loopRepository.GetLoops().Result);
    }
}