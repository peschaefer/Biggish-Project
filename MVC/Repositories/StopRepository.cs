using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Repositories
{
    public interface IStopRepository
    {
        Task<List<Stop>> GetStops();
        Task<Stop> GetStop(int id);
        Task<int> AddStop(Stop stop);
        Task<Stop> UpdateStop(Stop stop);
        Task<List<Stop>> DeleteStops(int[] ids);
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
            Console.WriteLine("ID: " + id);
            var stops = await _context.Stops.ToListAsync();
            foreach (var stop in stops)
            {
                Console.WriteLine(stop.Id);
            }
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

            // Update the found stop with the new properties
            foundStop.Name = stop.Name;
            foundStop.Latitude = stop.Latitude;
            foundStop.Longitude = stop.Longitude;

            // Mark the found stop as modified
            _context.Entry(foundStop).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return foundStop;
        }

        public async Task<List<Stop>> DeleteStops(int[] ids)
        {
            var stopsToDelete = new List<Stop>();

            foreach (var id in ids)
            {
                var foundStop = await _context.Stops.FindAsync(id);
                if (foundStop == null)
                {
                    throw new Exception($"Stop with ID {id} not found");
                }
                stopsToDelete.Add(foundStop);
            }

            _context.Stops.RemoveRange(stopsToDelete);
            await _context.SaveChangesAsync();
            return stopsToDelete;
        }
    }
}
