

using Serilog;


Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(@"logs\log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

Log.Information("Directory indexing tool");
