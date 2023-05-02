using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Controllers
{
    [Authorize(Policy = "ManagerOnly")]
    public class BusController : Controller
    {
        private readonly IBusRepository _busRepository;
        private readonly ILogger<BusController> _logger;

        public BusController(IBusRepository busRepository, ILogger<BusController> logger)
        {
            _busRepository = busRepository;
            _logger = logger;
        }
        [Route("Bus")]
        [Route("Bus/Index")]
        public async Task<IActionResult> Index()
        {
            var buses = await _busRepository.GetBuses();
            return View(buses);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,BusNumber")] Bus bus)
        {
            if (ModelState.IsValid)
            {
                await _busRepository.AddBus(bus);
                _logger.LogInformation("Created new bus with id {id} and bus number {number} at {time}", bus.Id, bus.BusNumber,DateTime.Now);
                return RedirectToAction("Index");
            }

            _logger.LogWarning("Failed to create new bus at {time}", DateTime.Now);
            return RedirectToAction("Index");

        }
        
        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditConfirmed(int id, [Bind("Id,BusNumber")] Bus bus)
        {
            if (id != bus.Id)
            {
                _logger.LogWarning("Bus with id {id} not found at {time}.", bus.Id, DateTime.Now);
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _busRepository.UpdateBus(bus);
                    _logger.LogInformation("Edited bus with id {id} at {time}", bus.Id,DateTime.Now);
                }
                catch (Exception e)
                {
                    _logger.LogWarning("Edit Failed with exception {exception} at {time}.", e, DateTime.Now);
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
                await _busRepository.DeleteBuses(ids);
                _logger.LogInformation("Deleted busses with ids {ids} at {time}", string.Join(", ", ids.Select(id => id.ToString())),DateTime.Now);
            }
            catch (Exception e)
            {
                _logger.LogWarning("Delete Failed with exception {exception} at {time}.", e, DateTime.Now);
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
