using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;
using MVC.ViewModels;

namespace MVC.Controllers
{
    [Authorize]
    public class EntryController : Controller
    {
        private readonly IEntryRepository _EntryRepository;
        private readonly IStopRepository _stopRepository;
        private readonly IBusRepository _busRepository;
        private readonly ILoopRepository _loopRepository;
        private readonly UserManager<Driver> _userManager;

        public EntryController(IEntryRepository EntryRepository, IStopRepository stopRepository, UserManager<Driver> userManager, IBusRepository busRepository, ILoopRepository loopRepository)
        {
            _EntryRepository = EntryRepository;
            _stopRepository = stopRepository;
            _userManager = userManager;
            _busRepository = busRepository;
            _loopRepository = loopRepository;
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
            entryCreatorViewModel.Bus = await _busRepository.GetBus(entryCreatorViewModel.BusId);
            entryCreatorViewModel.Loop = await _loopRepository.GetLoop(entryCreatorViewModel.LoopId);
            
            Entry entry = entryCreatorViewModel.Entry;
            entry.Stop = await _stopRepository.GetStop(entryCreatorViewModel.SelectedStopId);
            
            // entry.Driver = await _userManager.GetUserAsync(User);
            
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine($"userId: {userId}");
            Console.WriteLine(_userManager);
            var user = await _userManager.FindByIdAsync(userId);
            Console.WriteLine($"user: {user}");
            entry.Driver = await _userManager.FindByIdAsync(userId);


            entry.Bus = entryCreatorViewModel.Bus;
            entry.Loop = entryCreatorViewModel.Loop;
            entry.Timestamp = DateTime.Now;
            
            ModelState.Clear();
            TryValidateModel(entryCreatorViewModel.Entry);
            

            if (ModelState.IsValid)
            {
                Console.WriteLine("ModelState is valid");

                
                await _EntryRepository.AddEntry(entry);
            }
            else
            {
                foreach (var modelStateEntry in ModelState)
                {
                    var key = modelStateEntry.Key;
                    var errors = modelStateEntry.Value.Errors;
                    if (errors != null && errors.Count > 0)
                    {
                        Console.WriteLine($"Errors for {key}:");
                        foreach (var error in errors)
                        {
                            Console.WriteLine($"- {error.ErrorMessage}");
                        }
                    }
                }
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