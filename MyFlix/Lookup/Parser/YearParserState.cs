using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix.Lookup.Parser
{
    public class YearParserState : ParserState
    {
        ParserState nextState;

        public YearParserState(Parser parser) : base(parser)
        {
        }

        public override void ParseWord(string word)
        {

            if (IsSeriesInfo(word))
            {
                nextState = new SeriesInfoParserState(this.parser);

                this.parser.ChangeState(nextState);

                nextState.ParseWord(word);
            }
            else if (IsJunk(word))
            {
                this.parser.ChangeState(new JunkParserState(this.parser));

                return;
            }
            else if (IsYear(word)) 
            {
                // TODO: handle if we've already found the year 

                this.parser.parsedInfo.year = word;
            }
        }
    }
}
