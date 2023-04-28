using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Repositories;
using MVC.ViewModels;
using MVC.Models;

namespace MVC.Controllers;

public class DriverController: Controller
{
    private readonly ILoopRepository _loopRepository;
    private readonly IBusRepository _busRepository;
    private readonly IEntryRepository _entryRepository;
    
    public DriverController(ILoopRepository loopRepository, IBusRepository busRepository, IEntryRepository entryRepository)
    {
        _loopRepository = loopRepository;
        _busRepository = busRepository;
        _entryRepository = entryRepository;
    }

    public async Task<IActionResult> SelectBusLoop()
    {
        ViewData["BusId"] = new SelectList(await _busRepository.GetBuses(), "Id", "BusNumber");
        ViewData["LoopId"] = new SelectList(await _loopRepository.GetLoops(), "Id", "Name");

        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> StartDriving(int BusId, int LoopId)
    {
        Bus selectedBus = await _busRepository.GetBus(BusId);
        Loop selectedLoop = await _loopRepository.GetLoop(LoopId);
        return RedirectToAction("EntryCreator", new { BusId = selectedBus.Id, LoopId = selectedLoop.Id });
    }

    public async Task<IActionResult> EntryCreator(int BusId, int LoopId)
        {
            Bus selectedBus = await _busRepository.GetBus(BusId);
            Loop selectedLoop = await _loopRepository.GetLoop(LoopId);

            EntryCreatorViewModel entryCreatorViewModel = new EntryCreatorViewModel
            {
                Bus = selectedBus,
                Loop = selectedLoop
            };

            return View(entryCreatorViewModel);
        }
}