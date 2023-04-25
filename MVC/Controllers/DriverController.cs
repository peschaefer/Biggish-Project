using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVC.Repositories;

namespace MVC.Controllers;

[Authorize(Roles = "Driver")]
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



}