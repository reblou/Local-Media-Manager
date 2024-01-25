using GalaSoft.MvvmLight.Command;
using MyFlix.RenameTool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace MyFlix.ViewModels
{
    public class RenameToolViewModel
    {
        public string SearchRegex { get; set; }
        public string ReplaceRegex { get; set; }

        public ICommand folderPickerCommand { get; }
        public ICommand renameCommand { get; }
        public ICommand undoCommand { get; }

        public ObservableCollection<RenameToolVideo> renamingVideos { get; set; }

        public RenameToolViewModel() {
            folderPickerCommand = new RelayCommand(FolderPicker);
            renameCommand = new RelayCommand(RenameCommand);
            undoCommand = new RelayCommand(UndoCommand);

            renamingVideos = new ObservableCollection<RenameToolVideo>();

            SearchRegex = "";
            ReplaceRegex = "";
        }

        private void FolderPicker()
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new()
            {
                //options here
            };

            DialogResult result = dialog.ShowDialog();

            if (result != System.Windows.Forms.DialogResult.OK)
            {
                //If user cancelled out
                return;
            }

            renamingVideos.Clear();

            string rootFilePath = dialog.SelectedPath;

            FileSystemSearcher searcher = new();
            searcher.GetVideosInDirRecursively(rootFilePath);

            List<FileSystemVideo> videos = searcher.videos;

            //TODO: calculate every time input text changed ?

            foreach(FileSystemVideo video in videos)
            {
                //TODO: error handling of invalid regex formats etc.
                string newName = Regex.Replace(video.fileName, SearchRegex, ReplaceRegex);

                renamingVideos.Add(new RenameToolVideo() { 
                    fileName = video.fileName,
                    filePath = video.filePath,
                    newName = newName,
                });
            }
        }

        private void RenameCommand()
        {
            foreach (RenameToolVideo video in renamingVideos)
            {
                if (video.fileName == video.newName) continue;

                string newPath = Path.Combine(Path.GetDirectoryName(video.filePath), video.newName);

                //TODO: error handling 
                File.Move(video.filePath, newPath);
            }
        }

        private void UndoCommand()
        {
            // go through rename videos 

            // rename newName -> filename
        }
    }
}
