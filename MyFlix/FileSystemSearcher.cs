using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix
{
    internal class FileSystemSearcher
    {
        private readonly string[] _acceptedExtensions = { ".mkv", ".mp4", ".avi" };
        private List<string> _videos = new();

        public List<string> GetVideosInDirRecursively(string dirFilepath)
        {
            DirectoryInfo rootDirectory = new DirectoryInfo(dirFilepath);
            StepThroughDirectory(rootDirectory);
            return _videos;
        }
        
        private void StepThroughDirectory(DirectoryInfo directory)
        {
            foreach (FileInfo file in directory.EnumerateFiles())
            {
                if (!IsVideoFileExtension(file.Extension)) continue;

                _videos.Add(file.Name);
            }

            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                StepThroughDirectory(dir);
            }
        }

        private bool IsVideoFileExtension(string extension)
        {
            if (string.IsNullOrEmpty(extension)) return false;

            foreach (string videoExtension in _acceptedExtensions)
            {
                if (videoExtension == extension) return true;
            }
            return false;
        }
    }
}
