using MyFlix.Player;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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
        private bool dragging = false;
        ProgressSaver progressSaver;

        public PlayWindow(IPlayable playable)
        {
            InitializeComponent();
            this.playable = playable;
            this.progressSaver = new ProgressSaver();
            mediaPlayer.Source = new Uri(this.playable.filePath);
            mediaPlayer.Loaded += LoadProgress;

            this.DataContext = new PlayViewModel();

        }

        private void CheckMediaProgress()
        {
            if(mediaPlayer.NaturalDuration.HasTimeSpan && !dragging)
            {
                // check video progress and set slidebar
                TimeSpan progress = mediaPlayer.Position;
                TimeSpan fullLength = mediaPlayer.NaturalDuration.TimeSpan;

                timeline.Value = progress / fullLength;
            }

            Thread.Sleep(1000);
            timeline.Dispatcher.Invoke(CheckMediaProgress, System.Windows.Threading.DispatcherPriority.SystemIdle);
        }

        private void LoadProgress(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Position = progressSaver.GetProgress(playable.filePath);

            PlayToggle();


            timeline.Dispatcher.Invoke(CheckMediaProgress, System.Windows.Threading.DispatcherPriority.SystemIdle);
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
            this.progressSaver.SaveProgress(playable.filePath, mediaPlayer.Position);

            this.mediaPlayer.Stop();
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

        private void timeline_Drag(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            Console.Write(mediaPlayer.Position);
        }

        public void tileline_DragEnter(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            this.dragging = true;
        }
        public void tileline_DragLeave(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            this.dragging = false;
            Slider slider = sender as Slider;
            SetMediaPositionToValue(slider.Value);
        }

        private void timeline_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            SetMediaPositionToValue(slider.Value);
        }

        private void SetMediaPositionToValue(double value)
        {
            if (dragging) return;

            TimeSpan fullLength = mediaPlayer.NaturalDuration.TimeSpan;

            TimeSpan test = fullLength * value;

            mediaPlayer.Position = test;
        }
    }
}
