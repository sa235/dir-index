public class PathParts
{
    public readonly string Path;
    public readonly int Rank;
    public Dictionary<int, string> Parts = new Dictionary<int, string>();

    public PathParts(string filePath)
    {
        this.Path = filePath.TrimStart('\\');
        var parts = this.Path.Split("\\");
        Rank = parts.Length - 1;
        for (int i = 0; i < Rank; i++)
        {
            Parts.Add(i, parts[i]);
        }
    }
}