using SizingService.Models;
using SizingService.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddSingleton<IBacklogRepository, InMemoryBacklogRepository>();
builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

var backlog = app.MapGroup("/api/backlog").WithTags("Backlog");

backlog.MapGet("/", (IBacklogRepository repo) => Results.Ok(repo.GetAll()))
    .WithName("GetBacklogItems")
    .WithSummary("Get all backlog items");

backlog.MapGet("/{id:guid}", (Guid id, IBacklogRepository repo) =>
    repo.GetById(id) is { } item ? Results.Ok(item) : Results.NotFound())
    .WithName("GetBacklogItem")
    .WithSummary("Get a backlog item by ID");

backlog.MapPost("/", (BacklogItem item, IBacklogRepository repo) =>
{
    var created = repo.Add(item);
    return Results.Created($"/api/backlog/{created.Id}", created);
})
    .WithName("CreateBacklogItem")
    .WithSummary("Create a new backlog item");

backlog.MapPut("/{id:guid}", (Guid id, BacklogItem item, IBacklogRepository repo) =>
    repo.Update(id, item) is { } updated ? Results.Ok(updated) : Results.NotFound())
    .WithName("UpdateBacklogItem")
    .WithSummary("Update a backlog item");

backlog.MapPatch("/{id:guid}/size", (Guid id, SizeRequest request, IBacklogRepository repo) =>
    repo.ApplySize(id, request) is { } sized ? Results.Ok(sized) : Results.NotFound())
    .WithName("SizeBacklogItem")
    .WithSummary("Apply story points and priority to a backlog item");

backlog.MapPost("/{id:guid}/rankings", (Guid id, UserRanking ranking, IBacklogRepository repo) =>
    repo.AddRanking(id, ranking) is { } ranked ? Results.Ok(ranked) : Results.NotFound())
    .WithName("AddRanking")
    .WithSummary("Submit a user ranking for a backlog item");

backlog.MapDelete("/{id:guid}", (Guid id, IBacklogRepository repo) =>
    repo.Delete(id) ? Results.NoContent() : Results.NotFound())
    .WithName("DeleteBacklogItem")
    .WithSummary("Delete a backlog item");

app.Run();
