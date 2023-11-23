namespace MyFlix
{
    public interface IPlayable
    {
        string title { get; set; }
        string filePath { get; set;  }
        //TODO: add filename
        public string ToString();
    }
}
