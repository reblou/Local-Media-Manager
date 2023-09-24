using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public class Video
    {
        public string filePath;
        public string title { get; set; }
        public string fileName { get; set; }
        public string description { get; set; }
        public string releaseYear { get; set; }
        //TODO: remove hardcoded value.
        public string posterURL { get; set; } = "/images/1024px-Filmreel-icon.png";
        public string backdropURL { get; set; }

        public Video() { }

        public Video(string filename, string filepath)
        {
            this.fileName = filename;
            this.filePath = filepath;

            TitleParser parser = new TitleParser();
            parser.ParseTitleFromFilename(Path.ChangeExtension(fileName, ""));

            this.title = parser.title;
            this.releaseYear = parser.releaseYear;
        }

        public override string ToString()
        {
            return title;
        }

        public void LookupDetails(TMDBApiHandler apiHandler)
        {
            Result results = new Result();
            if (String.IsNullOrEmpty(releaseYear))
            {
                results = apiHandler.SearchMovieTitleOnly(title);
            }
            else
            {
                results = apiHandler.SearchMovie(title, releaseYear);
            }

            if(results.GetType() != typeof(EmptyResult))
            {
                title = results.title;
                posterURL = results.poster_path;
            }
            description = results.overview;
            backdropURL = results.backdrop_path;
        }
    }
}
