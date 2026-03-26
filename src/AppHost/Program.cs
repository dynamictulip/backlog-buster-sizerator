var builder = DistributedApplication.CreateBuilder(args);

var sizingService = builder.AddProject<Projects.SizingService>("sizing-service");

builder.AddProject<Projects.Frontend>("frontend")
    .WithReference(sizingService)
    .WaitFor(sizingService);

builder.Build().Run();
