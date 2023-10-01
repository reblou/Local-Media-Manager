using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyFlix
{
    internal class TvTitleParser
    {
        public string title = "";
        public string releaseYear = "";
        public int season;
        public int episode;

        bool squareBrackets = false;
        string currentWord = "";
        bool readingSeason = false;
        string strSeason = "";
        bool readingEpisode = false;
        string strEpisode = "";

        public void ParseTitleFromFilename(string filename)
        {
            foreach (char c in filename)
            {
                // ignore anything contained in []
                if(c == 'S')
                {
                    readingSeason = true;
                }
                else if (c == 'E')
                {
                    readingEpisode = true;
                }
                else if (readingSeason)
                {
                    if(Char.IsNumber(c))
                    {
                        strSeason += c;
                    }
                    else
                    {
                        readingSeason = false;
                        Int32.TryParse(strSeason, out season);
                        strSeason = "";
                        currentWord += c;
                    }
                }
                else if (readingEpisode)
                {
                    if (Char.IsNumber(c))
                    {
                        strEpisode += c;
                    }
                    else
                    {
                        readingEpisode = false;
                        Int32.TryParse(strEpisode, out episode);
                        strEpisode = "";
                        currentWord += c;
                    }
                }

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
