using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Tests;

public class RouteRepoTests
{
    Route route1 = new Route()
    {
        Id = 1,
        Order = 1
    };


    [Fact]
    public async void TestAddLoop()
    {
        var connection = new SqliteConnection("DataSource=:memory:");

        var option = new DbContextOptionsBuilder<BigishProjContext>()
            .UseSqlite(connection)
            .Options;

        var context = new BigishProjContext(option);

        RouteRepository routeRepository = new RouteRepository(context);
        routeRepository.AddRoute(route1);
        var retrievedEntry = context.Routes.FindAsync(1);

        Assert.Equal(route1.Id, retrievedEntry.Result.Id);
        Assert.Equal(route1.Order, retrievedEntry.Result.Order);

        var deletion = routeRepository.DeleteRoutes(new int[1]);
    }

    [Fact]
    public async void TestGetLoop()
    {
        var connection = new SqliteConnection("DataSource=:memory:");

        var option = new DbContextOptionsBuilder<BigishProjContext>()
            .UseSqlite(connection)
            .Options;

        var context = new BigishProjContext(option);

        RouteRepository routeRepository = new RouteRepository(context);
        routeRepository.AddRoute(route1);
        var retrievedEntry = routeRepository.GetRoute(1);

        Assert.Equal(route1.Id, retrievedEntry.Result.Id);
        Assert.Equal(route1.Order, retrievedEntry.Result.Order);

        var deletion = routeRepository.DeleteRoutes(new int[1]);
    }

    [Fact]
    public async void TestUpdateLoop()
    {
        var connection = new SqliteConnection("DataSource=:memory:");

        var option = new DbContextOptionsBuilder<BigishProjContext>()
            .UseSqlite(connection)
            .Options;

        var context = new BigishProjContext(option);

        RouteRepository routeRepository = new RouteRepository(context);

        routeRepository.AddRoute(route1);

        Route replacement = new Route()
        {
            Id = 1,
            Order = 75
        };

        routeRepository.UpdateRoute(replacement);

        var retrievedEntry = routeRepository.GetRoute(1);

        Assert.Equal(replacement.Id, retrievedEntry.Result.Id);
        Assert.Equal(replacement.Order, retrievedEntry.Result.Order);

        var deletion = routeRepository.DeleteRoutes(new int[1]);
    }

    [Fact]
    public async void TestDeleteEntry()
    {
        Assert.True(false);
    }
}