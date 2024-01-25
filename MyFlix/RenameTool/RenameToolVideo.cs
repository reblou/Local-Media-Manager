using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix.RenameTool
{
    public class RenameToolVideo : INotifyPropertyChanged
    {
        private string _fileName;
        public string FileName { get => _fileName; set { _fileName = value; OnPropertyChanged(); } }

        private string _newName;
        public string NewName { get => _newName; set { _newName = value; OnPropertyChanged(); } }
        public string filePath { get; set; }
        public string oldName;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            if (PropertyChanged == null) return;
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
