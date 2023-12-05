using MyFlix.Player;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        public IPlayable playable;
        private bool fullscreen = false;
        private bool playing = false;
        private bool dragging = false;
        ProgressSaver progressSaver;

        BackgroundWorker progressUpdateWorker;
        Timer controlHideTimer;

        const int SkipDurationSecs = 10;

        public static RoutedCommand fullScreenCommand = new RoutedCommand();
        public static RoutedCommand seekForwardCommand = new RoutedCommand();
        public static RoutedCommand seekBackCommand = new RoutedCommand();

        public PlayWindow(IPlayable playable)
        {
            //TODO: error handling on opening for invalid video for e.g. might have moved
            InitializeComponent();
            this.playable = playable;
            this.progressSaver = new ProgressSaver();
            mediaPlayer.Source = new Uri(this.playable.filePath);
            mediaPlayer.Loaded += LoadProgress;

            this.Title = playable.fileName;

            progressUpdateWorker = new BackgroundWorker()
            {
                WorkerSupportsCancellation = true
            };
            progressUpdateWorker.DoWork += ProgressUpdateWorker_DoWork;

            controlHideTimer = new Timer(HideControls, null, Timeout.Infinite, Timeout.Infinite);
        }

        private void ProgressUpdateWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            while(!progressUpdateWorker.CancellationPending)
            {
                timeline.Dispatcher.BeginInvoke(SetThumbToMediaPercentage, System.Windows.Threading.DispatcherPriority.SystemIdle);

                Thread.Sleep(2000);
            }
        }

        private void SetThumbToMediaPercentage()
        {
            if (!mediaPlayer.NaturalDuration.HasTimeSpan || dragging) return;

            // check video progress and set slidebar
            TimeSpan progress = mediaPlayer.Position;
            TimeSpan fullLength = mediaPlayer.NaturalDuration.TimeSpan;

            timeline.Value = progress / fullLength;
        }
        private void LoadProgress(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Position = progressSaver.GetProgress(playable.filePath);

            PlayToggle();
            SetThumbToMediaPercentage();

            progressUpdateWorker.RunWorkerAsync();
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
            progressUpdateWorker.CancelAsync();
            double watchPercentage = GetWatchPercentage();
            if(watchPercentage > 0.9)
            {
                progressSaver.RemoveProgress(playable.filePath);
                playable.BeenWatched = true;
            }
            else if(watchPercentage < 0.05)
            {
                progressSaver.RemoveProgress(playable.filePath);
                playable.BeenWatched = false;
            }
            else
            { 
                progressSaver.SaveProgress(playable.filePath, mediaPlayer.Position);
            }

            this.mediaPlayer.Stop();
        }

        private double GetWatchPercentage()
        {
            if (!mediaPlayer.IsLoaded) return 0;

            return mediaPlayer.Position / mediaPlayer.NaturalDuration.TimeSpan;
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

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            PlayControls.Visibility = Visibility.Visible;
            this.Cursor = Cursors.Arrow;
            // Set timer to hide play controls
            // Disable any active timer
            controlHideTimer.Change(2000, Timeout.Infinite);
        }

        private void HideControls(object? state)
        {
            PlayControls.Dispatcher.BeginInvoke(new Action(() =>
            {
                PlayControls.Visibility = Visibility.Hidden;
                this.Cursor = Cursors.None;
            }));
        }

        private void TogglePlayPauseBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            PlayToggle();
        }

        private void TogglePlayPauseBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayer.IsLoaded && !dragging;
        }

        private void ExecutedFullscreenCommand(object sender, ExecutedRoutedEventArgs e)
        {
            if (!fullscreen)
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

        private void CanExecuteFullscreenCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }


        private void ExecutedSeekForwardCommand(object sender, ExecutedRoutedEventArgs e)
        {
            mediaPlayer.Position += TimeSpan.FromSeconds(SkipDurationSecs);
        }

        private void CanExecuteSeekForwardCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayer.IsLoaded;
        }

        private void ExecutedSeekBackCommand(object sender, ExecutedRoutedEventArgs e)
        {
            mediaPlayer.Position -= TimeSpan.FromSeconds(SkipDurationSecs);
        }

        private void CanExecuteSeekBackCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = mediaPlayer.IsLoaded;
        }
    }
}
