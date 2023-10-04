using System.IO.Packaging;

namespace TestProject
{
    [TestClass]
    public class FilenameParserTests
    {
        [TestMethod]
        public void AnimeTest1()
        {
            FilenameParser parser = new FilenameParser();

            string filename = @"[Sephirotic] Evangelion - 02 [MULTI][BD 1080p 8bits 5.1 AAC][C08EC897].mkv";
            parser.ParseFilename(filename);

            Assert.AreEqual("Evangelion", parser.title);
            Assert.AreEqual("", parser.releaseYear);
            Assert.AreEqual(-1, parser.season);
            Assert.AreEqual(2, parser.episode);
        }

        [TestMethod]
        public void AnimeTest2()
        {
            FilenameParser parser = new FilenameParser();

            string filename = @"[Judas] Shingeki no Kyojin S2 - 02.mkv";
            parser.ParseFilename(filename);

            Assert.AreEqual("Shingeki no Kyojin", parser.title);
            Assert.AreEqual("", parser.releaseYear);
            Assert.AreEqual(2, parser.episode);
            Assert.AreEqual(2, parser.season);
        }

        [TestMethod]
        public void TvTest1()
        {
            FilenameParser parser = new FilenameParser();
            string filename = @"The.Mandalorian.S01E07.2019.1080p.WEBRip.X264.AC3-EVO.mkv";
            parser.ParseFilename(filename);

            Assert.AreEqual("The Mandalorian", parser.title);
            Assert.AreEqual("2019", parser.releaseYear);
            Assert.AreEqual(1, parser.season);
            Assert.AreEqual(7, parser.episode);
        }

        [TestMethod]
        public void TvTest2()
        {
            FilenameParser parser = new FilenameParser();
            string filename = @"The Simpsons S03E02 Mr. Lisa Goes to Washington.mp4";
            parser.ParseFilename(filename);

            Assert.AreEqual("The Simpsons", parser.title);
            Assert.AreEqual("", parser.releaseYear);
            Assert.AreEqual(3, parser.season);
            Assert.AreEqual(2, parser.episode);
        }

        [TestMethod]
        public void TvTest3()
        {
            FilenameParser parser = new FilenameParser();
            string filename = @"Over the Garden Wall (2014) - S01E03 - Schooltown Follies (1080p BluRay x265 RZeroX).mkv";
            parser.ParseFilename(filename);

            Assert.AreEqual("Over the Garden Wall", parser.title);
            Assert.AreEqual("2014", parser.releaseYear);
            Assert.AreEqual(1, parser.season);
            Assert.AreEqual(3, parser.episode);
        }

        [TestMethod]
        public void YearInTitle()
        {
            FilenameParser parser = new FilenameParser();
            string filename = @"2001.A.Space.Odyssey.(1968).1080p.WEBRip.X264.AC3-EVO.mkv";
            parser.ParseFilename(filename);

            Assert.AreEqual("2001 A Space Odyssey", parser.title);
            Assert.AreEqual("1968", parser.releaseYear);
            Assert.AreEqual(-1, parser.season);
            Assert.AreEqual(-1, parser.episode);
        }

        [TestMethod]
        public void YearInTitle2()
        {
            FilenameParser parser = new FilenameParser();
            string filename = @"Blade Runner 2049.(2017).1080p.WEBRip.X264.AC3-EVO.mkv";
            parser.ParseFilename(filename);

            Assert.AreEqual("Blade Runner 2049", parser.title);
            Assert.AreEqual("2017", parser.releaseYear);
            Assert.AreEqual(-1, parser.season);
            Assert.AreEqual(-1, parser.episode);
        }


        [TestMethod]
        public void FilmTest1()
        {
            FilenameParser parser = new FilenameParser();
            string filename = @"Arrival 2016 1080p BluRay x264 DTS-JYK.mkv";
            parser.ParseFilename(filename);

            Assert.AreEqual("Arrival", parser.title);
            Assert.AreEqual("2016", parser.releaseYear);
            Assert.AreEqual(-1, parser.season);
            Assert.AreEqual(-1, parser.episode);
        }

    }
}