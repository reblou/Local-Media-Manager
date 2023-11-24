using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix
{
    public class Episode : IPlayable
    {
        public string title { get; set; }
        public string releaseYear;
        public int seasonNumber;
        public int episodeNumber;
        public string fileName { get; set; }
        public string filePath { get; set; }

        public Episode(string title, string releaseYear, int seasonNumber, int episodeNumber, string fileName, string filePath)
        {
            this.title = title;
            this.releaseYear = releaseYear;
            this.seasonNumber = seasonNumber;
            this.episodeNumber = episodeNumber;
            this.fileName = fileName;
            this.filePath = filePath;
        }
    }
}
