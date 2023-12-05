using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace MyFlix.ViewModels
{
    public class MediaDetailsViewModel
    {
        public IDisplayable displayable { get; set; }
        public List<IPlayable> playables { get => displayable.GetPlayables(); set { playables = value; } }

        public MediaDetailsViewModel(IDisplayable displayable)
        {
            this.displayable = displayable;
        }
    }
}
