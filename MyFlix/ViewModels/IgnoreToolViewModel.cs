using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
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
        public ICommand removeFolderCommand { get; }

        public List<string> ignoredFolders { get; set; }
        public string extrasPath { get; set; }

        public IgnoreToolViewModel()
        {
            folderPickerCommand = new RelayCommand(FolderPicker);
            removeFolderCommand = new RelayCommand(RemoveFolder);

            ignoredFolders = new List<string>() { "test1", "test2", "test3" };
            //extrasPath = Path.Combine(libraryRootFolder, "Extras");
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

            DirectoryInfo di = new DirectoryInfo(dialog.SelectedPath);
            MoveFolder(di.FullName, Path.Combine(extrasPath, di.Name));
        }

        public void RemoveFolder()
        {
            //TODO: store old path? or use folder picker to move back? 
        }

        private void MoveFolder(string folder, string newFolder)
        {
            if (String.IsNullOrEmpty(folder) || String.IsNullOrEmpty(newFolder)) return;

            
            Directory.Move(folder, newFolder);
        }
    }
}
