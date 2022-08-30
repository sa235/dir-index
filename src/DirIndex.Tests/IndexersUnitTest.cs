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

            var indexer = new Indexer(testPath, testPath);
            indexer.CreateExelIndex();

            outFiles = Directory.GetFiles(testPath, "DirIndex_*.xlsx");
            Assert.AreEqual(outFiles.Length, 1);
        }
    }
}