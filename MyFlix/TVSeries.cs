using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix
{
    public class TVSeries : Media
    {
        public Dictionary<int, Season> seasons;

        public TVSeries(string filename, string filepath) 
        {
            this.fileName = filename;
            this.filePath = filepath;

            // Parse title
        }

        public override void LookupDetails(TMDBApiHandler handler)
        {
            throw new NotImplementedException();
        }
    }

    public class Season
    {
        public int seasonNumber;
        public List<Episode> episodes;
    }

    public class Episode
    {
        public string name;
        public int episodeNumber;

        public Episode(string filename)
        {
            TvTitleParser parser = new TvTitleParser();
            parser.ParseTitleFromFilename(filename);
            this.name = parser.title;
            this.episodeNumber = parser.episode;
        }
    }
}
