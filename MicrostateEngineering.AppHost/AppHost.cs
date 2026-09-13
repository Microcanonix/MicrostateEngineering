var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.MainConsole>("mainconsole")
    .WithExplicitStart();

var webApi = builder.AddProject<Projects.WebApi>("webapi");

builder.AddViteApp(
        "microstate-web",
        "../Web",
        runScriptName: "start")
    .WithReference(webApi)
    .WaitFor(webApi)
    .WithExternalHttpEndpoints()
    .WithExplicitStart();

builder.Build().Run();
