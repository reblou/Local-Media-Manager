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
            FilenameParser parser = new FilenameParser();
            parser.ParseFilename(Path.ChangeExtension(filename, ""));

            if(parser.episode != -1)
            {
                int season = parser.season == -1 ? 1 : parser.season;
                return new Episode(parser.title, parser.releaseYear, season, parser.episode, filename, filepath);
            }


            return new Film(filename, filepath, parser.title, parser.releaseYear);
        }
    }
}
