using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Controllers
{
    [Authorize]
    public class StopController : Controller
    {
        private readonly IStopRepository _stopRepository;
        private readonly ILogger<StopController> _logger;

        public StopController(IStopRepository stopRepository, ILogger<StopController> logger)
        {
            _stopRepository = stopRepository;
            _logger = logger;
        }

        [Route("Stop")]
        [Route("Stop/Index")]
        public async Task<IActionResult> Index()
        {
            var stops = await _stopRepository.GetStops();
            _logger.LogInformation("Retrieved {count} stops at {time}", stops.Count, DateTime.Now);
            return View(stops);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,Latitude,Longitude")] Stop stop)
        {
            if (ModelState.IsValid)
            {
                await _stopRepository.AddStop(stop);
                _logger.LogInformation("Stop with id {id} created at {time}", stop.Id, DateTime.Now);
                return RedirectToAction(nameof(Index));
            }

            _logger.LogWarning("Stop creation failed due to invalid model state at {time}", DateTime.Now);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var stop = await _stopRepository.GetStop(id);
            if (stop == null)
            {
                _logger.LogWarning("Stop with id {id} not found at {time}", id, DateTime.Now);
                return NotFound();
            }

            _logger.LogInformation("Retrieved stop with id {id} at {time}", id, DateTime.Now);
            return View(stop);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditConfirmed(int id, [Bind("Id,Name,Latitude,Longitude")] Stop stop)
        {
            if (id != stop.Id)
            {
                _logger.LogWarning("Stop with id {id} does not exist at {time}", id, DateTime.Now);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _stopRepository.UpdateStop(stop);
                    _logger.LogInformation("Updated stop with id {id} at {time}", id, DateTime.Now);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError("Failed to update stop with id {id} due to {exception} at {time}", id, ex.Message, DateTime.Now);
                    return NotFound();
                }
            }

            _logger.LogWarning("Stop update failed due to invalid model state at {time}", DateTime.Now);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var stop = await _stopRepository.GetStop(id);
            if (stop == null)
            {
                _logger.LogWarning("Stop with id {id} not found at {time}", id, DateTime.Now);
                return NotFound();
            }

            _logger.LogInformation("Retrieved stop with id {id} at {time}", id, DateTime.Now);
            return View(stop);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int[] ids)
        {
            try
            {
                await _stopRepository.DeleteStops(ids);
                _logger.LogInformation("Deleted stops with ids {ids} at {time}", string.Join(", ", ids.Select(id => id.ToString())), DateTime.Now);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Deleting Stops Failed with exception {exception} at {time}.", ex.Message, DateTime.Now);
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}