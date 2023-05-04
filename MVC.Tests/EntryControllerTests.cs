using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MVC.Controllers;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Tests;

public class EntryControllerTests
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

    private ILogger<EntryController> GetLogger()
    {
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        return loggerFactory.CreateLogger<EntryController>();
    }

    private BigishProjContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<BigishProjContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb{Guid.NewGuid()}")
            .Options;

        return new BigishProjContext(options);
    }

    private IEntryRepository GetInMemoryEntryRepository(BigishProjContext dbContext)
    {
        return new EntryRepository(dbContext);
    }

    private ILoopRepository GetInMemoryLoopRepository(BigishProjContext dbContext)
    {
        return new LoopRepository(dbContext);
    }

    private IStopRepository GetInMemoryStopRepository(BigishProjContext dbContext)
    {
        return new StopRepository(dbContext);
    }

    private IBusRepository GetInMemoryBusRepository(BigishProjContext dbContext)
    {
        return new BusRepository(dbContext);
    }

    [Fact]
    public async Task TestIndex()
    {
        var dbContext = GetInMemoryDbContext();
        var loopRepository = GetInMemoryLoopRepository(dbContext);
        var stopRepository = GetInMemoryStopRepository(dbContext);
        var entryRepository = GetInMemoryEntryRepository(dbContext);
        var busRepository = GetInMemoryBusRepository(dbContext);

        var entryController = new EntryController(entryRepository, stopRepository, new FakeUserManager(), busRepository, loopRepository, GetLogger());

        var result = await entryController.Index() as ViewResult;

        var model = result.ViewData.Model as List<Entry>;

        Assert.NotNull(model);
        Assert.Empty(model);
    }

    [Fact]
    public async Task TestEditConfirmed()
    {
        var dbContext = GetInMemoryDbContext();
        var loopRepository = GetInMemoryLoopRepository(dbContext);
        var stopRepository = GetInMemoryStopRepository(dbContext);
        var entryRepository = GetInMemoryEntryRepository(dbContext);
        var busRepository = GetInMemoryBusRepository(dbContext);
        var entryController = new EntryController(entryRepository, stopRepository, new FakeUserManager(), busRepository, loopRepository, GetLogger());

        var entry = new Entry { Timestamp = Convert.ToDateTime("5/1/2023"), Boarded = 0, LeftBehind = 0, Driver = driver, Bus = bus, Loop = loop, Stop = stop };
        var entryId = await entryRepository.AddEntry(entry);

        var actionResult = await entryController.EditConfirmed(entryId, entry) as RedirectToActionResult;

        var result = entryRepository.GetEntry(entryId);

        Assert.NotNull(actionResult);
        Assert.Equal("Index", actionResult.ActionName);
        Assert.Single(entryRepository.GetEntries().Result);
        Assert.True(result.Result.Equals(entry));
    }

    [Fact]
    public async Task TestDeleteConfirmed()
    {
        var dbContext = GetInMemoryDbContext();
        var loopRepository = GetInMemoryLoopRepository(dbContext);
        var stopRepository = GetInMemoryStopRepository(dbContext);
        var entryRepository = GetInMemoryEntryRepository(dbContext);
        var busRepository = GetInMemoryBusRepository(dbContext);
        var entryController = new EntryController(entryRepository, stopRepository, new FakeUserManager(), busRepository, loopRepository, GetLogger());

        var entry = new Entry { Timestamp = Convert.ToDateTime("5/1/2023"), Boarded = 0, LeftBehind = 0, Driver = driver, Bus = bus, Loop = loop, Stop = stop };
        var entryId = await entryRepository.AddEntry(entry);

        var actionResult = await entryController.DeleteConfirmed(new int[] { entryId }) as RedirectToActionResult;

        var result = entryRepository.GetEntry(entryId);

        Assert.NotNull(actionResult);
        Assert.Equal("Index", actionResult.ActionName);
        Assert.Empty(entryRepository.GetEntries().Result);
    }


}
