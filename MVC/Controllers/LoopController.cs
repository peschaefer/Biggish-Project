using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;
using MVC.ViewModels;

namespace MVC.Controllers
{
    public class LoopController : Controller
    {
        private readonly ILoopRepository _loopRepository;
        private readonly IStopRepository _stopRepository;

        public LoopController(ILoopRepository loopRepository, IStopRepository stopRepository)
        {
            _loopRepository = loopRepository;
            _stopRepository = stopRepository;
        }

        [Route("Loop")]
        [Route("Loop/Index")]
        public async Task<IActionResult> Index()
        {
            var viewModel = new LoopIndexViewModel
            {
                Loops = await _loopRepository.GetLoops(),
                CreateLoopViewModel = new CreateLoopViewModel
                {
                    Stops = await _stopRepository.GetStops()
                }
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name")] Loop loop)
        {
            if (ModelState.IsValid)
            {
                await _loopRepository.AddLoop(loop);
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