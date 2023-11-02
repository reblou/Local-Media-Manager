using MyFlix.Player;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace MyFlix
{
    /// <summary>
    /// Interaction logic for PlayWindow.xaml
    /// </summary>
    public partial class PlayWindow : Window
    {
        private IPlayable playable;
        private bool fullscreen = false;
        private bool playing = false;

        public PlayWindow(IPlayable playable)
        {
            InitializeComponent();
            this.playable = playable;
            mediaPlayer.Source = new Uri(this.playable.filePath);

            PlayToggle();
            this.DataContext = new PlayViewModel();
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            PlayToggle();
        }

        private void PlayToggle()
        {
            if (playing)
            {
                this.mediaPlayer.Pause();
            }
            else
            {
                this.mediaPlayer.Play();
            }

            playing = !playing;
        }

        public void Close(object sender, CancelEventArgs e)
        {
            this.mediaPlayer.Stop();

            //TODO: track progress here
        }

        private void Fullscreen_Click(object sender, RoutedEventArgs e)
        {
            if(!fullscreen)
            {
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.WindowState = WindowState.Normal;
            }
            fullscreen = !fullscreen;
        }
    }
}
