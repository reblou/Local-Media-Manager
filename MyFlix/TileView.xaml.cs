using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public MediaList videos = new();

        public TileView()
        {
            InitializeComponent();


            userSettingsManager = new UserSettingsManager();
            rootFilePath = userSettingsManager.settings.RootFilePath;
            PopulateMedia();

            mediaItemsControl.ItemsSource = videos;
        }

        private void PopulateMedia()
        {
            //TODO: do this on another thread.
            FileSystemSearcher searcher = new();
            searcher.GetVideosInDirRecursively(rootFilePath);

            videos.AddList(searcher.videos);
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
            PopulateMedia();
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
