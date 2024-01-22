using MyFlix.Lookup.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingTests
{
    [TestClass]
    public class StateParserTests
    {

        [TestMethod]
        public void AnimeTest1()
        {
            string filename = @"[Sephirotic] Evangelion - 02 [MULTI][BD 1080p 8bits 5.1 AAC][C08EC897].mkv";

            Parser parser = new Parser();

            ParsedInformation info = parser.ParseFilename(filename);

            Assert.AreEqual("Evangelion", info.title);
            Assert.IsTrue(String.IsNullOrEmpty(info.year));
            Assert.AreEqual(true, info.isEpisode);
        }

        [TestMethod]
        public void AnimeTest2()
        {
            string filename = @"[Judas] Shingeki no Kyojin S2 - 02.mkv";

            Parser parser = new Parser();

            ParsedInformation info = parser.ParseFilename(filename);

            Assert.AreEqual("Shingeki no Kyojin", info.title);
            Assert.IsTrue(String.IsNullOrEmpty(info.year));

            Assert.AreEqual(true, info.isEpisode);
        }

        [TestMethod]
        public void TvTest1()
        {
            string filename = @"The.Mandalorian.S01E07.2019.1080p.WEBRip.X264.AC3-EVO.mkv";
            
            Parser parser = new Parser();

            ParsedInformation info = parser.ParseFilename(filename);

            Assert.AreEqual("The Mandalorian", info.title);
            Assert.AreEqual("2019", info.year);
            Assert.AreEqual(true, info.isEpisode);
        }

        [TestMethod]
        public void TvTest2()
        {
            string filename = @"The Simpsons S03E02 Mr. Lisa Goes to Washington.mp4";
            Parser parser = new Parser();

            ParsedInformation info = parser.ParseFilename(filename);

            Assert.AreEqual("The Simpsons", info.title);
            Assert.IsTrue(String.IsNullOrEmpty(info.year));
            Assert.AreEqual(true, info.isEpisode);
        }

        [TestMethod]
        public void TvTest3()
        {
            string filename = @"Over the Garden Wall (2014) - S01E03 - Schooltown Follies (1080p BluRay x265 RZeroX).mkv";
            Parser parser = new Parser();

            ParsedInformation info = parser.ParseFilename(filename);

            Assert.AreEqual("Over the Garden Wall", info.title);
            Assert.AreEqual("2014", info.year);
            Assert.AreEqual(true, info.isEpisode);
        }

        [TestMethod]
        public void TvTest4()
        {
            string filename = @"The.Sopranos.S01E01.1080p.BluRay.x265-RARBG.mp4";
            Parser parser = new Parser();

            ParsedInformation info = parser.ParseFilename(filename);

            Assert.AreEqual("The Sopranos", info.title);
            Assert.IsTrue(String.IsNullOrEmpty(info.year));
            Assert.AreEqual(true, info.isEpisode);
        }

        [TestMethod]
        public void YearInTitle()
        {
            string filename = @"2001.A.Space.Odyssey.(1968).1080p.WEBRip.X264.AC3-EVO.mkv";
            Parser parser = new Parser();

            ParsedInformation info = parser.ParseFilename(filename);

            Assert.AreEqual("2001 A Space Odyssey", info.title);
            Assert.AreEqual("1968", info.year);
            Assert.AreEqual(false, info.isEpisode);

        }

        [TestMethod]
        public void YearInTitle2()
        {
            string filename = @"Blade Runner 2049.(2017).1080p.WEBRip.X264.AC3-EVO.mkv";
            Parser parser = new Parser();

            ParsedInformation info = parser.ParseFilename(filename);

            Assert.AreEqual("Blade Runner 2049", info.title);
            Assert.AreEqual("2017", info.year);
            Assert.AreEqual(false, info.isEpisode);
        }


        [TestMethod]
        public void FilmTest1()
        {
            string filename = @"Arrival 2016 1080p BluRay x264 DTS-JYK.mkv";

            Parser parser = new Parser();

            ParsedInformation info = parser.ParseFilename(filename);

            Assert.AreEqual("Arrival", info.title);
            Assert.AreEqual("2016", info.year);
            Assert.AreEqual(false, info.isEpisode);
        }
        [TestMethod]
        public void FilmTest2()
        {
            string filename = @"Bill and Teds Excellent Adventure (1989) 1080p x264 Phun Psyz.mp4";

            Parser parser = new Parser();

            ParsedInformation info = parser.ParseFilename(filename);

            Assert.AreEqual("Bill and Teds Excellent Adventure", info.title);
            Assert.AreEqual("1989", info.year);
            Assert.AreEqual(false, info.isEpisode);
        }

        [TestMethod]
        public void FilmTest3()
        {
            string filename = @"Apollo 13 1995 1080p BluRay x264 DTS-JYK.mkv";

            Parser parser = new Parser();

            ParsedInformation info = parser.ParseFilename(filename);

            Assert.AreEqual("Apollo 13", info.title);
            Assert.AreEqual("1995", info.year);
            Assert.AreEqual(false, info.isEpisode);
        }
    }
}
