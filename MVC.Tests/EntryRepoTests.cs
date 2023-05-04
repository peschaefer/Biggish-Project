using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Tests;

public class EntryRepoTests
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

    private BigishProjContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<BigishProjContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb-{Guid.NewGuid()}")
            .Options;

        return new BigishProjContext(options);
    }

    [Fact]
    public async Task TestAddAndGetEntry()
    {
        var dbContext = GetDbContext();
        var repository = new EntryRepository(dbContext);
        var newEntry = new Entry { Timestamp = Convert.ToDateTime("5/2/2023"), Boarded = 0, LeftBehind = 0, Driver = driver, Bus = bus, Loop = loop, Stop = stop};

        var entryId = await repository.AddEntry(newEntry);
        var addedEntry = await repository.GetEntry(entryId);

        Assert.NotNull(addedEntry);
        Assert.Equal(Convert.ToDateTime("5/2/2023"), addedEntry.Timestamp);
        Assert.Equal(0, addedEntry.Boarded);
        Assert.Equal(0, addedEntry.LeftBehind);
    }

    [Fact]
    public async Task TestGetEntries()
    {
        var dbContext = GetDbContext();
        var repository = new EntryRepository(dbContext);
        var entry1 = new Entry { Timestamp = Convert.ToDateTime("5/1/2023"), Boarded = 0, LeftBehind = 0, Driver = driver, Bus = bus, Loop = loop, Stop = stop };
        var entry2 = new Entry { Timestamp = Convert.ToDateTime("5/2/2023"), Boarded = 1, LeftBehind = 1, Driver = driver, Bus = bus, Loop = loop, Stop = stop };

        await repository.AddEntry(entry1);
        await repository.AddEntry(entry2);

        var entries = await repository.GetEntries();

        Assert.NotNull(entries);
        Assert.Equal(2, entries.Count);
        Assert.Contains(entries, entry => entry.Timestamp == Convert.ToDateTime("5/1/2023") && entry.Boarded == 0 && entry.LeftBehind == 0);
        Assert.Contains(entries, entry => entry.Timestamp == Convert.ToDateTime("5/2/2023") && entry.Boarded == 1 && entry.LeftBehind == 1);
    }

    [Fact]
    public async Task TestUpdateEntry()
    {
        var dbContext = GetDbContext();
        var repository = new EntryRepository(dbContext);
        var newEntry = new Entry { Timestamp = Convert.ToDateTime("5/2/2023"), Boarded = 0, LeftBehind = 0, Driver = driver, Bus = bus, Loop = loop, Stop = stop };
        var entryId = await repository.AddEntry(newEntry);
        newEntry.Id = entryId;
        newEntry.Boarded = 35;

        var updatedEntry = await repository.UpdateEntry(newEntry);

        Assert.NotNull(updatedEntry);
        Assert.Equal(35, updatedEntry.Boarded);
    }

    [Fact]
    public async Task TestDeleteEntries()
    {
        var dbContext = GetDbContext();
        var repository = new EntryRepository(dbContext);
        var entry1 = new Entry { Timestamp = Convert.ToDateTime("5/2/2023"), Boarded = 0, LeftBehind = 0, Driver = driver, Bus = bus, Loop = loop, Stop = stop };
        var entry2 = new Entry { Timestamp = Convert.ToDateTime("5/2/2023"), Boarded = 1, LeftBehind = 1, Driver = driver, Bus = bus, Loop = loop, Stop = stop };
        var entry1Id = await repository.AddEntry(entry1);
        var entry2Id = await repository.AddEntry(entry2);

        await repository.DeleteEntries(new int[] { entry1Id, entry2Id });
        var remainingEntries = await repository.GetEntries();

        Assert.Empty(remainingEntries);
    }
}