﻿using MyFlix.Player;
using MyFlix.ViewModels;
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
        private PlayWindow playWindow;

        private double scrollOffset;
        private FileInfoWindow fileInfoWindow;
        MediaDetailsViewModel viewModel;


        public MediaDetailsView(IDisplayable video, double scrollOffset)
        {
            this.viewModel = new MediaDetailsViewModel(video);
            this.DataContext = viewModel;
            this.scrollOffset = scrollOffset;
            this.Title = video.title;

            InitializeComponent();

            if (String.IsNullOrEmpty(this.viewModel.displayable.releaseYear))
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
            //TODO: error handling, what if we don't have a video to play? Is that possible

            IPlayable playable = viewModel.displayable.GetNextPlayable();

            OpenPlayWindow(playable);
        }

        private void FileInfo_Click(object sender, RoutedEventArgs e)
        {
            if(fileInfoWindow != null)
            {
                fileInfoWindow.Activate();
                return;
            }
            fileInfoWindow = new FileInfoWindow(viewModel.displayable.GetNextPlayable());

            fileInfoWindow.Show();
        }

        private void File_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Label label = sender as Label;
            IPlayable playable = label.DataContext as IPlayable; 

            if (playable != null)
            {
                OpenPlayWindow(playable);
            }
        }

        private void OpenPlayWindow(IPlayable video)
        {
            if (playWindow == null)
            {
                playWindow = new PlayWindow(video);

                playWindow.Closed += (sender, args) => 
                {
                    PlayWindow playWindow = sender as PlayWindow;

                    UserMediaSaver.SavePlayable(playWindow.playable);
                    this.playWindow = null;
                };
                playWindow.Show();
            }
        }
    }
}
