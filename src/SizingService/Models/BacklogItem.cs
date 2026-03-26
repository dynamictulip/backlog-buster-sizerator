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
}

public record SizeRequest(int? StoryPoints, Priority Priority);

public enum Priority
{
    Unset = 0,
    Low = 1,
    Medium = 2,
    High = 3,
    Critical = 4
}
