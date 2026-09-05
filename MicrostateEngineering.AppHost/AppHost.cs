var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.MainConsole>("mainconsole")
    .WithExplicitStart();

builder.AddViteApp(
        "microstate-web",
        "../Web",
        runScriptName: "start")
    .WithExternalHttpEndpoints()
    .WithExplicitStart();

builder.Build().Run();
