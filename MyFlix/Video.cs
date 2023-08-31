using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix
{
    internal class Video
    {
        public string filePath {  get; set; }
        public string title { get; set; }
        public string fileName { get; set; }

        public override string ToString()
        {
            return title;
        }

    }
}
