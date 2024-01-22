using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix.Lookup.Parser
{
    public class ParsedInformation
    {
        public string title;
        public string year;
        public bool isEpisode;

        public ParsedInformation()
        {
            this.title = "";
            this.isEpisode = false;
        }
    }
}
