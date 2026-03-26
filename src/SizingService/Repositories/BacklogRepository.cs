using SizingService.Models;
using System.Collections.Concurrent;

namespace SizingService.Repositories;

public interface IBacklogRepository
{
    IReadOnlyList<BacklogItem> GetAll();
    BacklogItem? GetById(Guid id);
    BacklogItem Add(BacklogItem item);
    BacklogItem? Update(Guid id, BacklogItem updated);
    BacklogItem? ApplySize(Guid id, SizeRequest request);
    BacklogItem? AddRanking(Guid id, UserRanking ranking);
    bool Delete(Guid id);
}

public class InMemoryBacklogRepository : IBacklogRepository
{
    private readonly ConcurrentDictionary<Guid, BacklogItem> _items = new();

    public InMemoryBacklogRepository()
    {
        var seed = new[]
        {
            new BacklogItem { Title = "Set up CI/CD pipeline", Description = "Configure GitHub Actions for build and deploy." },
            new BacklogItem { Title = "User authentication", Description = "Implement login and registration with JWT." },
            new BacklogItem { Title = "Backlog import from CSV", Description = "Allow teams to bulk-import backlog items via CSV upload." },
            new BacklogItem { Title = "Real-time sizing session", Description = "Enable live planning poker sessions for remote teams." },
            new BacklogItem { Title = "Priority voting", Description = "Let team members vote on item priority asynchronously." },
        };

        foreach (var item in seed)
            _items[item.Id] = item;
    }

    public IReadOnlyList<BacklogItem> GetAll() =>
        _items.Values.OrderBy(i => i.CreatedAt).ToList();

    public BacklogItem? GetById(Guid id) =>
        _items.TryGetValue(id, out var item) ? item : null;

    public BacklogItem Add(BacklogItem item)
    {
        _items[item.Id] = item;
        return item;
    }

    public BacklogItem? Update(Guid id, BacklogItem updated)
    {
        if (!_items.TryGetValue(id, out var existing))
            return null;

        existing.Title = updated.Title;
        existing.Description = updated.Description;
        existing.UpdatedAt = DateTimeOffset.UtcNow;
        return existing;
    }

    public BacklogItem? ApplySize(Guid id, SizeRequest request)
    {
        if (!_items.TryGetValue(id, out var item))
            return null;

        item.StoryPoints = request.StoryPoints;
        item.Priority = request.Priority;
        item.UpdatedAt = DateTimeOffset.UtcNow;
        return item;
    }

    public BacklogItem? AddRanking(Guid id, UserRanking ranking)
    {
        if (!_items.TryGetValue(id, out var item))
            return null;

        item.Rankings.RemoveAll(r => r.UserName == ranking.UserName);
        item.Rankings.Add(ranking);
        item.UpdatedAt = DateTimeOffset.UtcNow;
        return item;
    }

    public bool Delete(Guid id) => _items.TryRemove(id, out _);
}
