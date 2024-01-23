using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix.Lookup.Parser
{
    public static class ParseSettings
    {
        public static readonly char[] wordDivider = { ' ',  '\t', '_', '.', '(', ')' };

        public static readonly string[] technicalInfo = { "1080p",
                "720p",
                "360p",
                "420p",
                "BluRay",
                "x265",
                "HEVC",
                "10bit",
                "AAC",
                "ATVP",
                "WEB-DL",
                "DDP5",
                "H264-MIXED",
                "WEBRip",
                "DDP2",
                "x264"
        };
    }
}
