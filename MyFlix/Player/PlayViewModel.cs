using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyFlix.Player
{
    public class PlayViewModel
    {
        public FullscreenCommand FullscreenCommand { get; set; }

        public PlayViewModel()
        {
            FullscreenCommand = new FullscreenCommand();
        }
    }
}
