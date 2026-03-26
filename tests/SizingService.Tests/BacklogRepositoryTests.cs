using SizingService.Models;
using SizingService.Repositories;
using Xunit;

namespace SizingService.Tests;

public class BacklogRepositoryTests
{
    private static InMemoryBacklogRepository CreateRepo() => new();

    [Fact]
    public void AddRanking_ReturnsNull_WhenItemNotFound()
    {
        var repo = CreateRepo();
        var ranking = new UserRanking("Alice", 5, Priority.Medium);

        var result = repo.AddRanking(Guid.NewGuid(), ranking);

        Assert.Null(result);
    }

    [Fact]
    public void AddRanking_AddsRankingToItem()
    {
        var repo = CreateRepo();
        var item = repo.Add(new BacklogItem { Title = "Test" });
        var ranking = new UserRanking("Alice", 5, Priority.Medium);

        var result = repo.AddRanking(item.Id, ranking);

        Assert.NotNull(result);
        Assert.Single(result.Rankings);
        Assert.Equal("Alice", result.Rankings[0].UserName);
        Assert.Equal(5, result.Rankings[0].StoryPoints);
        Assert.Equal(Priority.Medium, result.Rankings[0].Priority);
    }

    [Fact]
    public void AddRanking_OverwritesPreviousRankingFromSameUser()
    {
        var repo = CreateRepo();
        var item = repo.Add(new BacklogItem { Title = "Test" });
        repo.AddRanking(item.Id, new UserRanking("Alice", 3, Priority.Low));

        var result = repo.AddRanking(item.Id, new UserRanking("Alice", 8, Priority.High));

        Assert.NotNull(result);
        Assert.Single(result.Rankings);
        Assert.Equal(8, result.Rankings[0].StoryPoints);
        Assert.Equal(Priority.High, result.Rankings[0].Priority);
    }

    [Fact]
    public void AddRanking_AllowsMultipleDifferentUsers()
    {
        var repo = CreateRepo();
        var item = repo.Add(new BacklogItem { Title = "Test" });
        repo.AddRanking(item.Id, new UserRanking("Alice", 3, Priority.Low));

        var result = repo.AddRanking(item.Id, new UserRanking("Bob", 5, Priority.Medium));

        Assert.NotNull(result);
        Assert.Equal(2, result.Rankings.Count);
    }

    [Fact]
    public void AverageStoryPoints_IsNull_WhenNoRankings()
    {
        var item = new BacklogItem { Title = "Test" };

        Assert.Null(item.AverageStoryPoints);
    }

    [Fact]
    public void AverageStoryPoints_CalculatesCorrectly()
    {
        var item = new BacklogItem { Title = "Test" };
        item.Rankings.Add(new UserRanking("Alice", 3, Priority.Unset));
        item.Rankings.Add(new UserRanking("Bob", 5, Priority.Unset));
        item.Rankings.Add(new UserRanking("Carol", 8, Priority.Unset));

        Assert.Equal(16.0 / 3, item.AverageStoryPoints);
    }

    [Fact]
    public void AverageStoryPoints_IgnoresNullStoryPoints()
    {
        var item = new BacklogItem { Title = "Test" };
        item.Rankings.Add(new UserRanking("Alice", 4, Priority.Unset));
        item.Rankings.Add(new UserRanking("Bob", null, Priority.Unset));

        Assert.Equal(4.0, item.AverageStoryPoints);
    }

    [Fact]
    public void AveragePriority_IsNull_WhenNoRankings()
    {
        var item = new BacklogItem { Title = "Test" };

        Assert.Null(item.AveragePriority);
    }

    [Fact]
    public void AveragePriority_CalculatesCorrectly()
    {
        var item = new BacklogItem { Title = "Test" };
        item.Rankings.Add(new UserRanking("Alice", null, Priority.Low));
        item.Rankings.Add(new UserRanking("Bob", null, Priority.High));

        Assert.Equal(2.0, item.AveragePriority);
    }

    [Fact]
    public void AveragePriority_IgnoresUnsetPriority()
    {
        var item = new BacklogItem { Title = "Test" };
        item.Rankings.Add(new UserRanking("Alice", null, Priority.High));
        item.Rankings.Add(new UserRanking("Bob", null, Priority.Unset));

        Assert.Equal((double)Priority.High, item.AveragePriority);
    }
}
