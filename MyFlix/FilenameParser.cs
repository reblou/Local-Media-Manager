using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix
{
    public class FilenameParser
    {
        public string title = "";
        public string releaseYear = "";
        public int season = -1;
        public int episode = -1;

        int i = 0;
        string filename;

        bool titleFound;
        string reserveTitle = "";

        public void ParseFilename(string filename)
        {
            this.filename = filename;

            while (i < filename.Length)
            {
                string word = GetNextWord();

                if(String.IsNullOrEmpty(word) )
                {
                    continue;
                }
                if(IsYear(word))
                {
                    if(!String.IsNullOrEmpty(releaseYear))
                    {
                        title += this.releaseYear + " " + reserveTitle;
                        reserveTitle = "";
                    }
                    this.releaseYear = word;
                    titleFound = true;
                } 
                else if(IsTechnicalInfo(word))
                {
                    break;
                }
                else if(IsSeasonEpisodeCombo(word))
                {
                    (season, episode) = ExtractSeasonEpisode(word);
                    titleFound = true;
                }
                else if (IsSeason(word))
                {
                    season = ExtractNumber(word);
                    titleFound = true;
                }
                else if (IsEpisode(word)) //TODO: number in title can be flagged as episode
                {
                    episode = ExtractNumber(word);
                    titleFound = true;
                }
                else if (titleFound) // check if title found first so random junk isn't picked up as an episode number
                {
                    reserveTitle += word + " ";
                }
                else
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

            if (s[0] != '1' && s[0] != '2') return false;

            if (s[1] != '9' && s[1] != '0') return false;

            foreach(char c in s)
            {
                if (!Char.IsDigit(c)) return false;
            }

            return true;
        }

        private static bool IsSeason(string s)
        {
            if (s.Length == 0) return false;
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

            if (s[0] == 'e' || s[0] == 'S')
            {
                s = s.Substring(1);
            }

            foreach (char c in s)
            {
                if (!Char.IsDigit(c)) return false;
            }

            return true;
        }

        private static bool IsSeasonEpisodeCombo(string s)
        {
            int mid = s.IndexOf('E');
            if (mid == -1) mid = s.IndexOf('e');
            if (mid == -1 || mid == 0) return false;

            return IsSeason(s.Substring(0, mid)) && IsEpisode(s.Substring(mid+1));
        }

        private static bool IsTechnicalInfo(string s)
        {
            List<string> technicalTerms = new List<string>()
            {
                "1080p",
                "720p",
                "360p",
                "420p",
                "BluRay",
                "x265",
                "HEVC",
                "10bit",
                "AAC",
                "ATVP",
                "WEB-DL",
                "DDP5",
                "H264-MIXED",
                "WEBRip",
                "DDP2",
                "x264"
            };

            return technicalTerms.Contains(s);
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

        private static (int, int) ExtractSeasonEpisode(string s)
        {
            int mid = s.IndexOf('E');
            if (mid == -1) mid = s.IndexOf('e');
            if (mid == -1) return (-1, -1);

            return (ExtractNumber(s.Substring(0, mid)), ExtractNumber(s.Substring(mid+1)));
        }
    }
}
