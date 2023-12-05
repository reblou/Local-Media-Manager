using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyFlix
{
    internal class FileSystemSearcher
    {
        private readonly string[] _acceptedExtensions = { ".mkv", ".mp4", ".avi" };
        public List<FileSystemVideo> videos = new();
        public Dictionary<String, TVSeries> tvSeries = new();

        //TODO: error handling in the case that root folder is invalid

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

                videos.Add(new FileSystemVideo(file.Name, file.FullName));
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
