using System.IO.Packaging;

namespace TestProject
{
    [TestClass]
    public class FilenameParserTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            FilenameParser parser = new FilenameParser();

            string filename = @"Bloom Into You (2018) - S01E02 - Heating Up Application for First Love.mkv";
            parser.ParseFilename(filename);

            
        }
    }
}