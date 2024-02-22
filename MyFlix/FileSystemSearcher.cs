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
        private readonly string[] extrasFolders = { "Extras", "Featurettes" };
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
                // Ignore extras folder
                if (extrasFolders.Contains(dir.Name)) continue;

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

        public List<string> FindExtrasFolders(DirectoryInfo rootDirtectory)
        {
            List<string> folders = new List<string>();

            foreach (DirectoryInfo dir in rootDirtectory.GetDirectories())
            {
                // ignore root level extras folder
                if (extrasFolders.Contains(dir.Name)) continue;

                folders.AddRange(RecFindExtrasFolders(dir));
            }

            return folders;
        }

        private List<string> RecFindExtrasFolders(DirectoryInfo directory)
        {
            List<string> folders = new List<string>();

            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                // add extras folders
                if (extrasFolders.Contains(dir.Name)) folders.Add(dir.FullName);

                folders.AddRange(RecFindExtrasFolders(dir));
            }

            return folders;
        }
    }
}
