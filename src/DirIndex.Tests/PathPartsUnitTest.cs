namespace DirIndex.Tests
{
    [TestClass]
    public class PathPartsUnitTest
    {
        [TestMethod]
        public void TestPathParts()
        {
            var testPath = @"\aa\bb\cc";
            var pathParts = new PathParts(testPath);
            Assert.AreEqual(pathParts.Rank, 2);
            Assert.AreEqual(pathParts.Parts[0], "aa");
        }
        
        [TestMethod]
        public void TestPathParts2()
        {
            var testPath = @"/aa/bb/cc";
            var pathParts = new PathParts(testPath);
            Assert.AreEqual(pathParts.Rank, 2);
            Assert.AreEqual(pathParts.Parts[0], "aa");
        }
    }
}