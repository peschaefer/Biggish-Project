using Microsoft.AspNetCore.Identity;

namespace MVC.Models;

public class Driver : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsManager { get; set; }
    public bool IsActive { get; set; }
}