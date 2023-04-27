using Microsoft.EntityFrameworkCore;
using MVC.Models;


namespace MVC.Repositories
{
    public interface IStopRepository
    {
        Task<List<Stop>> GetStops();
        Task<Stop> GetStop(int id);
        Task<int> AddStop(Stop Stop);
        Task<Stop> UpdateStop(Stop Stop);
        Task<Stop> DeleteStop(int id);
    }

    public class StopRepository : IStopRepository
    {
        private readonly BigishProjContext _context;

        public StopRepository(BigishProjContext context)
        {
            _context = context;
        }

        public async Task<List<Stop>> GetStops()
        {
            return await _context.Stops.ToListAsync();
        }

        public async Task<Stop> GetStop(int id)
        {
            return await _context.Stops.FindAsync(id);
        }

        public async Task<int> AddStop(Stop stop)
        {
            _context.Stops.Add(stop);
            await _context.SaveChangesAsync();
            return stop.Id;
        }

        public async Task<Stop> UpdateStop(Stop stop)
        {
            var foundStop = await _context.Stops.FindAsync(stop.Id);
            if (foundStop == null)
            {
                throw new Exception("Stop not found");
            }

            _context.Stops.Update(stop);
            await _context.SaveChangesAsync();
            return stop;
        }

        public async Task<Stop> DeleteStop(int id)
        {
            var foundStop = await _context.Stops.FindAsync(id);
            if (foundStop == null)
            {
                throw new Exception("Stop not found");
            }

            _context.Stops.Remove(foundStop);
            await _context.SaveChangesAsync();
            return foundStop;
        }
    }
}