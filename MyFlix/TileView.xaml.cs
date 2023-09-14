using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace MyFlix
{
    /// <summary>
    /// Interaction logic for TileView.xaml
    /// </summary>
    public partial class TileView : Page
    {
        string rootFilePath = "";
        UserSettingsManager userSettingsManager;
        //TODO: sort alphabetically? 
        public MediaList videos = new();
        BackgroundWorker fileSearcherWorker;

        public TileView()
        {
            InitializeComponent();

            userSettingsManager = new UserSettingsManager();
            rootFilePath = userSettingsManager.settings.RootFilePath;

            mediaItemsControl.ItemsSource = videos;

            fileSearcherWorker = new BackgroundWorker();
            fileSearcherWorker.DoWork += RetrieveMedia;
            //TODO: report progress and add videos one at a time? 
            fileSearcherWorker.WorkerReportsProgress = false;
            fileSearcherWorker.RunWorkerCompleted += MediaRetrieved;

            fileSearcherWorker.RunWorkerAsync();
        }

        private void RetrieveMedia(object sender, DoWorkEventArgs e)
        {
            //TODO: Disable set folder option in navbar
            FileSystemSearcher searcher = new();
            searcher.GetVideosInDirRecursively(rootFilePath);

            // send searcher.vidoes back to UI thread
            e.Result = searcher.videos;
        }

        private void MediaRetrieved(object sender, RunWorkerCompletedEventArgs e)
        {
            List<Video> foundVideos = (List <Video>) e.Result;

            videos.AddList(foundVideos);
            //TODO: re enable set folder option
        }

        private void ClearMedia()
        {
            videos.Clear();
        }

        private void SetMediaFolder_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new()
            {
                //options here
            };

            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                rootFilePath = dialog.SelectedPath;
                userSettingsManager.SetRootFilePath(rootFilePath);
            }

            ClearMedia();
            //PopulateMedia();
            fileSearcherWorker.RunWorkerAsync();
        }

        private void lstVideos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Launch media player
            Video video = (Video)((ListBox)sender).SelectedItem;


            //PlayVideo(video);
        }

        private void PlayVideo(Video video)
        {
            try
            {
                mediaElement.Source = new Uri(video.filePath);
            }
            catch (NullReferenceException) { }
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            NavigationService nav = NavigationService.GetNavigationService(this);
            nav.Navigate(new Uri("MediaDetailsView.xaml", UriKind.RelativeOrAbsolute));
        }

        private void Tile_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
