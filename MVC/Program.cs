using MVC.Models;
using MVC.Repositories;

// using var db = new BigishProjContext();
//
// Console.WriteLine($"Database path: {db.DbPath}");

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("BigishProj") ?? "Data Source=BigishProj.db";

builder.Services.AddSqlite<BigishProjContext>(connectionString);


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
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Bus}/{action=Index}/{id?}");

app.Run();
