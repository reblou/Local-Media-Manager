using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Navigation;

namespace MyFlix.ViewModels
{
    internal class IgnoreToolViewModel
    {
        public ICommand folderPickerCommand { get; }

        public ICommand searchCommand { get; }
        public ICommand commitCommand { get; }

        
        public ObservableCollection<string> ignoredFolders { get; set; }
        public string extrasPath { get; set; }

        public IgnoreToolViewModel()
        {
            folderPickerCommand = new RelayCommand(FolderPicker);
            searchCommand = new RelayCommand(Search);
            commitCommand = new RelayCommand(Commit);

            UserSettingsManager userSettings = new UserSettingsManager();
            string libraryRoot = userSettings.settings.RootFilePath;

            extrasPath = Path.Combine(libraryRoot, "Extras");

            // Create extras folder if it doesn't already exist
            Directory.CreateDirectory(extrasPath);
        }
        public void FolderPicker()
        {
            //TODO possible to select and move multiple? 
            //TODO: allow picking files and folders here? 
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

            MoveFolderToExtras(new DirectoryInfo(dialog.SelectedPath));
        }

        private void MoveFolderToExtras(DirectoryInfo di)
        {
            string extrasRootFolder = Path.Combine(extrasPath, di.Parent.Name);
            Directory.CreateDirectory(extrasRootFolder);

            string newFolder = Path.Combine(extrasRootFolder, di.Name);

            Directory.Move(di.FullName, newFolder);
        }

        // search folders recursively for extras/ featurettes folders
        // and add to ignoredFolders
        public void Search()
        {
            ignoredFolders = new ObservableCollection<string>();
            FileSystemSearcher searcher = new FileSystemSearcher();

            UserSettingsManager userSettingsManager = new UserSettingsManager();

            List<string> extras = searcher.FindExtrasFolders(new DirectoryInfo(userSettingsManager.settings.RootFilePath));

            foreach(string extrasPath in extras) 
            { 
                ignoredFolders.Add(extrasPath);
            }
        }

        public void Commit()
        {
            foreach(string folder in ignoredFolders)
            {
                MoveFolderToExtras(new DirectoryInfo(folder));
            }
        }
    }
}
