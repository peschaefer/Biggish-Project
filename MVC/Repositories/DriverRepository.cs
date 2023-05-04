using Microsoft.EntityFrameworkCore;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC.Repositories
{
    public interface IDriverRepository
    {
        Task<List<Driver>> GetDrivers();
        Task<Driver> GetDriver(string id);
        Task<string> AddDriver(Driver driver);
        Task<Driver> UpdateDriver(Driver driver);
        Task<List<Driver>> DeleteDrivers(string[] ids);
    }

    public class DriverRepository : IDriverRepository
    {
        private readonly BigishProjContext _context;

        public DriverRepository(BigishProjContext context)
        {
            _context = context;
        }

        public async Task<List<Driver>> GetDrivers()
        {
            return await _context.Drivers.ToListAsync();
        }

        public async Task<Driver> GetDriver(string id)
        {
            return await _context.Drivers.FindAsync(id);
        }

        public async Task<string> AddDriver(Driver driver)
        {
            _context.Drivers.Add(driver);
            await _context.SaveChangesAsync();
            return driver.Id;
        }

        public async Task<Driver> UpdateDriver(Driver driver)
        {
            var foundDriver = await _context.Drivers.FindAsync(driver.Id);
            if (foundDriver == null)
            {
                throw new Exception("Driver not found");
            }

            // Update the found driver with the new first and last names
            foundDriver.FirstName = driver.FirstName;
            foundDriver.LastName = driver.LastName;

            // Mark the found driver as modified
            _context.Entry(foundDriver).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return foundDriver;
        }

        public async Task<List<Driver>> DeleteDrivers(string[] ids)
        {
            var driversToDelete = new List<Driver>();

            foreach (var id in ids)
            {
                var foundDriver = await _context.Drivers.FindAsync(id);
                if (foundDriver == null)
                {
                    throw new Exception($"Driver with ID {id} not found");
                }
                driversToDelete.Add(foundDriver);
            }

            _context.Drivers.RemoveRange(driversToDelete);
            await _context.SaveChangesAsync();
            return driversToDelete;
        }
    }
}
