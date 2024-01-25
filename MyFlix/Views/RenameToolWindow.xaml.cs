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
    /// Interaction logic for RenameToolWindow.xaml
    /// </summary>
    public partial class RenameToolWindow : Window
    {
        public RenameToolWindow()
        {
            InitializeComponent();
        }

        private void TextBox_TextInput(object sender, TextCompositionEventArgs e)
        {

        }
    }
}
