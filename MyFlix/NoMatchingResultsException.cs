using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix
{
    internal class NoMatchingResultsException : Exception
    {
        public NoMatchingResultsException(string? message) : base(message)
        {
        }
    }
}
