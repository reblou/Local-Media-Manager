using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix
{
    internal enum SeriesInfoType
    {
        Season,
        Episode
    }

    public class FilenameParser
    {
        //TODO: return FilenameInfo - Film/TV type object
        public string title = "";
        public string releaseYear = "";
        public int season = -1;
        public int episode = -1;

        int i = 0;
        string filename;

        public void ParseFilename(string filename)
        {
            this.filename = filename;

            while (i < filename.Length)
            {
                string word = GetNextWord();

                // if year
                if(IsYear(word))
                {
                    this.releaseYear = word;
                } 

                if(String.IsNullOrEmpty(this.releaseYear) && this.episode == -1 && this.season == -1)
                {
                    this.title += word;
                }
            }


        }

        private string GetNextWord()
        {
            string word = "";
            bool squareBrackets = false;

            while (i < filename.Length && !IsWhitespaceChar(filename[i]))
            {
                if (filename[i] == '[')
                {
                    squareBrackets = true;
                    continue;
                }

                if (squareBrackets && filename[i] == ']')
                {
                    squareBrackets = false;
                    continue;
                }

                if (filename[i] == 'S' || filename[i] == 's')
                {
                    TryToExtractSeriesInfo(SeriesInfoType.Season);
                }
                else if (filename[i] == 'E' || filename[i] == 'e')
                {
                    TryToExtractSeriesInfo(SeriesInfoType.Episode);
                }

                if(!squareBrackets) word += filename[i];

                i++;
            }

            return word;
        }

        private void TryToExtractSeriesInfo(SeriesInfoType type)
        {
            if (type == SeriesInfoType.Season)
            {
                CheckForSeasonInFilename();
            }
            else
            {
                CheckForEpisodeInFilename();
            }

            i++;
            string number = "";
            while (Char.IsDigit(filename[i]))
            {
                number += filename[i];
                i++;
            }

            if (number.Length > 0)
            {
                // try to parse into int
                if(type == SeriesInfoType.Season)
                {
                    Int32.TryParse(number, out season);
                }
                else
                {
                    Int32.TryParse(number, out episode);
                }         
            }
        }

        private void CheckForSeasonInFilename()
        {
            if (filename[i..(i + 5)].ToLower() == "season")
            {
                i = i + 5;
            }
        }

        private void CheckForEpisodeInFilename()
        {
            if (filename[i..(i + 6)].ToLower() == "episode")
            {
                i = i + 6;
            }
        }

        private static bool IsWhitespaceChar(char c)
        {
            return c == ' ' || c == '.' || c == '_' || c == '(' || c == ')';
        }

        private static bool IsYear(string s)
        {
            if (s[0] != '1' || s[0] != '2') return false;

            if (s[1] != '9' || s[1] != '0') return false;

            foreach(char c in s)
            {
                if (!Char.IsDigit(c)) return false;
            }

            return true;
        }
    }
}
