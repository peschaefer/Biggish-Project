using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Controllers
{
    public class LoopController : Controller
    {
        private readonly ILoopRepository _loopRepository;

        public LoopController(ILoopRepository loopRepository)
        {
            _loopRepository = loopRepository;
        }
        [Route("Loop")]
        [Route("Loop/Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _loopRepository.GetLoops());
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
