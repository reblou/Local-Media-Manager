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
        public List<Season> seasons;

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
    }
}
