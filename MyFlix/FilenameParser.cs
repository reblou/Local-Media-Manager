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

        bool titleFound;

        public void ParseFilename(string filename)
        {
            this.filename = filename;

            while (i < filename.Length)
            {
                string word = GetNextWord();

                //TODO: combined s01e01 style format? 
                if(String.IsNullOrEmpty(word) )
                {
                    continue;
                }
                if(IsYear(word))
                {
                    this.releaseYear = word;
                    //TODO: What if date is in title? 
                    titleFound = true;
                } 
                else if (IsSeason(word))
                {
                    season = ExtractNumber(word);
                    titleFound = true;
                }
                else if (IsEpisode(word))
                {
                    episode = ExtractNumber(word);
                    titleFound = true;
                }
                else if(!titleFound)
                {
                    this.title += word + " ";
                }
            }

            title = title.Trim();
        }

        private string GetNextWord()
        {
            string word = "";
            bool squareBrackets = false;

            while (i < filename.Length && (!IsWhitespaceChar(filename[i]) || squareBrackets))
            {
                if(squareBrackets)
                {
                    if (filename[i] == ']') squareBrackets = false;
                }
                else if (filename[i] == '[')
                {
                    squareBrackets = true;
                }
                else
                {
                    word += filename[i];
                }

                i++;
            }
            i++;
            return word;
        }

        private static bool IsWhitespaceChar(char c)
        {
            return c == ' ' || c == '.' || c == '_' || c == '(' || c == ')' || c == '-';
        }

        private static bool IsYear(string s)
        {
            if (s.Length != 4) return false;

            if (s[0] != '1' || s[0] != '2') return false;

            if (s[1] != '9' || s[1] != '0') return false;

            foreach(char c in s)
            {
                if (!Char.IsDigit(c)) return false;
            }

            return true;
        }

        private static bool IsSeason(string s)
        {
            if (s.Length >= 6 && s[0..5].ToLower() == "season")
            {
                s = s.Substring(6);
            }
            else if (s[0] == 's' || s[0] == 'S')
            {
                s = s.Substring(1);
            }
            else
            {
                return false;
            }

            return IsEpisode(s);
        }

        private static bool IsEpisode(string s)
        {
            if (s.Length == 0 || s.Length > 3) return false;

            foreach (char c in s)
            {
                if (!Char.IsDigit(c)) return false;
            }

            return true;
        }

        private static int ExtractNumber(string s)
        {
            string numberOnly = "";

            foreach(char c in s)
            {
                if(Char.IsDigit(c)) numberOnly += c;
            }

            return Int32.Parse(numberOnly);
        }
    }
}
