using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix.Lookup.Parser
{
    public class Parser
    {
        ParserState state;
        public ParsedInformation parsedInfo;

        string filename;
        string workingFilename;

        public Parser()
        {
            // set to title state initialy
            parsedInfo = new ParsedInformation();
            state = new TitleParserState(this);
        }

        public ParsedInformation ParseFilename(string filename)
        {
            this.workingFilename = filename;

            while(this.workingFilename.Length > 0)
            {
                string word = GetNextWord();

                this.state.ParseWord(word);
            }

            this.state.EndParsing();

            return parsedInfo;
        }

        public void ChangeState(ParserState state, string word)
        {
            this.state = state;

            // reprocess the word using the next state
            this.state.ParseWord(word);
        }


        private string GetNextWord()
        {
            StringBuilder sb = new StringBuilder();

            bool squareBrackets = false;
            int? splitIndex = null;
            for(int i=0; i<workingFilename.Length; i++)
            {
                // ignore anything inside square brackets.
                if (squareBrackets)
                {
                    if (workingFilename[i] == ']') squareBrackets = false;

                    continue;
                }
                else if (workingFilename[i] == '[')
                {
                    squareBrackets = true;
                    continue;
                }

                if (ParseSettings.wordDivider.Contains(workingFilename[i]))
                {
                    // If only whitespace/ ignored brackets so far, continue until we have a word
                    if(sb.Length == 0)
                    {
                        continue;
                    }
                    else
                    {
                        //set split index for working filename to after witespace
                        splitIndex = ++i;
                        break;
                    }
                } 

                sb.Append(workingFilename[i]);
            }
            //if we reached end of string so splitIndex is null, spit on length which returns empty string.
            this.workingFilename = workingFilename.Substring(splitIndex ?? workingFilename.Length);

            return sb.ToString();
        }
    }
}
