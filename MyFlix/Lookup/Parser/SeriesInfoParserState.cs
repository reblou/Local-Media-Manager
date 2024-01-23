using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix.Lookup.Parser
{
    public class SeriesInfoParserState : ParserState
    {
        bool dashPredicate;

        public SeriesInfoParserState(Parser parser) : base(parser)
        {
            dashPredicate = false;
        }
        public override void EndParsing()
        {
            //Do nothing
            return;
        }

        public override void ParseWord(string word)
        {

            if ((dashPredicate && IsEpisodeNumber(word)) || IsSeriesInfo(word))
            {
                this.parser.parsedInfo.isEpisode = true;
            }
            else if (IsYear(word)) 
            {
                this.parser.ChangeState(new YearParserState(this.parser), word);
            }
            else if (IsJunk(word))
            {
                this.parser.ChangeState(new JunkParserState(this.parser), word);
            }
            else if (word == "-")
            {
                dashPredicate = true;
            }
        }

        private bool IsEpisodeNumber(string word)
        {
            int i = 0;
            return int.TryParse(word, out i);
        }
    }
}
