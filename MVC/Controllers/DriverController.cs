using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Repositories;
using MVC.ViewModels;
using MVC.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace MVC.Controllers;

[Authorize]
public class DriverController : Controller
{
    private readonly ILoopRepository _loopRepository;
    private readonly IBusRepository _busRepository;
    private readonly IRouteRepository _routeRepository;
    private readonly UserManager<Driver> _userManager;
    private readonly SignInManager<Driver> _signInManager;
    private readonly ILogger<DriverController> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DriverController(ILoopRepository loopRepository, IBusRepository busRepository,
        IHttpContextAccessor contextAccessor, IRouteRepository routeRepository, ILogger<DriverController> logger, UserManager<Driver> userManager, SignInManager<Driver> signInManager)
    {
        _loopRepository = loopRepository;
        _busRepository = busRepository;
        _httpContextAccessor = contextAccessor;
        _routeRepository = routeRepository;
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
    }
    
    public async Task<IActionResult> SelectBusLoop()
    {
        ViewData["BusId"] = new SelectList(await _busRepository.GetBuses(), "Id", "BusNumber");
        ViewData["LoopId"] = new SelectList(await _loopRepository.GetLoops(), "Id", "Name");
        ViewData["IsActive"] = User.HasClaim("IsActive", "true");
        return View();
    }

    [HttpPost]
    [Authorize(Policy = "ActiveOnly")]
    public async Task<IActionResult> StartDriving(int BusId, int LoopId)
    {
        Bus selectedBus = await _busRepository.GetBus(BusId);
        Loop selectedLoop = await _loopRepository.GetLoop(LoopId);
        _logger.LogInformation("Bus id {id} started driving on loop {loop} at {time}", BusId, selectedLoop.Name ,DateTime.Now);
        return RedirectToAction("EntryCreator", new { BusId = selectedBus.Id, LoopId = selectedLoop.Id });
    }

    [Authorize(Policy = "ActiveOnly")]

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

    [Authorize(Policy = "ManagerOnly")]
    public async Task<IActionResult> ApproveDriver(string userId)
    {
        _logger.LogInformation("Approve Driver called for user {id} at {time}.", userId ,DateTime.Now);
        // Check if user exists
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            // Handle the case where the user doesn't exist
            _logger.LogWarning("Approve Driver called on user that does not exist.", DateTime.Now);
            return NotFound();
        }

        // Add the approval claim to the user
        var result = await _userManager.AddClaimAsync(user, new Claim("IsActive", "true"));
        _logger.LogInformation("User with id {userId} approved at {time}.", userId, DateTime.Now);

        if (!result.Succeeded)
        {
            // Handle the case where the claim couldn't be added
            return BadRequest();
        }

        // If the claim was added successfully, update the user
        await _userManager.UpdateAsync(user);
        var identity = _httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity;
        
        if (identity != null)
        {
            identity.AddClaim(new Claim("IsActive", "true"));
            // Redirect to the Driver Index action method
            return RedirectToAction("Dashboard", "Home");
        }

        // Sign out the user
        await _signInManager.SignOutAsync();
        // Redirect to the Login page
        return RedirectToAction("Login", "User");
    }
}