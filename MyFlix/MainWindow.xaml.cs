using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace MyFlix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string rootFilePath = @"F:\Videos\";

        public MainWindow()
        {
            InitializeComponent();

            FileSystemSearcher searcher = new FileSystemSearcher();
            List<string> videos = searcher.GetVideosInDirRecursively(rootFilePath);

            AddMultipleObjectsToListBox(videos.ToArray(), lstVideos);

            lstVideos.Items.Add("Test");
        }

        private void AddMultipleObjectsToListBox(object[] list, ListBox control)
        {
            foreach(object item in list)
            {
                control.Items.Add(item);
            }
        }

        private void SetMediaFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new() { 

            };
            //dialog.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                rootFilePath = dialog.SelectedPath;
            }


        }
    }
}
