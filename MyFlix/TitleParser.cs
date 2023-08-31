using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyFlix
{
    internal class FilmTitleParser
    {
        bool squareBrackets;

        public string title;
        string filename;

        public FilmTitleParser(string filename)
        {
            squareBrackets = false;
            brackets = false;
            this.filename = filename;
            title = "";
        }

        public void ParseTitle()
        {
            // replace . ( [ etc with space
            //

            string currentWord = "";
            foreach (char c in filename)
            {
                if (squareBrackets)
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
                        // We have title hopefully
                        title += currentWord;
                        break;
                    }
                    title += currentWord;
                    currentWord = "";
                }
                else if (c=='(' || c==')')
                {
                    continue;
                }
                else
                {
                    currentWord += c;
                }
            }
        }

        public bool WordIsYear(string word)
        {
            Regex re = new Regex(@"[0-9]{4}");
            return re.IsMatch(word);
        }
    }

    internal static class TitleParser
    {
        public static string ParseTitleFromFilename(string filename)
        {
            FilmTitleParser parser = new FilmTitleParser(filename);
            parser.ParseTitle();

            return parser.title;
        }
    }
}
