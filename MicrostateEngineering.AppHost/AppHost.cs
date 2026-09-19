var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.MainConsole>("mainconsole")
    .WithExplicitStart();

var webApi = builder.AddProject<Projects.WebApi>("webapi")
    .WithExternalHttpEndpoints()    // expose external HTTP endpoints for the AppHost dashboard
    .WithExplicitStart();

builder.AddViteApp(
        "microstate-web",
        "../Web",
        runScriptName: "start")
    .WithReference(webApi)
    .WaitFor(webApi)
    .WithExternalHttpEndpoints()
    .WithExplicitStart();

builder.Build().Run();
