using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Tests;

public class RouteRepoTests
{
    Stop stop = new Stop()
    {
        Name = "Test",
        Latitude = 0.0,
        Longitude = 0.0
    };

    Loop loop = new Loop()
    {
        Name = "Test"
    };

    private BigishProjContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<BigishProjContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb-{Guid.NewGuid()}")
            .Options;

        return new BigishProjContext(options);
    }

    [Fact]
    public async Task TestAddAndGetRoute()
    {
        var dbContext = GetDbContext();
        var repository = new RouteRepository(dbContext);
        var newRoute = new Route { Order = 0, Stop = stop, Loop = loop };

        var routeId = await repository.AddRoute(newRoute);
        var addedRoute = await repository.GetRoute(routeId);

        Assert.NotNull(addedRoute);
        Assert.Equal(0, addedRoute.Order);
    }

    [Fact]
    public async Task TestGetRoutes()
    {
        var dbContext = GetDbContext();
        var repository = new RouteRepository(dbContext);
        var route1 = new Route { Order = 0, Stop = stop, Loop = loop };
        var route2 = new Route { Order = 1, Stop = stop, Loop = loop };

        await repository.AddRoute(route1);
        await repository.AddRoute(route2);

        var routes = await repository.GetRoutes();

        Assert.NotNull(routes);
        Assert.Equal(2, routes.Count);
        Assert.Contains(routes, route => route.Order == 0);
        Assert.Contains(routes, route => route.Order == 1);
    }

    [Fact]
    public async Task TestUpdateRoute()
    {
        var dbContext = GetDbContext();
        var repository = new RouteRepository(dbContext);
        var newRoute = new Route { Order = 0, Stop = stop, Loop = loop };
        var routeId = await repository.AddRoute(newRoute);
        newRoute.Id = routeId;
        newRoute.Order = 420;

        var updatedRoute = await repository.UpdateRoute(newRoute);

        Assert.NotNull(updatedRoute);
        Assert.Equal(420, updatedRoute.Order);
    }

    [Fact]
    public async Task TestDeleteRoutes()
    {
        var dbContext = GetDbContext();
        var repository = new RouteRepository(dbContext);
        var route1 = new Route { Order = 0, Stop = stop, Loop = loop };
        var route2 = new Route { Order = 1, Stop = stop, Loop = loop };
        var route1Id = await repository.AddRoute(route1);
        var route2Id = await repository.AddRoute(route2);

        await repository.DeleteRoutes(new int[] { route1Id, route2Id });
        var remainingRoutes = await repository.GetRoutes();

        Assert.Empty(remainingRoutes);
    }
}