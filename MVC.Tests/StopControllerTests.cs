using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MVC.Controllers;
using MVC.Models;
using MVC.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Tests;

public class StopControllerTests
{
    private ILogger<StopController> GetLogger()
    {
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        return loggerFactory.CreateLogger<StopController>();
    }

    private IStopRepository GetInMemoryRepository()
    {
        var options = new DbContextOptionsBuilder<BigishProjContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb{Guid.NewGuid()}")
            .Options;

        var dbContext = new BigishProjContext(options);
        return new StopRepository(dbContext);
    }

    [Fact]
    public async Task TestIndex()
    {
        var stopRepository = GetInMemoryRepository();
        var stopController = new StopController(stopRepository, GetLogger());

        var actionResult = await stopController.Index() as ViewResult;

        var result = actionResult.ViewData.Model as List<Stop>;

        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public async Task TestCreateStop()
    {
        var stopRepository = GetInMemoryRepository();
        var stopController = new StopController(stopRepository, GetLogger());
        var stop = new Stop() { Name = "test", Latitude = 0.0, Longitude = 0.0 };

        var actionResult = await stopController.Create(stop) as RedirectToActionResult;

        Assert.NotNull(actionResult);
        Assert.Equal("Index", actionResult.ActionName);
        Assert.Single(stopRepository.GetStops().Result);
    }

    [Fact]
    public async Task TestCreateStopModelFailure()
    {
        var stopRepository = GetInMemoryRepository();
        var stopController = new StopController(stopRepository, GetLogger());
        var stop = new Stop() { Name = "test", Latitude = 0.0, Longitude = 0.0 };
        stopController.ModelState.AddModelError("Latitude", "Latitude can't be 0.0");

        var actionResult = await stopController.Create(stop) as RedirectToActionResult;

        Assert.NotNull(actionResult);
        Assert.Equal("Index", actionResult.ActionName);
        Assert.Empty(stopRepository.GetStops().Result);
    }

    [Fact]
    public async Task TestEditStop()
    {
        var stopRepository = GetInMemoryRepository();
        var stopController = new StopController(stopRepository, GetLogger());
        var stop = new Stop() { Name = "test", Latitude = 0.0, Longitude = 0.0 };
        var stopId = await stopRepository.AddStop(stop);

        var actionResult = await stopController.Edit(stopId) as ViewResult;

        var model = actionResult.ViewData.Model as Stop;

        Assert.NotNull(actionResult);
        Assert.True(model.Name == stop.Name && model.Longitude == stop.Longitude && model.Latitude == stop.Latitude);
    }

    [Fact]
    public async Task TestEditStopNotFound()
    {
        var stopRepository = GetInMemoryRepository();
        var stopController = new StopController(stopRepository, GetLogger());

        var actionResult = await stopController.Edit(781923);

        Assert.IsType<NotFoundResult>(actionResult);
    }

    [Fact]
    public async Task TestEditConfirmedStopNotFound()
    {
        var stopRepository = GetInMemoryRepository();
        var stopController = new StopController(stopRepository, GetLogger());
        var stop = new Stop() { Name = "test", Latitude = 0.0, Longitude = 0.0 };

        var actionResult = await stopController.EditConfirmed(781923, stop);

        Assert.IsType<NotFoundResult>(actionResult);
    }

    [Fact]
    public async Task TestEditConfirmedStop()
    {
        var stopRepository = GetInMemoryRepository();
        var stopController = new StopController(stopRepository, GetLogger());
        var stop = new Stop() { Name = "test", Latitude = 0.0, Longitude = 0.0 };
        var stopId = await stopRepository.AddStop(stop);

        var actionResult = await stopController.EditConfirmed(stopId, stop) as RedirectToActionResult;

        var result = stopRepository.GetStop(stopId);

        Assert.NotNull(actionResult);
        Assert.Equal("Index", actionResult.ActionName);
        Assert.Single(stopRepository.GetStops().Result);
        Assert.True(result.Result.Name == stop.Name && result.Result.Longitude == stop.Longitude && result.Result.Latitude == stop.Latitude);
    }

    [Fact]
    public async Task TestEditConfirmedStopInvalidState()
    {
        var stopRepository = GetInMemoryRepository();
        var stopController = new StopController(stopRepository, GetLogger());
        stopController.ModelState.AddModelError("Test", "Something is wrong");
        var stop = new Stop() { Name = "test", Latitude = 0.0, Longitude = 0.0 };
        var stopId = await stopRepository.AddStop(stop);

        var actionResult = await stopController.EditConfirmed(stopId, stop) as RedirectToActionResult;

        Assert.NotNull(actionResult);
        Assert.Equal("Index", actionResult.ActionName);
    }

    [Fact]
    public async Task TestDeleteStop()
    {
        //This reflects the GetStop() in delete
        var stopRepository = GetInMemoryRepository();
        var stopController = new StopController(stopRepository, GetLogger());
        var stop = new Stop() { Name = "test", Latitude = 0.0, Longitude = 0.0 };
        var stopId = await stopRepository.AddStop(stop);

        var actionResult = await stopController.Delete(stopId) as ViewResult;

        Assert.NotNull(actionResult);
        Assert.Single(stopRepository.GetStops().Result);
    }

    [Fact]
    public async Task TestDeleteStopNotFound()
    {
        var stopRepository = GetInMemoryRepository();
        var stopController = new StopController(stopRepository, GetLogger());

        var actionResult = await stopController.Delete(1337);

        Assert.IsType<NotFoundResult>(actionResult);
    }

    [Fact]
    public async Task TestDeleteConfirmedStop()
    {
        var stopRepository = GetInMemoryRepository();
        var stopController = new StopController(stopRepository, GetLogger());
        var stop = new Stop() { Name = "test", Latitude = 0.0, Longitude = 0.0 };
        var stopId = await stopRepository.AddStop(stop);

        var actionResult = await stopController.DeleteConfirmed(new int[] { stopId }) as RedirectToActionResult;

        Assert.NotNull(actionResult);
        Assert.Equal("Index", actionResult.ActionName);
        Assert.Empty(stopRepository.GetStops().Result);
    }
}
