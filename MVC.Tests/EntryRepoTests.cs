using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Tests;

public class EntryRepoTests
{

    Entry entry1 = new Entry()
    {
        Id = 1,
        Timestamp = DateTime.Now,
        Boarded = 0,
        LeftBehind = 0
    };


    [Fact]
    public async void TestAddEntry()
    {
        var connection = new SqliteConnection("DataSource=:memory:");

        var option = new DbContextOptionsBuilder<BigishProjContext>()
            .UseSqlite(connection)
            .Options;

        var context = new BigishProjContext(option);

        EntryRepository entryRepository = new EntryRepository(context);
        entryRepository.AddEntry(entry1);
        var retrievedEntry = context.Entries.FindAsync(1);

        Assert.Equal(entry1.Id, retrievedEntry.Result.Id);
        Assert.Equal(entry1.Timestamp, retrievedEntry.Result.Timestamp);
        Assert.Equal(entry1.Boarded, retrievedEntry.Result.Boarded);
        Assert.Equal(entry1.LeftBehind, retrievedEntry.Result.LeftBehind);

        var deletion = entryRepository.DeleteEntry(1);
    }

    [Fact]
    public async void TestGetEntry()
    {
        var connection = new SqliteConnection("DataSource=:memory:");

        var option = new DbContextOptionsBuilder<BigishProjContext>()
            .UseSqlite(connection)
            .Options;

        var context = new BigishProjContext(option);

        EntryRepository entryRepository = new EntryRepository(context);
        entryRepository.AddEntry(entry1);
        var retrievedEntry = entryRepository.GetEntry(1);

        Assert.Equal(entry1.Id, retrievedEntry.Result.Id);
        Assert.Equal(entry1.Timestamp, retrievedEntry.Result.Timestamp);
        Assert.Equal(entry1.Boarded, retrievedEntry.Result.Boarded);
        Assert.Equal(entry1.LeftBehind, retrievedEntry.Result.LeftBehind);

        var deletion = entryRepository.DeleteEntry(1);
    }

    [Fact]
    public async void TestUpdateEntry()
    {
        var connection = new SqliteConnection("DataSource=:memory:");

        var option = new DbContextOptionsBuilder<BigishProjContext>()
            .UseSqlite(connection)
            .Options;

        var context = new BigishProjContext(option);

        EntryRepository entryRepository = new EntryRepository(context);

        entryRepository.AddEntry(entry1);

        Entry replacement = new Entry()
        {
            Id = 1,
            Timestamp = entry1.Timestamp,
            Boarded = 3,
            LeftBehind = 4
        };

        entryRepository.UpdateEntry(replacement);

        var retrievedEntry = entryRepository.GetEntry(1);

        Assert.Equal(replacement.Id, retrievedEntry.Result.Id);
        Assert.Equal(replacement.Timestamp, retrievedEntry.Result.Timestamp);
        Assert.Equal(replacement.Boarded, retrievedEntry.Result.Boarded);
        Assert.Equal(replacement.LeftBehind, retrievedEntry.Result.LeftBehind);

        var deletion = entryRepository.DeleteEntry(1);
    }

    [Fact]
    public async void TestDeleteEntry()
    {
        Assert.True(false);
    }
}