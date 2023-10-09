using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix
{
    interface IPlayable
    {
        public string ToString();
        public abstract void LookupDetails(TMDBApiHandler handler);
    }
}
