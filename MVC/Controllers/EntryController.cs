using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;
using MVC.ViewModels;

namespace MVC.Controllers
{
    public class EntryController : Controller
    {
        private readonly IEntryRepository _EntryRepository;
        private readonly IStopRepository _stopRepository;

        public EntryController(IEntryRepository EntryRepository, IStopRepository stopRepository)
        {
            _EntryRepository = EntryRepository;
            _stopRepository = stopRepository;
        }
        [Route("Entry")]
        [Route("Entry/Index")]
        public async Task<IActionResult> Index()
        {
            return View(await _EntryRepository.GetEntries());
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(EntryCreatorViewModel entryCreatorViewModel)
        {
            Entry entry = entryCreatorViewModel.Entry;
            entry.Stop = await _stopRepository.GetStop(entryCreatorViewModel.SelectedStopId);
            // entry.Driver = entryCreatorViewModel.Driver;
            entry.Bus = entryCreatorViewModel.Bus;
            entry.Loop = entryCreatorViewModel.Loop;
            entry.Timestamp = DateTime.Now;

            if (ModelState.IsValid)
            {
                await _EntryRepository.AddEntry(entry);
            }

            return RedirectToAction("EntryCreator", "Driver", new { BusId = entry.Bus.Id, LoopId = entry.Loop.Id });
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