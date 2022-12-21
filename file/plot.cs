using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot.Plottable;
using static iText.Svg.SvgConstants;

namespace Classes
{
    public class plot
    {
        private string date = DateTime.Now.ToString("yyyy-MM-dd");
        public void pie(ScottPlot.Plot plt, double[] data, string[] labels, string title = "")
        {
            //var plt = formsPlot1.Plot;
            plt.Clear();
            var pie = plt.AddPie(data);
            pie.SliceLabels = labels;
            pie.ShowPercentages = true;
            pie.ShowLabels = true;
            pie.Explode = true;
            plt.Legend();

            // mouse event
            //formsPlot1_MouseLeave(null, null);
            //plt.AddSignal(data);

            // Set axis limits to control the view
            // (min x, max x, min y, max y)
            //plt.SetAxisLimits(0, 100, -25, 25);

            // customize the axis labels
            plt.Title($"{date} 出貨{title}圓餅圖");

            // 4. 將統計圖顯示在GUI上面
            //formsPlot1.Refresh();
        }

        public void bar(ScottPlot.Plot plt, double[] data, string[] labels, string title = "")
        {
            double[] positions = new double[labels.Length];

            for (int i = 0; i < labels.Length; i++)
            {
                positions[i] = i;
            }

            plt.Clear();
            plt.AddBar(data, positions);
            //plt.Legend();

            // mouse event
            //formsPlot1_MouseLeave(null, null);
            plt.XTicks(positions, labels);

            // Set axis limits to control the view
            // (min x, max x, min y, max y)
            //plt.SetAxisLimits(0, 100, -25, 25);

            // customize the axis labels
            plt.Title($"{date} 出貨{title}長條圖");
            plt.SetAxisLimits(yMin: 0);
            //plt.SaveFig("bar_labels.png");

            // 4. 將統計圖顯示在GUI上面
            //formsPlot1.Refresh();
        }

        public void run(ScottPlot.Plot plt, double[] datas, double[] dates, string title = "")
        {
            //plt.AddSignal(datas, sampleRate: 200);
            plt.Title($"{date} 出貨{title}趨勢圖");
            //plt.SetAxisLimits(0, 5, -25, 25);

            plt.AddScatter(dates, datas);
            plt.XAxis.DateTimeFormat(true);

            // define tick spacing as 1 day (every day will be shown)
            plt.XAxis.ManualTickSpacing(1, ScottPlot.Ticks.DateTimeUnit.Day);
            plt.XAxis.TickLabelStyle(rotation: 45);

            // add some extra space for rotated ticks
            plt.XAxis.SetSizeLimit(min: 50);
        }
    }
}
