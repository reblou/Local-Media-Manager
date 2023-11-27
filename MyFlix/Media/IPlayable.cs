namespace MyFlix
{
    public interface IPlayable
    {
        string title { get; set; }
        string filePath { get; set;  }
        string fileName { get; set; }
        public bool watched { get; set; }

        public string ToString();
    }
}
