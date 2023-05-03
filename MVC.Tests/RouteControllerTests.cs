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

public class RouteControllerTests
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

    private ILogger<RouteController> GetLogger()
    {
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        return loggerFactory.CreateLogger<RouteController>();
    }

    private IRouteRepository GetInMemoryRepository()
    {
        var options = new DbContextOptionsBuilder<BigishProjContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb{Guid.NewGuid()}")
            .Options;

        var dbContext = new BigishProjContext(options);
        return new RouteRepository(dbContext);
    }

    [Fact]
    public async Task TestGetRoutes()
    {
        var routeRepository = GetInMemoryRepository();
        var routeController = new RouteController(routeRepository, GetLogger());

        var actionResult = await routeController.GetAllRoutes();

        var result = actionResult.Result as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task TestCreateRoute()
    {
        var routeRepository = GetInMemoryRepository();
        var routeController = new RouteController(routeRepository, GetLogger());
        var route = new Route() { Order = 1, Stop = stop, Loop = loop };

        var actionResult = await routeController.CreateRoute(route);

        var result = actionResult.Result as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task TestGetRoute()
    {
        var routeRepository = GetInMemoryRepository();
        var routeController = new RouteController(routeRepository, GetLogger());
        var route = new Route() { Order = 1, Stop = stop, Loop = loop };

        var routeId = await routeRepository.AddRoute(route);

        var actionResult = await routeController.GetRoute(routeId);

        var result = actionResult.Result as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task TestUpdateRoute()
    {
        var routeRepository = GetInMemoryRepository();
        var routeController = new RouteController(routeRepository, GetLogger());
        var route = new Route() { Order = 1, Stop = stop, Loop = loop };
        var routeId = await routeRepository.AddRoute(route);
        route.Id = routeId;
        route.Order = 75;

        var actionResult = await routeController.UpdateRoute(routeId, route);

        var result = actionResult as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public async Task TestDeleteRoute()
    {
        var routeRepository = GetInMemoryRepository();
        var routeController = new RouteController(routeRepository, GetLogger());
        var route = new Route() { Order = 1, Stop = stop, Loop = loop };
        var routeId = await routeRepository.AddRoute(route);

        var actionResult = await routeController.DeleteRoutes(new int[] { routeId } );

        var result = actionResult as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
    }
}