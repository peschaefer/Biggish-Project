using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Controllers;
using MVC.Models;
using MVC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MVC.Tests;

public class BusControllerTests
{
    private ILogger<BusController> GetLogger()
    {
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        return loggerFactory.CreateLogger<BusController>();
    }

    private IBusRepository GetInMemoryRepository()
    {
        var options = new DbContextOptionsBuilder<BigishProjContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb{Guid.NewGuid()}")
            .Options;
        
        var dbContext = new BigishProjContext(options);
        return new BusRepository(dbContext);
    }

    [Fact]
    public async Task TestIndexReturnsViewWithListOfBuses()
    {
        var busRepository = GetInMemoryRepository();
        var busController = new BusController(busRepository, GetLogger());

        var result = await busController.Index() as ViewResult;

        var model = result.ViewData.Model as List<Bus>;
        Assert.NotNull(model);
        Assert.Empty(model);
    }

    [Fact]
    public async Task TestCreateReturnsRedirectToActionIndexOnSuccess()
    {
        var busRepository = GetInMemoryRepository();
        var busController = new BusController(busRepository, GetLogger());
        var bus = new Bus { BusNumber = 123 };

        var result = await busController.Create(bus) as RedirectToActionResult;

        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        Assert.Single(busRepository.GetBuses().Result);
    }

    [Fact]
    public async Task TestCreateReturnsRedirectToActionIndexOnFailure()
    {
        var busRepository = GetInMemoryRepository();
        var busController = new BusController(busRepository, GetLogger());
        var bus = new Bus { BusNumber = 0 };
        busController.ModelState.AddModelError("BusNumber", "The BusNumber field is required");

        var result = await busController.Create(bus) as RedirectToActionResult;

        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        Assert.Empty(busRepository.GetBuses().Result);
    }

    [Fact]
    public async Task TestEditReturnsRedirectToActionIndexOnSuccess()
    {
        var busRepository = GetInMemoryRepository();
        var busController = new BusController(busRepository, GetLogger());
        var bus = new Bus { BusNumber = 123 };
        var busId = await busRepository.AddBus(bus);

        var result = await busController.EditConfirmed(busId, bus) as RedirectToActionResult;

        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        Assert.Single(busRepository.GetBuses().Result);
        Assert.Equal(123, busRepository.GetBus(busId).Result.BusNumber);
    }
}