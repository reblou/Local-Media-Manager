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

        public MediaDetailsView(IDisplayable video)
        {
            this.video = video;
            this.DataContext = video;
            InitializeComponent();
        }

        private void ReturnButtonClicked(object sender, RoutedEventArgs e)
        {
            NavigationService ns = this.NavigationService;
            ns.Navigate(new TileView());
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
    }
}
