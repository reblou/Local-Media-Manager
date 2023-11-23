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

        public Dictionary<int, Season> seasons;

        public TVSeries(string title, string releaseYear)
        {
            this.title = title;
            this.releaseYear = releaseYear;
            seasons = new Dictionary<int, Season>();
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
            int season = episode.seasonNumber;

            if (season < 0) season = 1;

            if(!seasons.ContainsKey(season)) seasons[season] = new Season();

            seasons[season].Add(episode);
        }

        public bool RepresentsFilename(string filename)
        {
            foreach(Season season in seasons.Values)
            {
                foreach(Episode episode in season.episodes)
                {
                    if(episode.fileName == filename) return true;
                }
            }
            return false;
        }

        public IPlayable GetNextPlayable()
        {
            //TODO: store watched episodes, get next unwatched.
            return seasons[1].episodes[0];
        }

        private string GetYearFromAirDate(string first_air_date)
        {
            return first_air_date.Substring(0, 4);
        }

        public List<IPlayable> GetPlayables()
        {
            List<IPlayable> episodes = new List<IPlayable>();
            if (seasons == null) return episodes;

            foreach (Season season in seasons.Values)
            {
                episodes.AddRange(season.episodes);
            }

            return episodes;
        }
    }
}
