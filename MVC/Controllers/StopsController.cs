using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    public class StopsController : Controller
    {
        private readonly IStopRepository _stopRepository;

        public StopsController(IStopRepository stopRepository)
        {
            _stopRepository = stopRepository;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,Latitude,Longitude")] Stop stop)
        {
            if (ModelState.IsValid)
            {
                await _stopRepository.AddStop(stop);
                return RedirectToAction("Index");
            }
            return View(stop);
        }
    }
}

