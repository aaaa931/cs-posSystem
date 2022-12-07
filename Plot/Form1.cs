using ScottPlot;
using ScottPlot.Drawing.Colormaps;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// ScottPlot start
using ScottPlot.Plottable;
// ScottPlot end


namespace Plot
{
    public partial class Form1 : Form
    {
        // ScottPlot start
        private Crosshair Crosshair;
        // ScottPlot end

        private string date = DateTime.Now.ToString("yyyy-MM-dd");
        public Form1(double[] dates, double[] data)
        {
            InitializeComponent();

            // ScottPlot start
            Crosshair = formsPlot1.Plot.AddCrosshair(0, 0);
            formsPlot1.Refresh();
            // ScottPlot end


            // 1. 先產生一維陣列，共有1000000個數字
            double[] values = ScottPlot.DataGen.RandomWalk(1000);
            //double[] values = [1.1, 2.2, ..., 9.8];

            // 先看一下第五個元素是什麼
            // MessageBox.Show(values[5].ToString());

            // 2. 用plt這個變數，當作【圖表數據】的捷徑
            var plt = formsPlot1.Plot;

            ////////////////////////////////
            // 3. 開始繪圖

            // sample data
            //double[] xs = DataGen.Consecutive(51);
            //double[] sin = DataGen.Sin(51);
            //double[] cos = DataGen.Cos(51);
            double[] xs = dates;
            double[] ys = DataGen.RandomWalk(dates.Length);
            plt.AddScatter(xs, ys);
            plt.XAxis.DateTimeFormat(true);

            // plot the data
            //plt.AddScatter(xs, sin, label: "sin");
            //plt.AddScatter(xs, cos, label: "cos");
            //plt.Legend();

            // customize the axis labels
            plt.Title($"{date} 統計圖表");
            plt.XLabel("Horizontal Axis");
            plt.YLabel("Vertical Axis");

            plt.SaveFig("quickstart_scatter.png");

            // 3. 繪圖結束
            ////////////////////////////////

            // 4. 將統計圖顯示在GUI上面
            formsPlot1.Refresh();
            //double[] values = { 778, 43, 283, 76, 184 };
            //string[] labels = { "C#", "JAVA", "Python", "F#", "PHP" };
            //plot_pie(values, labels);
        }

        private void plot_pie(double[] data, string[] labels)
        {
            var plt = formsPlot1.Plot;
            var pie = plt.AddPie(data);
            pie.SliceLabels = labels;
            pie.ShowPercentages = true;
            pie.ShowLabels = true;
            pie.Explode = true;
            plt.Legend();

            // mouse event
            formsPlot1_MouseLeave(null, null);
            plt.AddSignal(data);

            // Set axis limits to control the view
            // (min x, max x, min y, max y)
            plt.SetAxisLimits(0, 100, -25, 25);


            // customize the axis labels
            plt.Title($"{date} 圓餅圖");

            // 4. 將統計圖顯示在GUI上面
            formsPlot1.Refresh();
        }

        // 滑鼠移動進入圖表時，顯示座標
        private void formsPlot1_MouseEnter(object sender, EventArgs e)
        {
            Crosshair.IsVisible = true;
        }

        // 滑鼠移動時，顯示座標
        private void formsPlot1_MouseMove(object sender, MouseEventArgs e)
        {
            (double coordinateX, double coordinateY) =
                                     formsPlot1.GetMouseCoordinates();

            Crosshair.X = coordinateX;
            Crosshair.Y = coordinateY;

            formsPlot1.Refresh(lowQuality: true, skipIfCurrentlyRendering: true);

        }

        // 滑鼠移動離開圖表時，關閉顯示座標
        private void formsPlot1_MouseLeave(object sender, EventArgs e)
        {
            Crosshair.IsVisible = false;
            formsPlot1.Refresh();
        }
    }
}
