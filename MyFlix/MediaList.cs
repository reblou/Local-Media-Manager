﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyFlix
{
    public class MediaList : ObservableCollection<IDisplayable>
    {
        private BackgroundWorker apiSearchWorker;

        public MediaList() : base()
        {
            LoadFromFile();
            apiSearchWorker = BuildWorker();
        }

        private void AddList(List<IDisplayable> list)
        {
            foreach (IDisplayable video in list)
            {
                if (this.Items.Any(v => v.title == video.title)) continue;

                this.Add(video);
            }

            SaveToFile();
        }

        private void SaveToFile()
        {
            UserMediaSaver.SaveToFile(this.Items.ToList());
        }

        private void LoadFromFile()
        {
            AddList(UserMediaSaver.LoadFromFile());
        }

        public void LoadMediaFromDirectoryRecursively(string directory)
        {
            if(apiSearchWorker.IsBusy)
            {
                // cancel any existing background worker process
                apiSearchWorker.CancelAsync();
                apiSearchWorker = BuildWorker();
            }

            // get file system videos from file searcher
            FileSystemSearcher searcher = new();
            searcher.GetVideosInDirRecursively(directory);

            // eliminate those already loaded from json
            List<FileSystemVideo> newVideos = Exclude(searcher.videos);

            // new background worker -> search API for details for new files.
            // reports progress, adds vidoes as it goes

            apiSearchWorker.RunWorkerAsync(newVideos);
        }

        private void GetMediaDetails(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            List<FileSystemVideo> newVideos = (List<FileSystemVideo>)e.Argument;

            TMDBApiHandler handler = new TMDBApiHandler();

            TVSeriesFactory seriesFactory = new TVSeriesFactory();

            foreach(FileSystemVideo fsVideo in newVideos)
            {
                if (worker.CancellationPending)
                {
                    return;
                }

                IPlayable playable = PlayableFactory.CreatePlayableFromFilename(fsVideo.fileName, fsVideo.filePath);

                if (playable is IDisplayable displayable)
                {
                    //lookup details
                    displayable.LookupDetails(handler);
                    worker.ReportProgress(0, displayable);
                }
                else
                {
                    if(playable is Episode ep)
                    {
                        seriesFactory.Add(ep);
                    }
                }
            }

            // report progess on TV series to add to ui 
            List<TVSeries> seriesList = seriesFactory.GetSeries();
            foreach(TVSeries series in seriesList)
            {
                series.LookupDetails(handler);
                worker.ReportProgress(0, series);
            }
        }

        private void GetMediaDetailsProgress(object sender, ProgressChangedEventArgs e)
        {
            IDisplayable video = (IDisplayable)e.UserState;

            this.Add(video);
        }

        private void MediaRetrieved(object sender, RunWorkerCompletedEventArgs e)
        {
            this.SaveToFile();
        }

        private List<FileSystemVideo> Exclude(List<FileSystemVideo> videos)
        {
            List<FileSystemVideo> newVideos = new List<FileSystemVideo>();

            foreach(FileSystemVideo v in videos) 
            {
                if (FileExistsInList(v.fileName))
                {
                    continue;
                }
                else
                {
                    newVideos.Add(v);
                }
            }

            return newVideos;
        }

        private bool FileExistsInList(string fileName)
        {
            foreach(IDisplayable displayable in this.Items)
            {
                //TODO: Move to IDisplayable as bool RepresentsFilename(string filename)
                if(displayable is Film film)
                {
                    if (film.fileName == fileName) return true;
                }
                else if (displayable is TVSeries series)
                {
                    if(series.EpisodeHasFilename(fileName)) return true;
                }
            }
            return false;
        }

        private BackgroundWorker BuildWorker()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += GetMediaDetails;

            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.ProgressChanged += GetMediaDetailsProgress;
            worker.RunWorkerCompleted += MediaRetrieved;

            return worker;
        }
    }
}
