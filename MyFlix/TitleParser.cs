using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyFlix
{
    internal class TitleParser
    {
        bool squareBrackets = false;
        string currentWord = "";

        public string title = "";
        public string releaseYear = "";

        public void ParseTitleFromFilename(string filename)
        {
            foreach (char c in filename)
            {
                // ignore anything contained in []
                if (squareBrackets) 
                {
                    if (c == ']') squareBrackets = false;

                    continue;
                }
                else if (c == '[')
                {
                    squareBrackets = true;
                }
                else if (isIgnoredChar(c))
                {
                    continue;
                }
                else if (isWhitespaceChar(c))
                {
                    if (WordIsYear(currentWord) && title.Length > 0)
                    {
                        releaseYear = currentWord;
                        // We have full title hopefully

                        // TODO: check rest of title for another year? 

                        title.Trim();
                        return;
                    }
                    title += currentWord + " ";
                    currentWord = "";
                }
                else
                {
                    currentWord += c;
                }

            }

            title += currentWord;
            title.Trim();
        }

        public static bool WordIsYear(string word)
        {
            Regex re = new Regex(@"[0-9]{4}");
            return re.IsMatch(word);
        }

        private static bool isIgnoredChar(char c)
        {
            return c == '(' || c == ')';
        }

        private static bool isWhitespaceChar(char c)
        {
            return c == ' ' || c == '.' || c == '_';
        }
    }
}
