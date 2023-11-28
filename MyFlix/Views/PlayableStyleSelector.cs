using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyFlix.Views
{
    public class PlayableStyleSelector : StyleSelector
    {
        public override Style SelectStyle(object item, DependencyObject container)
        {
            Style style = new Style();


            ItemsControl itemsControl = ItemsControl.ItemsControlFromItemContainer(container);

            IPlayable playable = itemsControl.DataContext as IPlayable;
            playable ??= new Film("", "", "", "");

            if(playable.watched)
            {
            }

            return style;
        }
    }
}
