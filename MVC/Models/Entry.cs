namespace MVC.Models;

public class Entry
{
    public int Id { get; set; }
    public DateTime Timestamp { get; set; }
    public int Boarded { get; set; }
    public int LeftBehind { get; set; }
    public Driver Driver { get; set; }
    public Bus Bus { get; set; }
    public Loop Loop { get; set; }
    public Stop Stop { get; set; }
    public Route Route { get; set; }
}