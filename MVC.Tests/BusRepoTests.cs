using Microsoft.EntityFrameworkCore;
using MVC.Models;
using MVC.Repositories;
using Moq;
using System.Reflection.Metadata;
using Castle.Components.DictionaryAdapter.Xml;
using MVC.Controllers;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore.Sqlite;
using Microsoft.Data.Sqlite;

namespace MVC.Tests;

public class BusRepoTests
{
    Bus bus1 = new Bus()
    {
        BusNumber = 1,
        Id = 1
    };

    Bus bus2 = new Bus()
    {
        BusNumber = 2,
        Id = 2
    };

    [Fact]
    public async void TestAddBus()
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
    public async void TestGetBus()
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
    public async void TestUpdateBus()
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
    public async void TestUpdateBusNotFound()
    {
        Assert.True(false);

    }

    [Fact]
    public async void TestDeleteBuses()
    {
        Assert.True(false);
    }
}