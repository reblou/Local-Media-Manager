using System.ComponentModel;

namespace MyFlix
{
    public interface IPlayable : INotifyPropertyChanged
    {
        string title { get; set; }
        string filePath { get; set;  }
        string fileName { get; set; }
        public bool BeenWatched { get; set; }

        public string ToString();
    }
}
