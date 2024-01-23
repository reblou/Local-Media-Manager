using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix.Lookup.Parser
{
    public class YearParserState : ParserState
    {
        StringBuilder reserveTitle;

        public YearParserState(Parser parser) : base(parser)
        {
            reserveTitle = new StringBuilder();
        }

        public override void EndParsing()
        {
            //Do nothing
            return;
        }

        public override void ParseWord(string word)
        {

            if (IsSeriesInfo(word))
            {
                this.parser.ChangeState(new SeriesInfoParserState(this.parser), word);
            }
            else if (IsJunk(word))
            {
                this.parser.ChangeState(new JunkParserState(this.parser), word);
            }
            else if (IsYear(word)) 
            {
                // if we've already found the year 
                if(!String.IsNullOrEmpty(this.parser.parsedInfo.year))
                {
                    // we want to re add reserve words back to title, the first year was actually part of the title.
                    if(String.IsNullOrEmpty(this.parser.parsedInfo.title))
                    {
                        this.parser.parsedInfo.title += reserveTitle.ToString();
                    }
                    else
                    {
                        // append space before the reserve words if title isn't empty
                        this.parser.parsedInfo.title += ' ' + reserveTitle.ToString();
                    }
                    this.parser.parsedInfo.year = word;
                }

                this.parser.parsedInfo.year = word;
                reserveTitle.Append(word);
            }
            else
            {
                // Store any uncategorized words in case we find a second year
                reserveTitle.Append(' ');
                reserveTitle.Append(word);
            }
        }
    }
}
