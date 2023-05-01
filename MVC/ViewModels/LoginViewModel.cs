using System.ComponentModel.DataAnnotations;

namespace MVC.ViewModels;

public class LoginViewModel
{
    [Required]
    public string username { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string password { get; set; }
    
    [Display(Name = "Remember Me")]
    public bool rememberMe { get; set; }
}