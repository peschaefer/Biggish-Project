using Microsoft.EntityFrameworkCore;
using MVC.Models;
using MVC.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

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

// Add users
builder.Services.AddDefaultIdentity<Driver>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<BigishProjContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ManagerOnly", policy =>
        policy.RequireClaim("IsManager", "true"));
    options.AddPolicy("ActiveOnly", policy =>
        policy.RequireClaim("IsActive", "true"));
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/User/Login";
});


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