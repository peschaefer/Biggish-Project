using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Tests;

public class EntryRepoTests
{



    [Fact]
    public async void TestAddEntry()
    {
        var connection = new SqliteConnection("DataSource=:memory:");

        var option = new DbContextOptionsBuilder<BigishProjContext>()
            .UseSqlite(connection)
            .Options;

        var context = new BigishProjContext(option);

        BusRepository busRepository = new BusRepository(context);
        busRepository.AddBus(bus1);
        var retrievedBus = context.Buses.FindAsync(1);

        Assert.Equal(bus1.Id, retrievedBus.Result.Id);
        Assert.Equal(bus1.BusNumber, retrievedBus.Result.BusNumber);

        var deletion = busRepository.DeleteBuses(new int[1]);
    }

    [Fact]
    public async void TestGetEntry()
    {
        var connection = new SqliteConnection("DataSource=:memory:");

        var option = new DbContextOptionsBuilder<BigishProjContext>()
            .UseSqlite(connection)
            .Options;

        var context = new BigishProjContext(option);

        BusRepository busRepository = new BusRepository(context);
        busRepository.AddBus(bus1);
        var retrievedBus = busRepository.GetBus(1);

        Assert.Equal(bus1.Id, retrievedBus.Result.Id);
        Assert.Equal(bus1.BusNumber, retrievedBus.Result.BusNumber);

        var deletion = busRepository.DeleteBuses(new int[1]);
    }

    [Fact]
    public async void TestUpdateEntry()
    {
        var connection = new SqliteConnection("DataSource=:memory:");

        var option = new DbContextOptionsBuilder<BigishProjContext>()
            .UseSqlite(connection)
            .Options;

        var context = new BigishProjContext(option);

        BusRepository busRepository = new BusRepository(context);

        busRepository.AddBus(bus2);

        Bus replacement = new Bus()
        {
            Id = 2,
            BusNumber = 3
        };

        busRepository.UpdateBus(replacement);

        var retrievedBus = busRepository.GetBus(2);

        Assert.Equal(replacement.Id, retrievedBus.Result.Id);
        Assert.Equal(replacement.BusNumber, retrievedBus.Result.BusNumber);

        var deletion = busRepository.DeleteBuses(new int[2]);
    }

    [Fact]
    public async void TestDeleteEntry()
    {
        Assert.True(false);
    }
}