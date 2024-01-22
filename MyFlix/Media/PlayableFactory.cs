using MyFlix.Lookup.Parser;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix
{
    class PlayableFactory
    {
        public static IPlayable CreatePlayableFromFilename(string filename, string filepath)
        {
            Parser parser = new Parser();
            ParsedInformation parsedInfo = parser.ParseFilename(Path.ChangeExtension(filename, ""));

            if(parsedInfo.isEpisode)
            {
                return new Episode(parsedInfo.title, parsedInfo.year, filename, filepath);
            }
            else
            {
                return new Film(filename, filepath, parsedInfo.title, parsedInfo.year);
            }
        }
    }
}
