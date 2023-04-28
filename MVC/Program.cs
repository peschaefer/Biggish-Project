using Microsoft.EntityFrameworkCore;
using MVC.Models;
using MVC.Repositories;

// using var db = new BigishProjContext();
//
// Console.WriteLine($"Database path: {db.DbPath}");

var builder = WebApplication.CreateBuilder(args);
var folder = Environment.SpecialFolder.LocalApplicationData;
var path = Environment.GetFolderPath(folder);
var dbPath = Path.Join(path, "BigishProj.db");
var connectionString = builder.Configuration.GetConnectionString("BigishProj") ?? "Data Source=BigishProj.db";

//builder.Services.AddDbContext<BigishProjContext>(options => options.UseSqlite(dbPath));
builder.Services.AddDbContext<BigishProjContext>(options => options.UseInMemoryDatabase("biggishproject"));


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IBusRepository, BusRepository>();
builder.Services.AddScoped<IEntryRepository, EntryRepository>();
builder.Services.AddScoped<ILoopRepository, LoopRepository>();
builder.Services.AddScoped<IRouteRepository, RouteRepository>();
builder.Services.AddScoped<IStopRepository, StopRepository>();
builder.Services.AddScoped<IDriverRepository, DriverRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();
