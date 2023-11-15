using Accessibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace MyFlix
{
    public class MaxHeightUniformGrid : UniformGrid
    {
        public static readonly DependencyProperty MinRowHeightProperty = DependencyProperty.Register(
      "MinRowHeight", typeof(double), typeof(UniformGrid), new FrameworkPropertyMetadata(0d, FrameworkPropertyMetadataOptions.AffectsMeasure));

        protected override Size MeasureOverride(Size constraint)
        {
            Size calcSize = base.MeasureOverride(constraint);

            //TODO: account for label as well ? 

            double tileWidth = constraint.Width / Columns;

            // poster ratio: 2:3
            double tileDesiredHeight = tileWidth * 1.5;

            int rows = CalculateRows();
            double minHeight = tileDesiredHeight * rows;

            if(calcSize.Height < minHeight)
            {
                calcSize.Height = minHeight;
            }

            return calcSize;
        }

        int CalculateRows()
        {
            int _rows = Rows;

            if (FirstColumn >= Columns)
            {
                FirstColumn = 0;
            }

            if ((Rows == 0))
            {
                int nonCollapsedCount = 0;

                for (int i = 0, count = InternalChildren.Count; i < count; ++i)
                {
                    UIElement child = InternalChildren[i];
                    if (child.Visibility != Visibility.Collapsed)
                    {
                        nonCollapsedCount++;
                    }
                }

                if (nonCollapsedCount == 0)
                {
                    nonCollapsedCount = 1;
                }

                if (_rows == 0)
                {
                    if (Columns > 0)
                    {
                        _rows = (nonCollapsedCount + FirstColumn + (Columns - 1)) / Columns;
                    }
                    else
                    {
                        _rows = (int)Math.Sqrt(nonCollapsedCount);
                        if ((_rows * _rows) < nonCollapsedCount)
                        {
                            _rows++;
                        }
                        Columns = _rows;
                    }
                }
                else if (Columns == 0)
                {
                    Columns = (nonCollapsedCount + (_rows - 1)) / _rows;
                }
            }
            return _rows;
        }
    }
}
