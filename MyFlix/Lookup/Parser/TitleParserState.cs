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
        ParserState nextState;
        public TitleParserState(Parser parser) : base(parser)
        {
            title = "";
        }

        public override void ParseWord(string word)
        {

            //check if year or season episode info
            if(IsYear(word))
            {
                nextState = new YearParserState(this.parser);
            }
            else if (IsSeriesInfo(word))
            {                
                nextState = new SeriesInfoParserState(this.parser);
            }
            else if (IsJunk(word))
            {
                // assume we're at the end of useful information
                nextState = new JunkParserState(this.parser);
            }

            if(nextState != null)
            {
                // if non title word detected, set title in parser and change its state.
                this.parser.parsedInfo.title = this.title;
                this.parser.ChangeState(nextState);
                nextState.ParseWord(word);

                return;
            }

            // TODO: If we never change state we never save the title, need condition that if we are still in TitleState at the end of the word -> Save title.

            // Otherwise assume this is part of the title
            if (String.IsNullOrEmpty(title))
            {
                title += word;
            }
            else
            {
                this.title += ' ' + word;
            }
        }

    }
}
