using MVC.Models;

namespace MVC.ViewModels;

public class EntryCreatorViewModel
{
    public Bus Bus { get; set; }
    public Loop Loop { get; set; }
    public Entry Entry { get; set; }
    public int SelectedStopId { get; set; }
}
