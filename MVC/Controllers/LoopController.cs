using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;
using MVC.ViewModels;
using Route = MVC.Models.Route;

namespace MVC.Controllers
{
    [Authorize]
    public class LoopController : Controller
    {
        private readonly ILoopRepository _loopRepository;
        private readonly IStopRepository _stopRepository;
        private readonly IRouteRepository _routeRepository;
        private readonly IEntryRepository _entryRepository;
        private readonly ILogger<LoopController> _logger;

        public LoopController(ILoopRepository loopRepository, IStopRepository stopRepository,
            IRouteRepository routeRepository,ILogger<LoopController> logger, IEntryRepository entryRepository)
        {
            _loopRepository = loopRepository;
            _stopRepository = stopRepository;
            _routeRepository = routeRepository;
            _entryRepository = entryRepository;
            _logger = logger;
        }

        [Route("Loop")]
        [Route("Loop/Index")]
        public async Task<IActionResult> Index()
        {
            var stops = await _stopRepository.GetStops();
            var loops = await _loopRepository.GetLoops();
            var entries = await _entryRepository.GetEntries();

            var loopStops = new Dictionary<int, List<Stop>>();
            foreach (var loop in loops)
            {
                loopStops.Add(loop.Id, loop.Routes.Select(r => r.Stop).ToList());
            }

            var currentTime = DateTime.Now;
            var recentEntries = entries.Where(entry => (currentTime - entry.Timestamp).TotalMinutes <= 15).ToList();
            var stopsWithPassengerCount = stops.Select(stop => new StopWithPassengerCount
            {
                Id = stop.Id,
                Name = stop.Name,
                Latitude = stop.Latitude,
                Longitude = stop.Longitude,
                Passengers = recentEntries.Where(entry => entry.Stop.Id == stop.Id).Sum(entry => entry.Boarded),
            }).ToList();
            
            var viewModel = new LoopIndexViewModel
            {
                Loops = await _loopRepository.GetLoops(),
                CreateLoopViewModel = new CreateLoopViewModel
                {
                    Stops = stops
                },
                MapViewModel = new MapViewModel { Stops = stopsWithPassengerCount },
                LoopStops = loopStops
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Create([Bind("Loop, Routes")] CreateLoopViewModel createLoopViewModel)
        {
            if (ModelState.IsValid)
            {
                Loop loop = new Loop { Name = createLoopViewModel.Loop.Name };
                
                // Add the routes to the loop
                var routes = new List<Route>();
                foreach (var routeViewModel in createLoopViewModel.Routes)
                {
                     Route route = new Route
                    {
                        Stop = await _stopRepository.GetStop(routeViewModel.SelectedStopId),
                        Order = routeViewModel.Order,
                        Loop = loop
                    };
                     routes.Add(route);
                }

                await _loopRepository.AddLoopWithRoutes(loop, routes);
                
                _logger.LogInformation("Loop {id} created at {time}", loop.Id, DateTime.Now);

                return RedirectToAction(nameof(Index));
            }

            _logger.LogError("Failed to create loop at {time}", DateTime.Now);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditConfirmed(int id, [Bind("Loop,Routes")] CreateLoopViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var existingLoop = await _loopRepository.GetLoop(viewModel.Loop.Id);

                    existingLoop.Name = viewModel.Loop.Name;

                    await _routeRepository.RemoveRoutesByLoopId(existingLoop.Id);

                    // Add new routes based on the RouteViewModels
                    foreach (var routeViewModel in viewModel.Routes)
                    {
                        var stop = await _stopRepository.GetStop(routeViewModel.SelectedStopId);
                        var route = new Route
                        {
                            Stop = stop,
                            Order = routeViewModel.Order,
                            Loop = existingLoop
                        };
                        await _routeRepository.AddRoute(route);
                    }

                    // Update the loop
                    await _loopRepository.UpdateLoop(existingLoop);
                     _logger.LogInformation("Updated loop {id} at {time}", existingLoop.Id, DateTime.Now);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _logger.LogError("Failed to update loop with exception {exception} at {time}", ex.Message, DateTime.Now);
                    return RedirectToAction(nameof(Index));
                }
            }

            return RedirectToAction(nameof(Index));
        }




        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int[] ids)
        {
            try
            {
                await _loopRepository.DeleteLoops(ids);
                _logger.LogInformation("Deleted loops with ids {ids} at {time}", string.Join(", ", ids.Select(id => id.ToString())),DateTime.Now);
            }
            catch (Exception e)
            {
                _logger.LogError("Delete loop failed with exception {exception} at {time}.", e.Message, DateTime.Now);
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        // public async Task<IActionResult> MapStops()
        // {
        //     var stops = await _stopRepository.GetStops();
        //     var viewModel = new MapViewModel { Stops = stops };
        //     return View(viewModel);
        // }
    }
}