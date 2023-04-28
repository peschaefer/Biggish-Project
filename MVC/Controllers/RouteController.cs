using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Controllers
{
    public class RouteController : Controller
    {
        private readonly IRouteRepository _routeRepository;

        public RouteController(IRouteRepository routeRepository)
        {
            _routeRepository = routeRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<Models.Route>>> GetAllRoutes()
        {
            var routes = await _routeRepository.GetRoutes();
            return Ok(routes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Route>> GetRoute(int id)
        {
            var route = await _routeRepository.GetRoute(id);
            if (route == null)
            {
                return NotFound();
            }

            return Ok(route);
        }

        [HttpPost]
        public async Task<ActionResult<Models.Route>> CreateRoute([FromBody] Models.Route route)
        {
            try
            {
                var createdRoute = await _routeRepository.AddRoute(route);
                return Ok(createdRoute);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoute(int id, [FromBody] Models.Route route)
        {
            if (id != route.Id)
            {
                return BadRequest("The ID specified in the route object does not match the route ID in the request URL.");
            }

            try
            {
                await _routeRepository.UpdateRoute(route);
                return Ok(route);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRoutes([FromBody] int[] ids)
        {
            try
            {
                var deletedRoutes = await _routeRepository.DeleteRoutes(ids);
                return Ok(deletedRoutes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
