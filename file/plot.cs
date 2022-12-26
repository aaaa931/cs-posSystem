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
        private string date = DateTime.Now.ToString("yyyy-MM-dd").Substring(0, 8);
        private Classes.File file = new Classes.File();
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
            plt.Title($"{date} 出貨{title}圓餅圖");
            plt.SaveFig($"pie.png");
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
            plt.XTicks(positions, labels);
            plt.Title($"{date} 出貨{title}長條圖");
            plt.SetAxisLimits(yMin: 0);
            plt.SaveFig($"bar.png");
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
            plt.SaveFig($"run.png");
        }
    }
}
