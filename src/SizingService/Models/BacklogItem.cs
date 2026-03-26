namespace SizingService.Models;

public class BacklogItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? StoryPoints { get; set; }
    public Priority Priority { get; set; } = Priority.Unset;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }
    public List<UserRanking> Rankings { get; set; } = [];
    public double? AverageStoryPoints =>
        Rankings.Any(r => r.StoryPoints.HasValue)
            ? Rankings.Where(r => r.StoryPoints.HasValue).Average(r => (double)r.StoryPoints!.Value)
            : null;
    public double? AveragePriority =>
        Rankings.Any(r => r.Priority != Priority.Unset)
            ? Rankings.Where(r => r.Priority != Priority.Unset).Average(r => (double)r.Priority)
            : null;
}

public record SizeRequest(int? StoryPoints, Priority Priority);

public record UserRanking(string UserName, int? StoryPoints, Priority Priority);

public enum Priority
{
    Unset = 0,
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}
