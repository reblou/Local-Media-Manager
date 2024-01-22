﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyFlix.Lookup.Parser
{
    public abstract class ParserState
    {
        protected Parser parser;

        public ParserState(Parser parser)
        {
            this.parser = parser;
        }

        public abstract void ParseWord(string word);

        protected bool IsYear(string word)
        {
            if (word.Length != 4) return false;

            if (word[0] != '1' && word[0] != '2') return false;

            if (word[1] != '9' && word[1] != '0') return false;

            foreach (char c in word)
            {
                if (!Char.IsDigit(c)) return false;
            }

            return true;
        }

        protected bool IsSeriesInfo(string word)
        {
            Regex regex = new Regex("[sS][0-9]{1,2}[eE][0-9]{1,4}");

            //If S0XE0X Format:
            if(regex.IsMatch(word)) return true;

            //TODO: Check for Just number format ?

            //TODO: Check for Episode 00 Format

            return false;
        }

        protected bool IsJunk(string word)
        {
            return ParseSettings.technicalInfo.Contains(word);
        }
    }
}
