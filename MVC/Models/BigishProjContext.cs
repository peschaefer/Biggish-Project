using Microsoft.EntityFrameworkCore;

namespace MVC.Models;

public class BigishProjContext: DbContext
{
    public DbSet<Bus> Buses { get; set; }
    public DbSet<Driver> Drivers { get; set; }
    public DbSet<Entry> Entries { get; set; }
    public DbSet<Loop> Loops { get; set; }
    public DbSet<Route> Routes { get; set; }
    public DbSet<Stop> Stops { get; set; }
    
    public string DbPath { get; }

    public BigishProjContext(DbContextOptions<BigishProjContext> options) : base(options)
    {
        
    }
}