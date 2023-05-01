using MVC.Models;

namespace MVC.ViewModels;

public class EntryCreatorViewModel
{
    public Bus Bus { get; set; }
    public Loop Loop { get; set; }
    public Entry Entry { get; set; } = new Entry();
    public int SelectedStopId { get; set; }
    public int BusId { get; set; }
    public int LoopId { get; set; }
}
