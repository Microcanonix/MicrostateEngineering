var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.MainConsole>("mainconsole")
    .WithExplicitStart();

builder.AddProject<Projects.WebApi>("webapi");

builder.AddViteApp(
        "microstate-web",
        "../Web",
        runScriptName: "start")
    .WithExternalHttpEndpoints()
    .WithExplicitStart();

builder.Build().Run();
