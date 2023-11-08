using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix.Player
{
    internal class ProgressSaver
    {
        private const string fileName = "user-progress.json";
        Dictionary<string, TimeSpan> playableProgress;

        public ProgressSaver()
        {
            LoadFromFile();
        }

        public void SaveProgress(string title, TimeSpan progress)
        {
            playableProgress[title] = progress;
            SaveToFile();
        }

        public TimeSpan GetProgress(string title)
        {
            TimeSpan progress = new TimeSpan();
            playableProgress.TryGetValue(title, out progress);
            return progress;

        }

        void SaveToFile()
        {
            string output = JsonConvert.SerializeObject(playableProgress);

            using StreamWriter writer = new StreamWriter(fileName);
            writer.Write(output);
        }

        void LoadFromFile()
        {
            try
            {
                TryToLoadFromFile();
            }
            catch (FileNotFoundException)
            {
                playableProgress = new Dictionary<string, TimeSpan>();
            }
        }

        void TryToLoadFromFile()
        {
            string data = "";
            using (StreamReader reader = new StreamReader(fileName))
            {
                data = reader.ReadToEnd();
            }

            playableProgress = JsonConvert.DeserializeObject<Dictionary<string, TimeSpan>>(data);
        }
    }
}
