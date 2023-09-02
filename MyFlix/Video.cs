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
        public string filePath {  get; set; }
        public string title { get; set; }
        public string fileName { get; set; }

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
