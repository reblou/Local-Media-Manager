using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix
{
    public class Episode : IPlayable
    {
        public string title;
        public string releaseYear;
        public int seasonNumber;
        public int episodeNumber;

        public void LookupDetails(TMDBApiHandler handler)
        {
            throw new NotImplementedException();
        }
    }
}
