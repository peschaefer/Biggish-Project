using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using MVC.Models;

namespace MVC.ViewModels;
public class CreateLoopViewModel
{
    public Loop Loop { get; set; }
    [ValidateNever]
    public List<Stop> Stops { get; set; }
    public List<RouteViewModel> Routes { get; set; } = new List<RouteViewModel>();

}