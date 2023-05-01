using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Controllers
{
    public class EntryController : Controller
    {
        private readonly IEntryRepository _EntryRepository;

        public EntryController(IEntryRepository EntryRepository)
        {
            _EntryRepository = EntryRepository;
        }
        [Route("Entry")]
        [Route("Entry/Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _EntryRepository.GetEntries());
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(int SelectedStopId, [Bind("Id,Timestamp,Boarded,LeftBehind,Driver,Bus,Loop,Stop")] Entry Entry)
        {
            if (ModelState.IsValid)
            {
                await _EntryRepository.AddEntry(Entry);
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));

        }
        
        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> EditConfirmed(int id, [Bind("Id,Timestamp,Boarded,LeftBehind,Driver,Bus,Loop,Stop")] Entry Entry)
        {
            if (id != Entry.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _EntryRepository.UpdateEntry(Entry);
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
                await _EntryRepository.DeleteEntries(ids);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}