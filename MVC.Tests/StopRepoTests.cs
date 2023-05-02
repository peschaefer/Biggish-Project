using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Tests;

public class StopRepoTests
{
    Stop stop1 = new Stop()
    {
        Id = 1,
        Name = "Test",
        Latitude = 0.0,
        Longitude = 0.0
    };


    [Fact]
    public async void TestAddStop()
    {
        var connection = new SqliteConnection("DataSource=:memory:");

        var option = new DbContextOptionsBuilder<BigishProjContext>()
            .UseSqlite(connection)
            .Options;

        var context = new BigishProjContext(option);

        StopRepository stopRepository = new StopRepository(context);
        stopRepository.AddStop(stop1);
        var retrievedEntry = context.Stops.FindAsync(1);

        Assert.Equal(stop1.Id, retrievedEntry.Result.Id);
        Assert.Equal(stop1.Name, retrievedEntry.Result.Name);
        Assert.Equal(stop1.Latitude, retrievedEntry.Result.Latitude);
        Assert.Equal(stop1.Longitude, retrievedEntry.Result.Longitude);

        var deletion = stopRepository.DeleteStop(1);
    }

    [Fact]
    public async void TestGetStop()
    {
        var connection = new SqliteConnection("DataSource=:memory:");

        var option = new DbContextOptionsBuilder<BigishProjContext>()
            .UseSqlite(connection)
            .Options;

        var context = new BigishProjContext(option);

        StopRepository stopRepository = new StopRepository(context);
        stopRepository.AddStop(stop1);
        var retrievedEntry = stopRepository.GetStop(1);

        Assert.Equal(stop1.Id, retrievedEntry.Result.Id);
        Assert.Equal(stop1.Name, retrievedEntry.Result.Name);
        Assert.Equal(stop1.Latitude, retrievedEntry.Result.Latitude);
        Assert.Equal(stop1.Longitude, retrievedEntry.Result.Longitude);

        var deletion = stopRepository.DeleteStop(1);
    }

    [Fact]
    public async void TestUpdateStop()
    {
        var connection = new SqliteConnection("DataSource=:memory:");

        var option = new DbContextOptionsBuilder<BigishProjContext>()
            .UseSqlite(connection)
            .Options;

        var context = new BigishProjContext(option);

        StopRepository stopRepository = new StopRepository(context);

        stopRepository.AddStop(stop1);

        Stop replacement = new Stop()
        {
            Id = 1,
            Name = "New Stop",
            Latitude = 1.4,
            Longitude = 2.33
        };

        stopRepository.UpdateStop(replacement);

        var retrievedEntry = stopRepository.GetStop(1);

        Assert.Equal(replacement.Id, retrievedEntry.Result.Id);
        Assert.Equal(replacement.Name, retrievedEntry.Result.Name);
        Assert.Equal(replacement.Latitude, retrievedEntry.Result.Latitude);
        Assert.Equal(replacement.Longitude, retrievedEntry.Result.Longitude);

        var deletion = stopRepository.DeleteStop(1);
    }

    [Fact]
    public async void TestDeleteStop()
    {
        Assert.True(false);
    }
}