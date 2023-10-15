using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix
{
    internal class TVSeriesFactory
    {
        Dictionary<string, TVSeries> series;

        public TVSeriesFactory()
        {
            series = new Dictionary<string, TVSeries>();
        }

        public void Add(Episode episode)
        {
            if (series.ContainsKey(episode.title))
            {
                series[episode.title].AddEpisode(episode);
            }
            else
            {
                TVSeries newSeries = new TVSeries(episode.title, episode.releaseYear);
                series.Add(newSeries.title, newSeries);

                series[newSeries.title].AddEpisode(episode);
            }
        }

        public List<TVSeries> GetSeries()
        {
            return series.Values.ToList();
        }
    }
}
