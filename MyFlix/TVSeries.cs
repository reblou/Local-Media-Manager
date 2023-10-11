using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix
{
    public class TVSeries : IPlayable
    {
        public Dictionary<int, Season> seasons;

        public TVSeries() 
        {
            // Parse title
        }

        public void LookupDetails(TMDBApiHandler handler)
        {
            throw new NotImplementedException();
        }
    }

    public class Season
    {
        public int seasonNumber;
        public List<Episode> episodes;
    }
}
