using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix.Lookup.Parser
{
    public class TitleParserState : ParserState
    {
        string title;
        public TitleParserState(Parser parser) : base(parser)
        {
            title = "";
        }

        public override void ParseWord(string word)
        {

            //check if year or season episode info
            if(IsYear(word))
            {
                this.parser.parsedInfo.title = this.title;
                this.parser.ChangeState(new YearParserState(this.parser), word);
            }
            else if (IsSeriesInfo(word) || word == "-")
            {                
                this.parser.parsedInfo.title = this.title;
                this.parser.ChangeState(new SeriesInfoParserState(this.parser), word);
            }
            else if (IsJunk(word))
            {
                // assume we're at the end of useful information
                this.parser.parsedInfo.title = this.title;
                this.parser.ChangeState(new JunkParserState(this.parser), word);
            }
            else if (String.IsNullOrEmpty(title)) // Otherwise assume this is part of the title
            {
                title += word;
            }
            else
            {
                this.title += ' ' + word;
            }
        }

        public override void EndParsing()
        {
            //if we are still in TitleState at the end of the word->Save title.
            this.parser.parsedInfo.title = this.title;
        }

    }
}
