using MyFlix.Views;
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

namespace MyFlix
{
    /// <summary>
    /// Interaction logic for MediaDetailsView.xaml
    /// </summary>
    public partial class MediaDetailsView : Page
    {
        public IDisplayable video { get; set; }

        private PlayWindow playWindow;
        private double scrollOffset;
        private FileInfoWindow fileInfoWindow;


        public MediaDetailsView(IDisplayable video, double scrollOffset)
        {
            this.video = video;
            this.DataContext = video;
            this.scrollOffset = scrollOffset;

            InitializeComponent();

            if (String.IsNullOrEmpty(this.video.releaseYear))
            {
                this.releaseYear.Visibility = Visibility.Hidden;
            }

        }

        private void ReturnButtonClicked(object sender, RoutedEventArgs e)
        {
            NavigationService ns = this.NavigationService;

            if(fileInfoWindow != null) fileInfoWindow.Close();
            if (playWindow != null) playWindow.Close();
            ns.Navigate(new TileView(scrollOffset));
        }

        private void PlayButton_Clicked(object sender, RoutedEventArgs e)
        {
            if(playWindow == null)
            {
                playWindow = new PlayWindow(video.GetPlayable());

                //TODO: closed callback function -> update storage with video progress
                playWindow.Closed += (sender, args) => this.playWindow = null;
                playWindow.Show();
            }
        }

        private void FileInfo_Click(object sender, RoutedEventArgs e)
        {
            if(fileInfoWindow != null)
            {
                fileInfoWindow.Activate();
                return;
            }
            fileInfoWindow = new FileInfoWindow(video.GetPlayable());

            fileInfoWindow.Show();
        }
    }
}
