using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Tests;

public class LoopRepoTests
{
    Loop loop1 = new Loop()
    {
        Id = 1,
        Name = "Test"
    };


    [Fact]
    public async void TestAddLoop()
    {
        var connection = new SqliteConnection("DataSource=:memory:");

        var option = new DbContextOptionsBuilder<BigishProjContext>()
            .UseSqlite(connection)
            .Options;

        var context = new BigishProjContext(option);

        LoopRepository loopRepository = new LoopRepository(context);
        loopRepository.AddLoop(loop1);
        var retrievedEntry = context.Loops.FindAsync(1);

        Assert.Equal(loop1.Id, retrievedEntry.Result.Id);
        Assert.Equal(loop1.Name, retrievedEntry.Result.Name);

        var deletion = loopRepository.DeleteLoop(1);
    }

    [Fact]
    public async void TestGetLoop()
    {
        var connection = new SqliteConnection("DataSource=:memory:");

        var option = new DbContextOptionsBuilder<BigishProjContext>()
            .UseSqlite(connection)
            .Options;

        var context = new BigishProjContext(option);

        LoopRepository loopRepository = new LoopRepository(context);
        loopRepository.AddLoop(loop1);
        var retrievedEntry = loopRepository.GetLoop(1);

        Assert.Equal(loop1.Id, retrievedEntry.Result.Id);
        Assert.Equal(loop1.Name, retrievedEntry.Result.Name);

        var deletion = loopRepository.DeleteLoop(1);
    }

    [Fact]
    public async void TestUpdateLoop()
    {
        var connection = new SqliteConnection("DataSource=:memory:");

        var option = new DbContextOptionsBuilder<BigishProjContext>()
            .UseSqlite(connection)
            .Options;

        var context = new BigishProjContext(option);

        LoopRepository loopRepository = new LoopRepository(context);

        loopRepository.AddLoop(loop1);

        Loop replacement = new Loop()
        {
            Id = 1,
            Name = "New Name"
        };

        loopRepository.UpdateLoop(replacement);

        var retrievedEntry = loopRepository.GetLoop(1);

        Assert.Equal(replacement.Id, retrievedEntry.Result.Id);
        Assert.Equal(replacement.Name, retrievedEntry.Result.Name);

        var deletion = loopRepository.DeleteLoop(1);
    }

    [Fact]
    public async void TestDeleteEntry()
    {
        Assert.True(false);
    }
}