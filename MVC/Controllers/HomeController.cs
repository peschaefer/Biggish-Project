﻿using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;
using MVC.ViewModels;

namespace MVC.Controllers;
[Authorize]
public class HomeController : Controller
{

    private readonly IDriverRepository _driverRepository;
    private readonly UserManager<Driver> _userManager;


    public HomeController(ILogger<HomeController> logger, IDriverRepository driverRepository, UserManager<Driver> userManager)
    {
        _driverRepository = driverRepository;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    public async Task<IActionResult> Dashboard()
    {
        var drivers = await _driverRepository.GetDrivers();

        var activeUserIds = (await _userManager.GetUsersForClaimAsync(new Claim("IsActive", "true"))).Select(u => u.Id);

        drivers = drivers.Where(d => !d.IsManager && !activeUserIds.Contains(d.Id)).ToList();

        var viewModel = new DashboardViewModel
        {
            Drivers = drivers
        };

        return View(viewModel);
    }
}
