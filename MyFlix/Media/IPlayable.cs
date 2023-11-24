namespace MyFlix
{
    public interface IPlayable
    {
        string title { get; set; }
        string filePath { get; set;  }
        //TODO: add filename
        string fileName { get; set; }
        public string ToString();
    }
}
