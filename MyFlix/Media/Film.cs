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

        public Film(string filename, string filepath, string title, string releaseYear)
        {
            this.fileName = filename;
            this.filePath = filepath;

            this.title = title;
            this.releaseYear = releaseYear;
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
    }
}
