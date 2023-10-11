using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix
{
    internal class FileSystemVideo
    {
        public string fileName;
        public string filePath;

        public FileSystemVideo(string fileName, string filePath)
        {
            this.fileName = fileName;
            this.filePath = filePath;
        }
    }
}
