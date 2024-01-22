using MyFlix.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace MyFlix
{
    public class TVSeries : IDisplayable
    {
        public string title { get; set; }
        public string description { get; set; }
        public string posterURL { get; set; }
        public string backdropURL { get; set; }
        public string releaseYear { get; set; }

        public string type { get => this.GetType().Name; }

        public Dictionary<int, Season> seasons;

        private List<Episode> episodes;
        public List<Episode> Episodes { get => episodes.OrderBy(e => e.fileName).ToList(); set => episodes = value; }

        public TVSeries(string title, string releaseYear)
        {
            this.title = title;
            this.releaseYear = releaseYear;
            seasons = new Dictionary<int, Season>();
            episodes = new List<Episode>();
        }

        public void LookupDetails(TMDBApiHandler handler)
        {
            TVResult results = handler.SearchTV(title, releaseYear);

            title = results.name;
            releaseYear = GetYearFromAirDate(results.first_air_date);
            description = results.overview;
            posterURL = results.poster_path;
            backdropURL = results.backdrop_path;
        }

        public void AddEpisode(Episode episode)
        {
            episodes.Add(episode);
        }

        public bool RepresentsFilename(string filename)
        {
            foreach(Episode episode in episodes)
            {
                if(episode.fileName == filename) return true;
            }
            return false;
        }

        public IPlayable GetNextPlayable()
        {
            if (episodes == null) throw new NullReferenceException("No playables to return.");

            if(episodes.Count < 1) throw new NullReferenceException("No playables to return.");

            foreach (Episode episode in Episodes)
            {
                if (!episode.BeenWatched) return episode;
            }

            //If all playables have been watched reset and return from the start again.

            foreach (Episode episode in Episodes)
            {
                episode.BeenWatched = false;
            }
            
            UserMediaSaver.SaveDisplayable(this);

            return GetNextPlayable();
        }

        private string GetYearFromAirDate(string first_air_date)
        {
            if (String.IsNullOrEmpty(first_air_date)) return String.Empty;
            return first_air_date.Substring(0, 4);
        }

        public List<IPlayable> GetPlayables()
        {
            List<IPlayable> playables = new List<IPlayable>();
            if (episodes == null) return playables;

            playables.AddRange(Episodes);

            return playables;
        }

        public void SetPlayable(IPlayable playable)
        {
            if (playable is not Episode) return;

            Episode newEpisode = playable as Episode;

            for(int i=0; i<episodes.Count; i++)
            {
                if (episodes[i].filePath == newEpisode.filePath)
                {
                    episodes[i] = newEpisode;
                    return;
                }
            }
        }
    }
}
