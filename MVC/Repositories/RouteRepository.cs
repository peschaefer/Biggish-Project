using Microsoft.EntityFrameworkCore;
using MVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Repositories
{
    public interface IRouteRepository
    {
        Task<List<Models.Route>> GetRoutes();
        Task<Models.Route> GetRoute(int id);
        Task<int> AddRoute(Models.Route route);
        Task<Models.Route> UpdateRoute(Models.Route route);
        Task<List<Models.Route>> DeleteRoutes(int[] ids);
    }

    public class RouteRepository : IRouteRepository
    {
        private readonly BigishProjContext _context;

        public RouteRepository(BigishProjContext context)
        {
            _context = context;
        }

        public async Task<List<Models.Route>> GetRoutes()
        {
            return await _context.Routes
                .Include(r => r.Stop)
                .Include(r => r.Loop)
                .ToListAsync();
        }

        public async Task<Models.Route> GetRoute(int id)
        {
            return await _context.Routes
                .Include(r => r.Stop)
                .Include(r => r.Loop)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<int> AddRoute(Models.Route route)
        {
            _context.Routes.Add(route);
            await _context.SaveChangesAsync();
            return route.Id;
        }

        public async Task<Models.Route> UpdateRoute(Models.Route route)
        {
            var foundRoute = await _context.Routes.FindAsync(route.Id);
            if (foundRoute == null)
            {
                throw new Exception("Route not found");
            }

            // Update the found route with the new properties
            foundRoute.Order = route.Order;
            foundRoute.Stop = route.Stop;
            foundRoute.Loop = route.Loop;

            // Mark the found route as modified
            _context.Entry(foundRoute).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return foundRoute;
        }

        public async Task<List<Models.Route>> DeleteRoutes(int[] ids)
        {
            var routesToDelete = new List<Models.Route>();

            foreach (var id in ids)
            {
                var foundRoute = await _context.Routes.FindAsync(id);
                if (foundRoute == null)
                {
                    throw new Exception($"Route with ID {id} not found");
                }
                routesToDelete.Add(foundRoute);
            }

            _context.Routes.RemoveRange(routesToDelete);
            await _context.SaveChangesAsync();
            return routesToDelete;
        }
    }
}
