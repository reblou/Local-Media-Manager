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
        public List<Video> videos = new();

        public void GetVideosInDirRecursively(string dirFilepath)
        {
            if (String.IsNullOrEmpty(dirFilepath)) return;

            DirectoryInfo rootDirectory = new DirectoryInfo(dirFilepath);
            StepThroughDirectory(rootDirectory);
        }
        
        private void StepThroughDirectory(DirectoryInfo directory)
        {
            foreach (FileInfo file in directory.EnumerateFiles())
            {
                if (!IsVideoFileExtension(file.Extension)) continue;

                videos.Add(new Video() {
                    title = TitleParser.ParseTitleFromFilename(file.Name),
                    fileName = file.Name,
                    filePath = file.FullName
                }
                );
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
