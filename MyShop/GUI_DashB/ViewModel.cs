using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using LiveChartsCore.SkiaSharpView.Painting.Effects;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GUI_DashB
{
    public class ViewModel
    {
        public static ISeries[] Series { get; set; }
            = new ISeries[]
            {
                new LineSeries<float>
                {
                    Values = new float[] { 0 }
                },
            };
        public static Axis[] XAxes { get; set; }
            = new Axis[]
            {
                new Axis
                {

                }
            };
        public static Axis[] YAxes { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    Name = "Doanh thu",
                    NamePaint = new SolidColorPaint(SKColors.Red),


                    SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray)
                    {
                        StrokeThickness = 2,
                        PathEffect = new DashEffect(new float[] { 3, 3 })
                    }
                }
            };

        public static ISeries[] Series2 { get; set; }
            = new ISeries[]
            {
                new ColumnSeries<int>
                {
                    Values = new int[] { 0 }
                }
            };
        public static Axis[] XAxes2 { get; set; }
            = new Axis[]
            {
                new Axis
                {
                }
            };
        public static Axis[] YAxes2 { get; set; }
            = new Axis[]
            {
                new Axis
                {
                    Name = "Số lượng",
                    NamePaint = new SolidColorPaint(SKColors.Red),

                    SeparatorsPaint = new SolidColorPaint(SKColors.LightSlateGray)
                    {
                        StrokeThickness = 2,
                        PathEffect = new DashEffect(new float[] { 3, 3 })
                    }
                }
            };
    }
}
