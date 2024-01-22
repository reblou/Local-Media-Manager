using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix.Lookup
{
    public class MediaLookupWorker
    {

        public static BackgroundWorker GetMediaLookupWorker(ProgressChangedEventHandler progressHandler, RunWorkerCompletedEventHandler completeHandler)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += Do_Work;

            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.ProgressChanged += progressHandler;
            worker.RunWorkerCompleted += completeHandler;

            return worker;
        }

        // Goes through list and looks up details from API, reporting progess after each lookup.
        public static void Do_Work(object sender, DoWorkEventArgs args)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            List<FileSystemVideo> newVideos = (List<FileSystemVideo>)args.Argument;

            TMDBApiHandler handler = new TMDBApiHandler();

            TVSeriesFactory seriesFactory = new TVSeriesFactory();

            foreach (FileSystemVideo fsVideo in newVideos)
            {
                if (worker.CancellationPending)
                {
                    return;
                }

                IPlayable playable = PlayableFactory.CreatePlayableFromFilename(fsVideo.fileName, fsVideo.filePath);

                // If displayable and not part of a series
                if (playable is IDisplayable displayable)
                {
                    displayable.LookupDetails(handler);
                    worker.ReportProgress(0, displayable);
                }
                else if(playable is Episode ep)
                {
                    bool exists = seriesFactory.AddIfSeriesExists(ep);
                    // If series not created yet lookup details
                    if(!exists) 
                    {
                        TVSeries series = seriesFactory.CreateSeriesAndAdd(ep);

                        series.LookupDetails(handler);
                        worker.ReportProgress(0, series);
                    }
                }
            }
        }
    }
}
