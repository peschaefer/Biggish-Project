using Microsoft.EntityFrameworkCore;
using MVC.Models;
using MVC.Repositories;

namespace MVC.Tests;

public class LoopRepoTests
{
    private BigishProjContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<BigishProjContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb-{Guid.NewGuid()}")
            .Options;

        return new BigishProjContext(options);
    }

    [Fact]
    public async Task TestAddAndGetLoop()
    {
        var dbContext = GetDbContext();
        var repository = new LoopRepository(dbContext);
        var newLoop = new Loop { Name = "Test Loop" };

        var loopId = await repository.AddLoop(newLoop);
        var addedLoop = await repository.GetLoop(loopId);

        Assert.NotNull(addedLoop);
        Assert.Equal("Test Loop", addedLoop.Name);
    }

    [Fact]
    public async Task TestGetLoops()
    {
        var dbContext = GetDbContext();
        var repository = new LoopRepository(dbContext);
        var loop1 = new Loop { Name = "Loop 1" };
        var loop2 = new Loop { Name = "Loop 2" };

        await repository.AddLoop(loop1);
        await repository.AddLoop(loop2);

        var loops = await repository.GetLoops();

        Assert.NotNull(loops);
        Assert.Equal(2, loops.Count);
        Assert.Contains(loops, loop => loop.Name == "Loop 1");
        Assert.Contains(loops, loop => loop.Name == "Loop 2");
    }

    [Fact]
    public async Task TestUpdateLoop()
    {
        var dbContext = GetDbContext();
        var repository = new LoopRepository(dbContext);
        var newLoop = new Loop { Name = "Test Loop" };
        var loopId = await repository.AddLoop(newLoop);
        newLoop.Id = loopId;
        newLoop.Name = "Updated Loop";

        var updatedLoop = await repository.UpdateLoop(newLoop);

        Assert.NotNull(updatedLoop);
        Assert.Equal("Updated Loop", updatedLoop.Name);
    }

    [Fact]
    public async Task TestDeleteLoops()
    {
        var dbContext = GetDbContext();
        var repository = new LoopRepository(dbContext);
        var loop1 = new Loop { Name = "Loop 1" };
        var loop2 = new Loop { Name = "Loop 2" };
        var loop1Id = await repository.AddLoop(loop1);
        var loop2Id = await repository.AddLoop(loop2);

        await repository.DeleteLoops(new int[] { loop1Id, loop2Id });
        var remainingLoops = await repository.GetLoops();

        Assert.Empty(remainingLoops);
    }
}