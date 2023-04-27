using Microsoft.EntityFrameworkCore;
using MVC.Models;
using Route = MVC.Models.Route;

namespace MVC.Repositories
{
    public interface IRouteRepository
    {

        Task<List<Route>> GetRoutes();
        Task<Route> GetRoute(int id);
        Task<int> AddRoute(Route route);
        Task<Route> UpdateRoute(Route route);
        Task<Route> DeleteRoute(int id);
    }

    public class RouteRepository : IRouteRepository
    {
        private readonly BigishProjContext _context;

        public RouteRepository(BigishProjContext context)
        {
            _context = context;
        }

        public async Task<List<Route>> GetRoutes()
        {
            return await _context.Routes.ToListAsync();

        }

        public async Task<Route> GetRoute(int id)
        {
            return await _context.Routes.FindAsync(id);
        }

        public async Task<int> AddRoute(Route route)
        {
            _context.Routes.Add(route);
            await _context.SaveChangesAsync();
            return route.Id;
        }

        public async Task<Route> UpateRoute(Route route)
        {
            var foundRoute = await _context.Routes.FindAsync(route.Id);
            if (foundRoute == null)
            {
                throw new Exception("Route not found");
            }

            _context.Routes.Update(route);
            await _context.SaveChangesAsync();
            return route;
        }

        public async Task<Route> DeleteRoute(int id)
        {
            var foundRoute = await _context.Routes.FindAsync(id);
            if (foundRoute == null)
            {
                throw new Exception("Route not found");
            }

            _context.Routes.Remove(foundRoute);
            await _context.SaveChangesAsync();
            return foundRoute;
        }

        public Task<Route> UpdateRoute(Route route)
        {
            throw new NotImplementedException();
        }

        Task<Route> IRouteRepository.DeleteRoute(int id)
        {
            throw new NotImplementedException();
        }
    }
}