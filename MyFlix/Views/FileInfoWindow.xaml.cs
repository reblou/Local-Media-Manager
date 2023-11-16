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
using System.Windows.Shapes;

namespace MyFlix.Views
{
    /// <summary>
    /// Interaction logic for FileInfoWindow.xaml
    /// </summary>
    public partial class FileInfoWindow : Window
    {
        IPlayable playable;

        public FileInfoWindow(IPlayable playable)
        {
            InitializeComponent();
            this.playable = playable;
            this.DataContext = playable;
        }
    }
}
