using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix
{
    public interface IPlayable
    {
        string title { get; set; }
        string filePath { get; set;  }
        public string ToString();
    }
}
