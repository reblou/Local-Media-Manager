using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MyFlix
{
    public class UserSettings
    {
        public string RootFilePath;
    }

    internal class UserSettingsManager
    {
        public UserSettings settings { get; set; }
        private readonly string configFileName = "user_settings.xml";

        XmlSerializer xmlSerializer;
        public UserSettingsManager()
        {
            xmlSerializer = new XmlSerializer(typeof(UserSettings));
            ReadSettingsFromFile();
        }

        public void ReadSettingsFromFile()
        {
            settings = new UserSettings();

            //TODO: errorhandling - no file, invalid file etc
            using (var reader = new StreamReader(configFileName))
            {
                settings = (UserSettings) xmlSerializer.Deserialize(reader);
            }
            if (settings == null) throw new NullReferenceException("Couldn't read user settings");
        }

        public void WriteSettingsToFile()
        {
            // write settings to file
            xmlSerializer.Serialize(new StreamWriter(configFileName, false), settings);
        }

        public void SetRootFilePath(string rootFilePath)
        {
            settings.RootFilePath = rootFilePath;
            WriteSettingsToFile();
        }
    }
}
