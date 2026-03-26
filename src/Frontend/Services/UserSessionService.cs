namespace Frontend.Services;

public class UserSessionService
{
    public string? UserName { get; set; }

    public event Action? OnShowNamePromptRequested;

    public void RequestNamePrompt() => OnShowNamePromptRequested?.Invoke();
}
