using MVC.Models;

namespace MVC.ViewModels
{
    public class MapViewModel
    {
        public List<StopWithPassengerCount> Stops { get; set; }
    }

    public class StopWithPassengerCount : Stop
    {
        public int Passengers { get; set; }
    }
}
