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
        public MediaList videos = new MediaList();

        public TileView(double scrollOffset) : this()
        {
            this.scrollViewer.ScrollToVerticalOffset(scrollOffset);
        }

        public TileView()
        {
            InitializeComponent();
        }
    }
}
