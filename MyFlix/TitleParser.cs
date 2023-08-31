using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyFlix
{
    internal static class TitleParser
    {
        public static string ParseTitleFromFilename(string filename)
        {
            bool squareBrackets = false;
            string title = "";

            string currentWord = "";
            foreach (char c in filename)
            {
                if (squareBrackets) // ignore anything contained in []
                {
                    if (c == ']') squareBrackets = false;

                    continue;
                }
                else if (c == '[')
                {
                    squareBrackets = true;
                }
                else if (c == ' ' || c == '.' || c == '_')
                {
                    currentWord += ' ';
                    if (WordIsYear(currentWord) && title.Length > 0)
                    {
                        // We have full title hopefully
                        title += currentWord;
                        break;
                    }
                    title += currentWord;
                    currentWord = "";
                }
                else if (c == '(' || c == ')')
                {
                    continue;
                }
                else
                {
                    currentWord += c;
                }
            }

            return title;
        }
        public static bool WordIsYear(string word)
        {
            Regex re = new Regex(@"[0-9]{4}");
            return re.IsMatch(word);
        }
    }
}
