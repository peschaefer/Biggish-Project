using Microsoft.EntityFrameworkCore;
using MVC.Models;


namespace MVC.Repositories
{
    public interface ILoopRepository
    {
        Task<List<Loop>> GetLoops();
        Task<Loop> GetLoop(int id);
        Task<int> AddLoop(Loop loop);
        Task<Loop> UpdateLoop(Loop loop);
        Task<Loop> DeleteLoop(int id);
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
            return await _context.Loops.ToListAsync();
        }

        public async Task<Loop> GetLoop(int id)
        {
            return await _context.Loops.FindAsync(id);
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

        public async Task<Loop> DeleteLoop(int id)
        {
            var foundLoop = await _context.Loops.FindAsync(id);
            if (foundLoop == null)
            {
                throw new Exception("Loop not found");
            }

            _context.Loops.Remove(foundLoop);
            await _context.SaveChangesAsync();
            return foundLoop;
        }
    }
}