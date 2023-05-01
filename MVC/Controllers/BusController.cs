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

        public BusController(IBusRepository busRepository)
        {
            _busRepository = busRepository;
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
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");

        }
        
        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditConfirmed(int id, [Bind("Id,BusNumber")] Bus bus)
        {
            if (id != bus.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _busRepository.UpdateBus(bus);
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
                await _busRepository.DeleteBuses(ids);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
