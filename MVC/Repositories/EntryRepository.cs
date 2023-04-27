using Microsoft.EntityFrameworkCore;
using MVC.Models;


namespace MVC.Repositories
{
    public interface IEntryRepository
    {
        Task AddEntryAsync(Entry entry);
        Task SaveChangesAsync();
        Task<List<Entry>> GetEntries();
        Task<Entry> GetEntry(int id);
        Task<int> AddEntry(Entry entry);
        Task<Entry> UpdateEntry(Entry entry);
        Task<Entry> DeleteEntry(int id);
    }

    public class EntryRepository : IEntryRepository
    {
        private readonly BigishProjContext _context;

        public EntryRepository(BigishProjContext context)
        {
            _context = context;
        }

        public async Task<List<Entry>> GetEntries()
        {
            return await _context.Entries.ToListAsync();
        }

        public async Task<Entry> GetEntry(int id)
        {
            return await _context.Entries.FindAsync(id);
        }

        public async Task<int> AddEntry(Entry entry)
        {
            _context.Entries.Add(entry);
            await _context.SaveChangesAsync();
            return entry.Id;
        }

        public async Task<Entry> UpdateEntry(Entry entry)
        {
            var foundEntry = await _context.Entries.FindAsync(entry.Id);
            if (foundEntry == null)
            {
                throw new Exception("Entry not found");
            }

            _context.Entries.Update(entry);
            await _context.SaveChangesAsync();
            return entry;
        }

        public async Task<Entry> DeleteEntry(int id)
        {
            var foundEntry = await _context.Entries.FindAsync(id);
            if (foundEntry == null)
            {
                throw new Exception("Entry not found");
            }

            _context.Entries.Remove(foundEntry);
            await _context.SaveChangesAsync();
            return foundEntry;
        }
        public async Task AddEntryAsync(Entry entry)
        {
            _context.Entries.Add(entry);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}