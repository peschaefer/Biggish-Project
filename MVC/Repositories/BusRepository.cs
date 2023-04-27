using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Repositories;

public interface IBusRepository
{
    Task<List<Bus>> GetBuses();
    Task<Bus> GetBus(int id);
    Task<int> AddBus(Bus bus);
    Task<Bus> UpdateBus(Bus bus);
    Task<List<Bus>> DeleteBuses(int[] ids);
}

public class BusRepository : IBusRepository
{
    private readonly BigishProjContext _context;

    public BusRepository(BigishProjContext context)
    {
        _context = context;
    }

    public async Task<List<Bus>> GetBuses()
    {
        return await _context.Buses.ToListAsync();
    }

    public async Task<Bus> GetBus(int id)
    {
        return await _context.Buses.FindAsync(id);
    }

    public async Task<int> AddBus(Bus bus)
    {
        _context.Buses.Add(bus);
        await _context.SaveChangesAsync();
        return bus.Id;
    }

    public async Task<Bus> UpdateBus(Bus bus)
    {
        var foundBus = await _context.Buses.FindAsync(bus.Id);
        if (foundBus == null)
        {
            throw new Exception("Bus not found");
        }

        // Update the found bus with the new bus number
        foundBus.BusNumber = bus.BusNumber;

        // Mark the found bus as modified
        _context.Entry(foundBus).State = EntityState.Modified;

        await _context.SaveChangesAsync();
        return foundBus;
    }

    public async Task<List<Bus>> DeleteBuses(int[] ids)
    {
        var busesToDelete = new List<Bus>();

        foreach (var id in ids)
        {
            var foundBus = await _context.Buses.FindAsync(id);
            if (foundBus == null)
            {
                throw new Exception($"Bus with ID {id} not found");
            }
            busesToDelete.Add(foundBus);
        }

        _context.Buses.RemoveRange(busesToDelete);
        await _context.SaveChangesAsync();
        return busesToDelete;
    }
}