using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

public class Indexer
{
    private readonly string indexDir;
    public readonly string OutFileName;

    public Indexer(string indexDir)
    {
        this.indexDir = indexDir;
        OutFileName = indexDir + $"\\DirIndex_{DateTime.Now.Minute}.xlsx";
    }

    public void CreateExelIndex()
    {
        IWorkbook workbook = new XSSFWorkbook();
        ISheet sheet1 = workbook.CreateSheet("Index");

        var rowIndex = 0;
        foreach (FileInfo fi in Scan(indexDir))
        {
            var relativePath = GetRelativePath(fi.FullName);
            var fileName = fi.Name;
            var extention = fi.Extension;
            var fileSize = fi.Length;
            Console.WriteLine(relativePath);
            IRow row = sheet1.CreateRow(rowIndex);
            row.CreateCell(0).SetCellValue(fileName);
            row.CreateCell(1).SetCellValue(extention);
            row.CreateCell(2).SetCellValue(fileSize);
            row.CreateCell(3).SetCellValue(relativePath);
            rowIndex++;
        }

        using var fs = new FileStream(OutFileName, FileMode.Create, FileAccess.Write);
        workbook.Write(fs);
    }

    public string GetRelativePath(string path)
    {
        return path.Substring(indexDir.Length);
    }

    public static IEnumerable<FileInfo> Scan(string indexDir)
    {
        var queue = new Queue<string>();
        queue.Enqueue(indexDir);

        while(queue.Count > 0)
        {
            var path = queue.Dequeue();

            foreach(var dirName in Directory.GetDirectories(path))
            {
                queue.Enqueue(dirName);
            }

            var dir = new DirectoryInfo(path);

            foreach(FileInfo fi in dir.GetFiles())
            {
                yield return fi;
            }
        }
    }

}