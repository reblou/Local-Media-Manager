using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix.Lookup.Parser
{
    public class SeriesInfoParserState : ParserState
    {
        ParserState nextState;
        public SeriesInfoParserState(Parser parser) : base(parser)
        {

        }

        public override void ParseWord(string word)
        {
            // Check for year
            if (IsYear(word))
            {
                nextState = new YearParserState(this.parser);
            }
            else if (IsJunk(word))
            {
                nextState = new JunkParserState(this.parser);
            }

            if(nextState != null)
            {
                this.parser.ChangeState(nextState);

                nextState.ParseWord(word);
            }


            // Otherwise we assume this is an episode.

            this.parser.parsedInfo.isEpisode = true;
        }
    }
}
