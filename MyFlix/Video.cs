using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

    public class Video
    {
        public string filePath;
        public string title;
        public string fileName;
        public string description;
        public string posterURL;
        public string backdropURL;

        public override string ToString()
        {
            return title;
        }
    }

    public class Folder
    {
        List<Video> videos { get; set; }
    }
}
