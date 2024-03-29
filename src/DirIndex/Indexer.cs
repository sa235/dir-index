﻿using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;


public class Indexer
{
    public const int excelMaxRow = 1048576;
    private readonly string indexDir;
    public readonly string OutFileName;

    public Indexer(string indexDir, string outDir)
    {
        this.indexDir = indexDir;
        OutFileName = outDir + $"{Path.DirectorySeparatorChar}DirIndex_{DateTime.Now.Minute}_{Path.GetRandomFileName()}.xlsx";
    }

    public void CreateExelIndex()
    {
        IWorkbook workbook = new XSSFWorkbook();

        ICellStyle headerStyle = CreateHeaderCellStyle(workbook);
        ICellStyle dateTimeStyle = CreateDateTimeCellStyle(workbook);

        int sheetIndex = 1;
        var rowIndex = 0;
        int maxRank = 0;

        ISheet sheet1 = null;
        IRow header = null;

        using var fs = new FileStream(OutFileName, FileMode.Create, FileAccess.Write);
        
        foreach (FileInfo fi in Scan(indexDir))
        {
            if (rowIndex == 0)
            {
                sheet1 = workbook.CreateSheet($"Index_{sheetIndex++}");
                header = sheet1.CreateRow(rowIndex);
                AddBaseHeader(headerStyle, header);
                rowIndex++;
            }

            //var relativePath = GetRelativePath(fi.FullName);
            var relativePath = Path.GetRelativePath(indexDir, fi.FullName);

            Console.WriteLine($"{rowIndex} {relativePath}");

            var item = new PathParts(relativePath);

            if (maxRank < item.Rank)
            {
                maxRank = item.Rank;
            }

            IRow row = sheet1.CreateRow(rowIndex);

            row.CreateCell(0).SetCellValue(item.Path);
            row.CreateCell(1).SetCellValue(fi.Extension);

            var cell = row.CreateCell(2);
            cell.CellStyle = dateTimeStyle;
            cell.SetCellValue(fi.LastWriteTime);

            row.CreateCell(3).SetCellValue(fi.Length);

            cell = row.CreateCell(4);
            cell.SetCellValue(fi.Name);

            //HyperLink corrupt excel file. WTF?
            //var targetLink = new XSSFHyperlink(HyperlinkType.File);
            //targetLink.Address = item.Path;
            //cell.Hyperlink = targetLink;

            foreach (var kv in item.Parts)
            {
                row.CreateCell(kv.Key + 5).SetCellValue(kv.Value);
            }

            rowIndex++;

            if (rowIndex % excelMaxRow == 0)
            {
                FinalizeSheet(sheet1, header, headerStyle, maxRank);                
                rowIndex = 0;                
            }
        }

        FinalizeSheet(sheet1, header, headerStyle, maxRank);
        workbook.Write(fs);
    }

    private static void FinalizeSheet(ISheet sheet1, IRow header, ICellStyle headerStyle, int maxRank)
    {
        for (int i = 0; i < maxRank; i++)
        {
            var cell = header.CreateCell(i + 5);
            cell.SetCellValue($"Level{i}");
            cell.CellStyle = headerStyle;
        }

        sheet1.AutoSizeColumn(2);
        sheet1.AutoSizeColumn(3);
        sheet1.AutoSizeColumn(4);

        sheet1.SetAutoFilter(new CellRangeAddress(0, 0, 0, maxRank + 5 - 1));
        sheet1.CreateFreezePane(0, 1);
    }

    private ICellStyle CreateDateTimeCellStyle(IWorkbook workbook)
    {
        var cellStyle = workbook.CreateCellStyle();
        IDataFormat dataFormatCustom = workbook.CreateDataFormat();
        cellStyle.DataFormat = dataFormatCustom.GetFormat("yyyy.MM.dd HH:mm:ss"); ;
        return cellStyle;
    }

    private static void AddBaseHeader(ICellStyle headerStyle, IRow header)
    {
        var cell = header.CreateCell(0);
        cell.SetCellValue("Path");
        cell.CellStyle = headerStyle;

        cell = header.CreateCell(1);
        cell.SetCellValue("Extension");
        cell.CellStyle = headerStyle;

        cell = header.CreateCell(2);
        cell.SetCellValue("LastWriteTime");
        cell.CellStyle = headerStyle;

        cell = header.CreateCell(3);
        cell.SetCellValue("Size");
        cell.CellStyle = headerStyle;

        cell = header.CreateCell(4);
        cell.SetCellValue("Name");
        cell.CellStyle = headerStyle;
    }

    private ICellStyle CreateHeaderCellStyle(IWorkbook workbook)
    {
        ICellStyle style = workbook.CreateCellStyle();
        XSSFFont font = (XSSFFont)workbook.CreateFont();
        font.IsBold = true;
        style.SetFont(font);
        style.VerticalAlignment = VerticalAlignment.Center;
        return style;
    }

    public string GetRelativePath(string path)
    {
        return path.Substring(indexDir.Length);
    }

    public static IEnumerable<FileInfo> Scan(string indexDir)
    {
        var queue = new Stack<string>();
        queue.Push(indexDir);

        while (queue.Count > 0)
        {
            var path = queue.Pop();

            foreach (var dirName in Directory.GetDirectories(path))
            {
                queue.Push(dirName);
            }

            var dir = new DirectoryInfo(path);

            foreach (FileInfo fi in dir.GetFiles())
            {
                yield return fi;
            }
        }
    }

}