using Microsoft.EntityFrameworkCore;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Tests;

public class BusRepoTests
{
    private BigishProjContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<BigishProjContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb{Guid.NewGuid()}")
            .Options;

        return new BigishProjContext(options);
    }
    
    [Fact]
    public async Task TestAddAndGetBus()
    {
        // add and get are so closely related it makes sense to test them together
        var dbContext = GetDbContext();
        var repository = new BusRepository(dbContext);
        var newBus = new Bus { BusNumber = 123 };

        var busId = await repository.AddBus(newBus);
        var addedBus = await repository.GetBus(busId);

        Assert.NotNull(addedBus);
        Assert.Equal(123, addedBus.BusNumber);
    }

    [Fact]
    public async Task TestGetBuses()
    {
        var dbContext = GetDbContext();
        var repository = new BusRepository(dbContext);
        var bus1 = new Bus { BusNumber = 123 };
        var bus2 = new Bus { BusNumber = 456 };

        await repository.AddBus(bus1);
        await repository.AddBus(bus2);

        var buses = await repository.GetBuses();

        Assert.NotNull(buses);
        Assert.Equal(2, buses.Count);
        Assert.Contains(buses, bus => bus.BusNumber == 123);
        Assert.Contains(buses, bus => bus.BusNumber == 456);
    }

    [Fact]
    public async Task TestUpdateBus()
    {
        var dbContext = GetDbContext();
        var repository = new BusRepository(dbContext);
        var newBus = new Bus { BusNumber = 123 };
        var busId = await repository.AddBus(newBus);
        newBus.Id = busId;
        newBus.BusNumber = 456;

        var updatedBus = await repository.UpdateBus(newBus);

        Assert.NotNull(updatedBus);
        Assert.Equal(456, updatedBus.BusNumber);
    }
    
    [Fact]
    public async void TestDeleteBuses()
    {
        var dbContext = GetDbContext();
        var repository = new BusRepository(dbContext);
        var bus1 = new Bus { BusNumber = 123 };
        var bus2 = new Bus { BusNumber = 456 };
        var bus1Id = await repository.AddBus(bus1);
        var bus2Id = await repository.AddBus(bus2);

        await repository.DeleteBuses(new int[] { bus1Id, bus2Id });
        var remainingBuses = await repository.GetBuses();

        Assert.Empty(remainingBuses);
    }
}