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
        public MediaList videos = new MediaList();

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

        private void Tile_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            IDisplayable clickedVideo = (IDisplayable)button.DataContext;

            // navigate to details view & send video
            NavigationService ns = this.NavigationService;
            MediaDetailsView detailsPage = new MediaDetailsView(clickedVideo);
            ns.Navigate(detailsPage);
        }
    }
}
