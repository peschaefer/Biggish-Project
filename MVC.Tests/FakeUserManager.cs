using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC.Tests;
public class FakeUserManager : UserManager<Driver>
{
    public FakeUserManager()
        : base(new Mock<IUserStore<Driver>>().Object,
              new Mock<IOptions<IdentityOptions>>().Object,
              new Mock<IPasswordHasher<Driver>>().Object,
              new IUserValidator<Driver>[0],
              new IPasswordValidator<Driver>[0],
              new Mock<ILookupNormalizer>().Object,
              new Mock<IdentityErrorDescriber>().Object,
              new Mock<IServiceProvider>().Object,
              new Mock<ILogger<UserManager<Driver>>>().Object)
    { }
}

