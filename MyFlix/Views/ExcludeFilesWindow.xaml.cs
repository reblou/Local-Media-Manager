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
    /// Interaction logic for ExcludeFilesWindow.xaml
    /// </summary>
    public partial class ExcludeFilesWindow : Window
    {
        public List<string> ExcludedPaths { get; set; }

        public ExcludeFilesWindow()
        {
            ExcludedPaths = new List<string>() { "Test1", "test2" };
            this.DataContext = ExcludedPaths;
            InitializeComponent();
        }
    }
}
