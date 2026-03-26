using Frontend.Models;

namespace Frontend.Clients;

public class BacklogClient(HttpClient httpClient)
{
    public async Task<List<BacklogItem>> GetAllAsync() =>
        await httpClient.GetFromJsonAsync<List<BacklogItem>>("/api/backlog") ?? [];

    public async Task<BacklogItem?> GetByIdAsync(Guid id) =>
        await httpClient.GetFromJsonAsync<BacklogItem>($"/api/backlog/{id}");

    public async Task<BacklogItem?> CreateAsync(BacklogItem item)
    {
        var response = await httpClient.PostAsJsonAsync("/api/backlog", item);
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<BacklogItem>()
            : null;
    }

    public async Task<BacklogItem?> UpdateAsync(Guid id, BacklogItem item)
    {
        var response = await httpClient.PutAsJsonAsync($"/api/backlog/{id}", item);
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<BacklogItem>()
            : null;
    }

    public async Task<BacklogItem?> ApplySizeAsync(Guid id, SizeRequest request)
    {
        var response = await httpClient.PatchAsJsonAsync($"/api/backlog/{id}/size", request);
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<BacklogItem>()
            : null;
    }

    public async Task<BacklogItem?> AddRankingAsync(Guid id, UserRanking ranking)
    {
        var response = await httpClient.PostAsJsonAsync($"/api/backlog/{id}/rankings", ranking);
        return response.IsSuccessStatusCode
            ? await response.Content.ReadFromJsonAsync<BacklogItem>()
            : null;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var response = await httpClient.DeleteAsync($"/api/backlog/{id}");
        return response.IsSuccessStatusCode;
    }
}
