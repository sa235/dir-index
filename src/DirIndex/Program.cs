
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File(@"logs\log_.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

Log.Information("Directory indexing tool");

string? indexDir;
if (args.Length > 0)
{
    indexDir = args[0];
}
else
{
    Console.Write("Input indexing directory path:");
    indexDir = Console.ReadLine();
}

if(!Directory.Exists(indexDir))
{
    Log.Error($"Directory not exist: ({indexDir})");
    return;
}

Log.Information($"Indexing dirrectory: {indexDir}");

var indexer = new Indexer(indexDir);
try
{
    indexer.CreateExelIndex();
    Log.Information($"Indexing complete");
    Log.Information($"Index file: {indexer.OutFileName}");
}
catch(Exception er)
{
    Log.Error(er, "Indexation error");
}
