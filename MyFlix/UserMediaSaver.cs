using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix
{
    internal static class UserMediaSaver
    {
        private static readonly string filename = "user-media.json";

        public static void SaveToFile(List<IDisplayable> media)
        {
            UserMediaSaveData saveData = new UserMediaSaveData(media);

            string output = JsonConvert.SerializeObject(saveData);

            using StreamWriter writer = new StreamWriter(filename);
            writer.Write(output);
        }
        public static List<IDisplayable> LoadFromFile()
        {
            try
            {
                return TryToLoadFromFile();
            }
            catch (FileNotFoundException)
            {
                return new List<IDisplayable>();
            }
        }

        public static void SavePlayable(IPlayable playable)
        {
            List<IDisplayable> displayables = LoadFromFile();

            foreach(IDisplayable displayable in displayables)
            {
                if(displayable.RepresentsFilename(playable.fileName))
                {
                    displayable.SetPlayable(playable);
                    break;
                }
            }
            SaveToFile(displayables);
        }

        public static void SaveDisplayable(IDisplayable newDisplayable)
        {
            List<IDisplayable> displayables = LoadFromFile();

            for (int i = 0; i < displayables.Count; i++)
            {
                if (displayables[i].title == newDisplayable.title)
                {
                    displayables[i] = newDisplayable;
                    break;
                }
            }
            SaveToFile(displayables);
        }

        public static void WipeData()
        {
            SaveToFile(new List<IDisplayable>());
        }

        private static List<IDisplayable> TryToLoadFromFile()
        {
            string data = "";
            using (StreamReader reader = new StreamReader(filename))
            {
                data = reader.ReadToEnd();
            }

            UserMediaSaveData saveData = JsonConvert.DeserializeObject<UserMediaSaveData>(data);
            return saveData.GetCombinedList();
        }
    }
}
