using GalaSoft.MvvmLight.Command;
using MyFlix.Lookup;
using MyFlix.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Navigation;

namespace MyFlix.ViewModels
{
    public class TileViewModel
    {
        public ObservableCollection<IDisplayable> displayables { get; set; }
        private BackgroundWorker mediaLookupWorker;
        UserSettingsManager userSettingsManager;

        public ICommand lookupCommand { get; }
        public ICommand excludeCommand { get; }
        public ICommand refreshCommand { get; }
        public ICommand wipeCommand { get; }

        public TileViewModel()
        {
            userSettingsManager = new UserSettingsManager();
            displayables = new ObservableCollection<IDisplayable>();
            mediaLookupWorker = MediaLookupWorker.GetMediaLookupWorker(MediaLookupWorker_Progress, MediaLookupWorker_Completed);

            lookupCommand = new RelayCommand(SetFolderClick);
            excludeCommand = new RelayCommand(ExcludeClick);
            refreshCommand = new RelayCommand(RefreshClick);
            wipeCommand = new RelayCommand(WipeClick);

            LoadDisplayablesFromFile();
        }

        private void SetFolderClick()
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new()
            {
                //options here
            };

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string rootFilePath = dialog.SelectedPath;
                userSettingsManager.SetRootFilePath(rootFilePath);

                SetRootFolder(rootFilePath);
            }

        }

        //TODO: move to another class
        private void SetRootFolder(string rootFolder)
        {
            if (String.IsNullOrEmpty(rootFolder)) return;

            displayables.Clear();

            if (mediaLookupWorker.IsBusy)
            {
                // cancel any existing background worker process

                // TODO: find a way to do this that ensures first thread has finished before another can start.
                mediaLookupWorker.CancelAsync();
                mediaLookupWorker = MediaLookupWorker.GetMediaLookupWorker(MediaLookupWorker_Progress, MediaLookupWorker_Completed);
            }

            // get file system videos from file searcher
            FileSystemSearcher searcher = new();
            searcher.GetVideosInDirRecursively(rootFolder);

            // eliminate those already loaded from json
            List<FileSystemVideo> newVideos = Exclude(searcher.videos);

            // new background worker -> search API for details for new files.
            // reports progress, adds vidoes as it goes

            mediaLookupWorker.RunWorkerAsync(newVideos);
        }

        private void ExcludeClick()
        {
            ExcludeFilesWindow excludeWindow = new ExcludeFilesWindow();

            excludeWindow.Show();
        }

        private void WipeClick()
        {
            // wipe save data
            userSettingsManager.WipeSettings();

            // clear media details items
            displayables.Clear();
            UserMediaSaver.SaveToFile(this.displayables.ToList());
        }

        private void RefreshClick()
        {
            SetRootFolder(userSettingsManager.settings.RootFilePath);
        }



        private void LoadDisplayablesFromFile()
        {
            foreach(IDisplayable displayable in UserMediaSaver.LoadFromFile())
            {
                displayables.Add(displayable);
            }
        }

        private void SaveToFile()
        {
            UserMediaSaver.SaveToFile(this.displayables.ToList());
        }

        private void MediaLookupWorker_Progress(object? sender, ProgressChangedEventArgs e)
        {
            IDisplayable video = (IDisplayable)e.UserState;

            displayables.Add(video);
        }

        private void MediaLookupWorker_Completed(object? sender, RunWorkerCompletedEventArgs e)
        {
            SaveToFile();
        }

        private List<FileSystemVideo> Exclude(List<FileSystemVideo> videos)
        {
            List<FileSystemVideo> newVideos = new List<FileSystemVideo>();

            foreach (FileSystemVideo v in videos)
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
            foreach (IDisplayable displayable in displayables)
            {
                if (displayable.RepresentsFilename(fileName)) return true;
            }
            return false;
        }
    }
}
