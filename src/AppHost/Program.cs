var builder = DistributedApplication.CreateBuilder(args);

var sizingService = builder.AddProject<Projects.SizingService>("sizing-service")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.Frontend>("frontend")
    .WithReference(sizingService)
    .WaitFor(sizingService)
    .WithHttpHealthCheck("/health")
    .WithExternalHttpEndpoints();

builder.Build().Run();
