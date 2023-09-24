﻿using Newtonsoft.Json;
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
        private BackgroundWorker apiSearchWorker;
        private readonly string filename = "user-media.json";

        public MediaList() : base()
        {
            LoadFromFile();

            apiSearchWorker = new BackgroundWorker();
            apiSearchWorker.DoWork += GetMediaDetails;

            apiSearchWorker.WorkerReportsProgress = true;
            apiSearchWorker.ProgressChanged += GetMediaDetailsProgress;
            apiSearchWorker.RunWorkerCompleted += MediaRetrieved;
        }

        public void AddList(List<Video> list)
        {
            foreach (Video video in list)
            {
                // If media already in list ignore it
                //if (this.Items.Any(vid => vid.title == video.title)) continue;

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

        public void LoadMediaFromDirectoryRecursively(string directory)
        {
            // get list<video> from file searcher
            FileSystemSearcher searcher = new();
            searcher.GetVideosInDirRecursively(directory);

            // eliminate those already loaded from json
            List<Video> newVideos = Exclude(searcher.videos);

            //  TODO: cancel any existing background worker process

            // new background worker -> search API for details for new files.
            // reports progress, adds vidoes as it goes
            apiSearchWorker.RunWorkerAsync(newVideos);
        }

        private void GetMediaDetails(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            List<Video> newVideos = (List<Video>)e.Argument;

            TMDBApiHandler handler = new TMDBApiHandler();

            foreach(Video video in newVideos)
            {
                video.LookupDetails(handler);
                worker.ReportProgress(0, video);
            }
        }

        private void GetMediaDetailsProgress(object sender, ProgressChangedEventArgs e)
        {
            Video video = (Video)e.UserState;

            this.Add(video);
        }

        private void MediaRetrieved(object sender, RunWorkerCompletedEventArgs e)
        {
            this.SaveToFile();
        }

        private List<Video> Exclude(List<Video> videos)
        {
            List<Video> newVideos = new List<Video>();

            foreach(Video v in videos) 
            { 
                if (!this.Items.Any(i => i.fileName == v.fileName))
                {
                    newVideos.Add(v);
                }
            }

            return newVideos;
        }
    }
}
