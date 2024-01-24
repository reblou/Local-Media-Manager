using GalaSoft.MvvmLight.Command;
using MyFlix.RenameTool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

        public ObservableCollection<RenameToolVideo> renamingVideos { get; set; }

        public RenameToolViewModel() {
            folderPickerCommand = new RelayCommand(FolderPicker);
            renamingVideos = new ObservableCollection<RenameToolVideo>();
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

            string rootFilePath = dialog.SelectedPath;

            FileSystemSearcher searcher = new();
            searcher.GetVideosInDirRecursively(rootFilePath);

            List<FileSystemVideo> videos = searcher.videos;

            //TODO: calculate regex search/replace effect and set RenameToolVideo.newName 

            foreach(FileSystemVideo video in videos)
            {
                renamingVideos.Add(new RenameToolVideo() { 
                    fileName = video.fileName,
                    filePath = video.filePath,
                });
            }
        }
    }
}
