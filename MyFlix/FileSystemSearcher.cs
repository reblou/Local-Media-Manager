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
        TMDBApiHandler apiHandler = new TMDBApiHandler();

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

                videos.Add(GetVideoDetails(file));
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

        private Video GetVideoDetails(FileInfo file)
        {
            Video video = new Video(file.Name, file.FullName);
            video.LookupDetails(apiHandler);

            return video;
        }
    }
}
