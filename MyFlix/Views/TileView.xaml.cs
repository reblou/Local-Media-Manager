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
using MyFlix.Views;

namespace MyFlix
{
    /// <summary>
    /// Interaction logic for TileView.xaml
    /// </summary>
    public partial class TileView : Page
    {
        string rootFilePath = "";
        UserSettingsManager userSettingsManager;
        public MediaList videos = new MediaList();

        public TileView(double scrollOffset) : this()
        {
            this.scrollViewer.ScrollToVerticalOffset(scrollOffset);
        }

        public TileView()
        {
            InitializeComponent();
            mediaItemsControl.ItemsSource = videos;

            userSettingsManager = new UserSettingsManager();
            rootFilePath = userSettingsManager.settings.RootFilePath;
            videos.LoadMediaFromDirectoryRecursively(rootFilePath);
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

            videos.Clear();
            videos.LoadMediaFromDirectoryRecursively(rootFilePath);
        }

        private void ExcludeFolder_Click(object sender, RoutedEventArgs e)
        {
            ExcludeFilesWindow excludeWindow = new ExcludeFilesWindow();

            excludeWindow.Show();
        }

        private void WipeMediaData_Click(object sender, RoutedEventArgs e)
        {
            // wipe save data
            userSettingsManager.WipeSettings();

            // clear media details items
            videos.Wipe();
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            videos.Clear();
            videos.LoadMediaFromDirectoryRecursively(rootFilePath);
        }
        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            IDisplayable clickedVideo = (IDisplayable)button.DataContext;

            // navigate to details view & send video
            NavigationService ns = this.NavigationService;
            MediaDetailsView detailsPage = new MediaDetailsView(clickedVideo, this.scrollViewer.VerticalOffset);

            ns.Navigate(detailsPage);
        }
    }
}
