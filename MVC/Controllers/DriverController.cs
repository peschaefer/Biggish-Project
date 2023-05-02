using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Repositories;
using MVC.ViewModels;
using MVC.Models;

namespace MVC.Controllers;

[Authorize]
public class DriverController : Controller
{
    private readonly ILoopRepository _loopRepository;
    private readonly IBusRepository _busRepository;
    private readonly IEntryRepository _entryRepository;
    private readonly IRouteRepository _routeRepository;
    private readonly ILogger<DriverController> _logger;

    public DriverController(ILoopRepository loopRepository, IBusRepository busRepository,
        IEntryRepository entryRepository, IRouteRepository routeRepository, ILogger<DriverController> logger)
    {
        _loopRepository = loopRepository;
        _busRepository = busRepository;
        _entryRepository = entryRepository;
        _routeRepository = routeRepository;
        _logger = logger;
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
        _logger.LogInformation("Bus id {id} started driving on loop {loop} at {time}", BusId, selectedLoop.Name ,DateTime.Now);
        return RedirectToAction("EntryCreator", new { BusId = selectedBus.Id, LoopId = selectedLoop.Id });
    }

    public async Task<IActionResult> EntryCreator(int BusId, int LoopId)
    {
        Bus selectedBus = await _busRepository.GetBus(BusId);
        Loop selectedLoop = await _loopRepository.GetLoop(LoopId);
        await _routeRepository.GetRoutes();

        EntryCreatorViewModel entryCreatorViewModel = new EntryCreatorViewModel
        {
            Bus = selectedBus,
            Loop = selectedLoop,
            Entry = new Entry(),
            BusId = BusId,
            LoopId = LoopId,
        };

        return View(entryCreatorViewModel);
    }
}