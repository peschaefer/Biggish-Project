using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;
using MVC.ViewModels;
using Route = MVC.Models.Route;

namespace MVC.Controllers
{
    public class LoopController : Controller
    {
        private readonly ILoopRepository _loopRepository;
        private readonly IStopRepository _stopRepository;
        private readonly IRouteRepository _routeRepository;

        public LoopController(ILoopRepository loopRepository, IStopRepository stopRepository, IRouteRepository routeRepository)
        {
            _loopRepository = loopRepository;
            _stopRepository = stopRepository;
            _routeRepository = routeRepository;
        }

        [Route("Loop")]
        [Route("Loop/Index")]
        public async Task<IActionResult> Index()
        {
            var stops = await _stopRepository.GetStops();
            await _routeRepository.GetRoutes();
            
            
            var viewModel = new LoopIndexViewModel
            {
                Loops = await _loopRepository.GetLoops(),
                CreateLoopViewModel = new CreateLoopViewModel
                {
                    Stops = stops
                }
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
                foreach (var routeViewModel in createLoopViewModel.Routes)
                {
                    Route route = new Route
                    {
                        Stop = await _stopRepository.GetStop(routeViewModel.SelectedStopId),
                        Order = routeViewModel.Order,
                        Loop = loop
                    };
                    await _routeRepository.AddRoute(route);
                    loop.Routes.Add(route);
                }

                await _loopRepository.UpdateLoop(loop);

                return RedirectToAction(nameof(Index));
            }


            return RedirectToAction(nameof(Index));
        }


        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditConfirmed(int id, [Bind("Id,Name")] Loop loop)
        {
            if (id != loop.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _loopRepository.UpdateLoop(loop);
                }
                catch (Exception)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int[] ids)
        {
            try
            {
                await _loopRepository.DeleteLoops(ids);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}