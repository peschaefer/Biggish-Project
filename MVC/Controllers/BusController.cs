using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Controllers
{
    public class BusController : Controller
    {
        private readonly IBusRepository _busRepository;

        public BusController(IBusRepository busRepository)
        {
            _busRepository = busRepository;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _busRepository.GetBuses());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,BusNumber")] Bus bus)
        {
            if (ModelState.IsValid)
            {
                await _busRepository.AddBus(bus);
                return RedirectToAction(nameof(Index));
            }

            return View(bus);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var bus = await _busRepository.GetBus(id);
            if (bus == null)
            {
                return NotFound();
            }

            return View(bus);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,Seats,CompanyId")] Bus bus)
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

            return View(bus);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var bus = await _busRepository.GetBus(id);
            if (bus == null)
            {
                return NotFound();
            }

            return View(bus);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _busRepository.DeleteBus(id);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
