using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MVC.Models;
using MVC.Repositories;
using Route = MVC.Models.Route;

namespace MVC.Data
{
    public static class DatabaseSeeder
    {
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            var busRepository = services.GetRequiredService<IBusRepository>();
            var loopRepository = services.GetRequiredService<ILoopRepository>();
            var routeRepository = services.GetRequiredService<IRouteRepository>();
            var stopRepository = services.GetRequiredService<IStopRepository>();

            // Seed Bus data
            var busData = new[]
            {
                new Bus { BusNumber = 1 },
                new Bus { BusNumber = 2 },
                new Bus { BusNumber = 3 },
            };

            foreach (var bus in busData)
            {
                await busRepository.AddBus(bus);
            }

            // Seed Stop data
            var stopData = new[]
            {
                new Stop { Name = "Anthony", Latitude = 40.210539, Longitude = -85.411956 },
                new Stop { Name = "Bell Tower", Latitude = 40.20381703953198, Longitude = -85.40781540467735 },
                new Stop { Name = "Teachers College", Latitude = 40.20067881970986, Longitude = -85.40789989830907 },
            };

            foreach (var stop in stopData)
            {
                await stopRepository.AddStop(stop);
            }
            
            // Seed Loop data
            var loopData = new[]
            {
                new Loop { Name = "Loop A" },
                new Loop { Name = "Loop B" },
            };

            foreach (var loop in loopData)
            {
                await loopRepository.AddLoop(loop);
            }

            var loopA = loopData[0];
            var loopB = loopData[1];

            var stop1 = stopData[0];
            var stop2 = stopData[1];
            var stop3 = stopData[2];

            var loopARoutes = new List<Route>
            {
                new Route { Order = 1, Stop = stop1 },
                new Route { Order = 2, Stop = stop2 },
                new Route { Order = 3, Stop = stop3 }
            };

            var loopBRoutes = new List<Route>
            {
                new Route { Order = 1, Stop = stop3 },
                new Route { Order = 2, Stop = stop2 },
            };

            loopA.Routes.AddRange(loopARoutes);
            loopB.Routes.AddRange(loopBRoutes);

            await loopRepository.UpdateLoop(loopA);
            await loopRepository.UpdateLoop(loopB);
        }
    }
}


