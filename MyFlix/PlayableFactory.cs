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

        public static IPlayable CreatePlayableFromFilename(string filename)
        {
            FilenameParser parser = new FilenameParser();
            parser.ParseFilename(Path.ChangeExtension(filename, ""));

            if(parser.season != -1 || parser.episode != -1)
            {
                return new Episode();
            }


            return new Film();
        }
    }
}
