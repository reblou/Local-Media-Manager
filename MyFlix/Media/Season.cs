using System.Collections.Generic;

namespace MyFlix
{
    public class Season
    {
        public int seasonNumber;
        public List<Episode> episodes;

        public Season()
        {
            episodes = new List<Episode>();
        }

        public void Add(Episode episode)
        {
            episodes.Add(episode);
        }
    }
}
