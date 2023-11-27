using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Markup;
using MyFlix.Player;
using Newtonsoft.Json;

namespace MyFlix
{
    public class Film : IPlayable, IDisplayable
    {
        public string filePath { get; set; }
        public string fileName { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string posterURL { get; set; }
        public string backdropURL { get; set; }
        public string releaseYear { get; set; }
        public bool watched { get; set; }

        public Film(string filename, string filepath, string title, string releaseYear)
        {
            this.fileName = filename;
            this.filePath = filepath;

            this.title = title;
            this.releaseYear = releaseYear;
            this.watched = false;
        }

        public void AssignValues(Film film)
        {
            if (film == null) return; 

            this.fileName = film.fileName;
            this.filePath = film.filePath;
            this.title = film.title;
            this.description = film.description;
            this.posterURL = film.posterURL;
            this.backdropURL = film.backdropURL;
            this.releaseYear = film.releaseYear;
            this.watched = film.watched;

        }

        public override string ToString()
        {
            return title;
        }

        public void LookupDetails(TMDBApiHandler apiHandler)
        {
            FilmResult results = apiHandler.SearchMovie(title, releaseYear);

            title = results.title;
            posterURL = results.poster_path;
            description = results.overview;
            backdropURL = results.backdrop_path;
        }

        public bool RepresentsFilename(string filename)
        {
            return this.fileName == filename;
        }

        public IPlayable GetNextPlayable()
        {
            return this;
        }

        public List<IPlayable> GetPlayables()
        {
            return new List<IPlayable>() { this };
        }

        public void SetPlayable(IPlayable playable)
        {
            if (playable is not Film) return;

            AssignValues(playable as Film);
        }
    }
}
