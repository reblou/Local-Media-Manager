using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Windows.Documents;

namespace MyFlix
{
    public interface IDisplayable
    {
        public string title { get; set; }
        public string description { get; set; }
        public string posterURL { get; set; }
        public string backdropURL { get; set; }
        public string releaseYear { get; set; }

        public void LookupDetails(TMDBApiHandler handler);
        public bool RepresentsFilename(string filename);

        public IPlayable GetNextPlayable();

        public List<IPlayable> GetPlayables();
    }
}