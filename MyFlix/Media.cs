using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix
{
    public abstract class Media
    {
        public string filePath;
        public string fileName;
        public string title { get; set; }
        public string description { get; set; }

        public string posterURL { get; set; }
        public string backdropURL { get; set; }

        public override string ToString()
        {
            return title;
        }

        public abstract void LookupDetails(TMDBApiHandler handler);
    }
}
