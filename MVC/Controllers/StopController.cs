using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Controllers
{
    [Authorize]
    public class StopController : Controller
    {
        private readonly IStopRepository _stopRepository;

        public StopController(IStopRepository StopRepository)
        {
            _stopRepository = StopRepository;
        }
        [Route("Stop")]
        [Route("Stop/Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _stopRepository.GetStops());
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,Latitude,Longitude")] Stop Stop)
        {
            if (ModelState.IsValid)
            {
                await _stopRepository.AddStop(Stop);
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));

        }
        
        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditConfirmed(int id, [Bind("Id,Timestamp,Boarded,LeftBehind,Driver,Bus,Loop,Stop")] Stop Stop)
        {
            if (id != Stop.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _stopRepository.UpdateStop(Stop);
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
                await _stopRepository.DeleteStops(ids);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}