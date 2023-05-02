using MVC.Models;

namespace MVC.ViewModels;


public class LoopIndexViewModel
{
    public List<Loop> Loops { get; set; }
    public CreateLoopViewModel CreateLoopViewModel { get; set; }
    public MapViewModel MapViewModel { get; set; }
    public Dictionary<int, List<Stop>> LoopStops { get; set; }
}
