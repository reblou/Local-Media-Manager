using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix.Lookup.Parser
{
    public class JunkParserState : ParserState
    {
        public JunkParserState(Parser parser) : base(parser)
        {
        }

        public override void ParseWord(string word)
        {
            // Do nothing
            return;
        }
        public override void EndParsing()
        {
            //Do nothing
            return;
        }
    }
}
