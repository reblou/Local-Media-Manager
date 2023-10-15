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
using System.Windows.Forms;
using System.Windows.Markup;
using Newtonsoft.Json;

namespace MyFlix
{
    public class Film : IPlayable, IDisplayable
    {
        public string filePath;
        public string fileName;
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
            FilmResult results = new FilmResult();
            if (String.IsNullOrEmpty(releaseYear))
            {
                results = apiHandler.SearchMovieTitleOnly(title);
            }
            else
            {
                results = apiHandler.SearchMovie(title, releaseYear);
            }

            title = results.title;
            posterURL = results.poster_path;
            description = results.overview;
            backdropURL = results.backdrop_path;
        }
    }
}
