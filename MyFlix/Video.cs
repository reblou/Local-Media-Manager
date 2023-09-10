using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix
{
    public class MediaList : ObservableCollection<Video>
    {
        public MediaList() : base() { }

        public void AddList(List<Video> list)
        {
            foreach (Video video in list)
            {
                this.Add(video);
            }
        }
    }
    public class Folder
    {
        List<Video> videos { get; set; }
    }

    public class Video
    {
        public string filePath;
        public string title { get; set; }
        public string fileName;
        public string description;
        public string releaseYear;
        public string posterURL { get; set; }
        public string backdropURL;

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
            Video results = new Video();
            if (String.IsNullOrEmpty(releaseYear))
            {
                results = apiHandler.SearchMovieTitleOnly(title);
            }
            else
            {
                results = apiHandler.SearchMovie(title, releaseYear);
            }

            title = results.title;
            description = results.description;
            posterURL = results.posterURL;
            backdropURL = results.backdropURL;
        }
    }


    public class NoResultsVideo : Video
    {
        public NoResultsVideo() 
        {
            this.title = this.fileName;
            this.posterURL = "/images/1024px-Filmreel-icon.png";
        }
    }
}
