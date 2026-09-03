var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.MainConsole>("mainconsole");


builder.Build().Run();
