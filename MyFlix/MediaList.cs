using Newtonsoft.Json;
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
        public MediaList() : base()
        {
            LoadFromFile();
        }

        private readonly string filename = "user-media.json";

        public void AddList(List<Video> list)
        {
            foreach (Video video in list)
            {
                // If media already in list ignore it
                if (this.Items.Any(vid => vid.title == video.title)) continue;

                this.Add(video);
            }

            SaveToFile();
        }

        private void SaveToFile()
        {
            string output = JsonConvert.SerializeObject(this.Items);

            using StreamWriter writer = new StreamWriter(filename);
            writer.Write(output);
        }

        private void LoadFromFile()
        {
            try
            {
                TryToLoadFromFile();
            }
            catch (FileNotFoundException)
            {
                return;
            }
        }

        private void TryToLoadFromFile()
        {
            string data = "";
            using (StreamReader reader = new StreamReader(filename))
            {
                data = reader.ReadToEnd();
            }

            List<Video> savedVideos = JsonConvert.DeserializeObject<List<Video>>(data);
            AddList(savedVideos);
        }
    }
}
