using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using MVC.Controllers;
using MVC.Models;
using MVC.Repositories;
using MVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Tests;

public class UserControllerTests
{
    private ILogger<UserController> GetLogger()
    {
        var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        return loggerFactory.CreateLogger<UserController>();
    }

    [Fact]
    public async Task TestRegister()
    {

        var userController = new UserController(new FakeUserManager(), new FakeSignInManager(), GetLogger()) ;

        var result = userController.Register() as ViewResult;

        Assert.NotNull(result);
    }

    [Fact]
    public async Task TestLogin()
    {

        var userController = new UserController(new FakeUserManager(), new FakeSignInManager(), GetLogger());

        var result = userController.Login() as ViewResult;

        Assert.NotNull(result);
    }

    [Fact]
    public void TestLoginWithLoginViewModel()
    {
        var userController = new UserController(new FakeUserManager(), new FakeSignInManager(), GetLogger());
        LoginViewModel loginViewModel = new LoginViewModel();
        loginViewModel.username= "username";
        loginViewModel.password= "password";
        loginViewModel.rememberMe= true;
        var result = userController.Login(loginViewModel);

        Assert.NotNull(result);
    }
}
