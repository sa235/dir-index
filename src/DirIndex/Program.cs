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

string? outDir;

if (args.Length > 1)
{
    outDir = args[1];
}
else
{
    Console.Write("Input (or skip) out directory path:");
    outDir = Console.ReadLine();

    if(string.IsNullOrEmpty(outDir))
    {
        outDir = Directory.GetCurrentDirectory();
    }
}

if (!Directory.Exists(indexDir))
{
    Log.Error($"Directory not exist: ({indexDir})");
    return;
}

if (!Directory.Exists(outDir))
{
    Log.Error($"Directory not exist: ({outDir})");
    return;
}

Log.Information($"Indexing dirrectory: {indexDir}");

var indexer = new Indexer(indexDir, outDir);
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
