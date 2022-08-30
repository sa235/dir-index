namespace DirIndex.Tests
{
    [TestClass]
    public class IndexersUnitTest
    {
        [TestMethod]
        public void TestIndexer()
        {
            string? testPath = Directory.GetCurrentDirectory();
            
            var outFiles = Directory.GetFiles(testPath, "DirIndex_*.xlsx");
            foreach(var fi in outFiles)
            {
                File.Delete(fi);
            }

            Console.WriteLine(testPath);

            try
            {
                var indexer = new Indexer(testPath, testPath);
                indexer.CreateExelIndex();
                Console.WriteLine($"Indexing complete");
                Console.WriteLine($"Index file: {indexer.OutFileName}");
            }
            catch (Exception er)
            {
                Console.WriteLine(er.ToString());
            }

            outFiles = Directory.GetFiles(testPath, "DirIndex_*.xlsx");
            Assert.AreEqual(1, outFiles.Length);
        }
    }
}