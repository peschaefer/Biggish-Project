namespace MVC.Models;

public class Loop
{
    public int Id { get; set; }
    public string Name { get; set; }
    //THIS HAS BEEN CHANGED, RUN DB UPDATE
    public List<Route> Routes { get; set; }
}