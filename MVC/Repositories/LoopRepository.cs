using Microsoft.EntityFrameworkCore;
using MVC.Models;
using Route = MVC.Models.Route;

namespace MVC.Repositories
{
    public interface ILoopRepository
    {
        Task<List<Loop>> GetLoops();
        Task<Loop> GetLoop(int id);
        Task<int> AddLoop(Loop loop);
        Task<Loop> UpdateLoop(Loop loop);
        Task<List<Loop>> DeleteLoops(int[] ids);
        Task<Loop> AddLoopWithRoutes(Loop loop, List<Route> routes);
    }

    public class LoopRepository : ILoopRepository
    {
        private readonly BigishProjContext _context;

        public LoopRepository(BigishProjContext context)
        {
            _context = context;
        }

        public async Task<List<Loop>> GetLoops()
        {
            return await _context.Loops.Include(l => l.Routes).ThenInclude(r => r.Stop).ToListAsync();
        }

        public async Task<Loop> GetLoop(int id)
        {
            return await _context.Loops.Include(l => l.Routes).ThenInclude(r => r.Stop).SingleOrDefaultAsync(l => l.Id == id);
        }

        public async Task<int> AddLoop(Loop loop)
        {
            _context.Loops.Add(loop);
            await _context.SaveChangesAsync();
            return loop.Id;
        }

        public async Task<Loop> UpdateLoop(Loop loop)
        {
            var foundLoop = await _context.Loops.FindAsync(loop.Id);
            if (foundLoop == null)
            {
                throw new Exception("Loop not found");
            }

            _context.Loops.Update(loop);
            await _context.SaveChangesAsync();
            return loop;
        }
        
        
        public async Task<List<Loop>> DeleteLoops(int[] ids)
        {
            var loopsToDelete = new List<Loop>();

            foreach (var id in ids)
            {
                var foundLoop = await _context.Loops.FindAsync(id);
                if (foundLoop == null)
                {
                    throw new Exception($"Bus with ID {id} not found");
                }
                loopsToDelete.Add(foundLoop);
            }

            _context.Loops.RemoveRange(loopsToDelete);
            await _context.SaveChangesAsync();
            return loopsToDelete;
        }
        
        public async Task<Loop> AddLoopWithRoutes(Loop loop, List<Route> routes)
        {
            loop.Routes = routes;
            _context.Loops.Add(loop);
            await _context.SaveChangesAsync();
            return loop;
        }

    }
}
