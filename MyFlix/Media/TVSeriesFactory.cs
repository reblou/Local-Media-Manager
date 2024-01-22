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
            series = new Dictionary<string, TVSeries>(StringComparer.InvariantCultureIgnoreCase);
        }

        public bool AddIfSeriesExists(Episode episode)
        {
            if (!series.ContainsKey(episode.title)) return false;


            series[episode.title].AddEpisode(episode);
            return true;
        }

        public TVSeries CreateSeriesAndAdd(Episode episode)
        {
            TVSeries newSeries = new TVSeries(episode.title, episode.releaseYear);
            series.Add(newSeries.title, newSeries);

            series[newSeries.title].AddEpisode(episode);
            return series[newSeries.title];
        }

        public List<TVSeries> GetSeries()
        {
            return series.Values.ToList();
        }
    }
}
