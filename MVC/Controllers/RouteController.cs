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
        private readonly ILogger<RouteController> _logger;

        public RouteController(] routeRepository, ILogger<RouteController> logger)
        {
            _routeRepository = routeRepository;
            _logger = logger;
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
                _logger.LogWarning("Route with id {id} not found at {time}", id, DateTime.Now);
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
                _logger.LogWarning("Route with id {id} created at {time}", route.Id, DateTime.Now);
                return Ok(createdRoute);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to create route with exception {exception} created at {time}", ex.Message, DateTime.Now);
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoute(int id, [FromBody] Models.Route route)
        {
            if (id != route.Id)
            {
                _logger.LogWarning("Route with id {id} does not exist.", id);
                return BadRequest("The ID specified in the route object does not match the route ID in the request URL.");
            }

            try
            {
                await _routeRepository.UpdateRoute(route);
                _logger.LogInformation("Updated route with id {id} at {time}", id, DateTime.Now);
                return Ok(route);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to update route with exception {exception} at {time}", ex.Message, DateTime.Now);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRoutes([FromBody] int[] ids)
        {
            try
            {
                var deletedRoutes = await _routeRepository.DeleteRoutes(ids);
                _logger.LogInformation("Deleted loops with ids {ids} at {time}", string.Join(", ", ids.Select(id => id.ToString())),DateTime.Now);
                return Ok(deletedRoutes);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to delete routes with exception {exception} at {time}", ex.Message, DateTime.Now);
                return BadRequest(ex.Message);
            }
        }
    }
}
