using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix
{
    public class Episode : IPlayable
    {
        public string title { get; set; }
        public string releaseYear;
        public int seasonNumber;
        public int episodeNumber;

        public event PropertyChangedEventHandler PropertyChanged;

        public string fileName { get; set; }
        public string filePath { get; set; }


        private bool watched;
        public bool BeenWatched { get => watched ; set { watched = value; OnPropertyChanged(); } }


        public Episode(string title, string releaseYear, string fileName, string filePath)
        {
            this.title = title;
            this.releaseYear = releaseYear;
            this.fileName = fileName;
            this.filePath = filePath;
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            if (PropertyChanged == null) return;
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
