Console.WriteLine("Sample tree generator");

var dirTreeRoot = Path.Combine(Environment.CurrentDirectory, "sample");
Console.WriteLine($"Sample tree root: {dirTreeRoot}");
const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789_";

if (Directory.Exists(dirTreeRoot))
{
    Directory.Delete(dirTreeRoot, true);
}

int deep = 10;
int width = 50;
int count = 1;

foreach(string dirName in GenDirTree(deep, width))
{
    var treeDir = dirTreeRoot + dirName;
    Console.WriteLine($"{count}\t{dirName}");
    Directory.CreateDirectory(treeDir);
    
    for (int i = 0; i < width; i++)
    {
        var fileName = $"{dirName}\\{GenName(5)}.txt";
        Console.WriteLine($"{count++}\t{fileName}");
        File.AppendAllText(dirTreeRoot + fileName, fileName);
    }
}


static IEnumerable<string> GenDirTree(int deep, int width)
{   
    string genPath = "\\";
    for (int i = 0; i < deep; i++)
    {
        genPath = Path.Combine(genPath, GenName(5));
        yield return genPath;

        for (int j = 0; j < width; j++)
        {
            var subDir = Path.Combine(genPath, GenName(5));
            yield return subDir;
        }
    }
}

static string GenName(int length)
{    
    var name = new char[length];
    var random = new Random();
    for (int i = 0; i < name.Length; i++)
    {
        name[i] = chars[random.Next(chars.Length)];
    }

    return new String(name); 
}




