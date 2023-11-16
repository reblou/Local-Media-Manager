namespace MyFlix
{
    public interface IPlayable
    {
        string title { get; set; }
        string filePath { get; set;  }
        public string ToString();
    }
}
