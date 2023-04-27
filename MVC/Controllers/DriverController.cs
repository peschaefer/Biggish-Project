using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Repositories;
using MVC.Models;

namespace MVC.Controllers;

public class DriverController : Controller
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
    public IActionResult StartDriving(int BusId, int LoopId)
    {
        return RedirectToAction("EnterBusStopInfo", new { BusId = BusId, LoopId = LoopId });
    }

    [HttpPost]
    public async Task<IActionResult> ProcessEntry(Entry entry)
    {
        entry.Timestamp = DateTime.UtcNow;

        await _entryRepository.AddEntryAsync(entry);
        await _entryRepository.SaveChangesAsync();

        return RedirectToAction("SelectBusLoop");
    }
}