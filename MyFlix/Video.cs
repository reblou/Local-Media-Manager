using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

    public class BoundVideo : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;
        private Video _video;
        public Video Video { get { return _video; } set
            { 
                _video = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(_video)));
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
        public string fileName { get; set; }
        public string description { get; set; }
        public string releaseYear { get; set; }
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


    public class NoResultsVideo : Video
    {
        public NoResultsVideo() 
        {
            this.title = this.fileName;
            this.posterURL = "/images/1024px-Filmreel-icon.png";
        }
    }
}
