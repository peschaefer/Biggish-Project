using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using MVC.ViewModels;

namespace MVC.Controllers;

public class UserController : Controller
{
    private readonly UserManager<Driver> _userManager;
    private readonly SignInManager<Driver> _signInManager;
    
    public UserController(UserManager<Driver> userManager, SignInManager<Driver> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public IActionResult Login()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.username, model.password, model.rememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        return View(model);
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new Driver { UserName = model.FirstName, FirstName = model.FirstName, LastName = model.LastName, IsActive = false};

            bool managerExists = await _userManager.Users.AnyAsync(u => u.IsManager);
            if (!managerExists)
            {
                user.IsManager = true;
                user.IsActive = true;
            }
            else
            {
                user.IsManager = false;
                user.IsActive = false;
            }
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (user.IsManager)
                {
                    await _userManager.AddClaimAsync(user, new Claim("IsManager", "true"));
                }
                
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        return View(model);
    }

}