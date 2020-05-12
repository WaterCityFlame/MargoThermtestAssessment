using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MargoThermtestAssessment.Models
{
    using OxyPlot;
    using OxyPlot.Series;
    using Microsoft.VisualBasic.FileIO;
    using System;
    using System.Threading;

    public class TempControlPlot : PlotModel
    {
        private string path = "Resources\\test_out.txt";
        private TextFieldParser csvParser;

        public TempControlPlot()
        {
            this.csvParser = new TextFieldParser(this.path);
            this.csvParser.SetDelimiters(new string[] { "," });
            this.csvParser.HasFieldsEnclosedInQuotes = false;

            this.Title = "Plant Temperature Over Time";
            ScatterSeries tempData = new ScatterSeries { 
                Title = "Plant Temperature", 
                MarkerType = MarkerType.Diamond, 
                MarkerFill=OxyColors.DarkBlue};
            this.Series.Add(tempData);

            OxyPlot.Axes.LinearAxis yAxis = MakeAxis("Temperature (°C)", 0, 40);
            tempData.YAxisKey = yAxis.Key; 
            this.Axes.Add(yAxis);

            OxyPlot.Axes.LinearAxis xAxis = MakeAxis("Time (Seconds)", 0, 30);
            xAxis.Position = OxyPlot.Axes.AxisPosition.Bottom;
            tempData.XAxisKey = xAxis.Key; 
            this.Axes.Add(xAxis);

        }

        private OxyPlot.Axes.LinearAxis MakeAxis(string title, double min, double max)
        {
            OxyPlot.Axes.LinearAxis axis = new OxyPlot.Axes.LinearAxis(); 
            axis.AbsoluteMinimum = min;
            axis.MinimumRange = max;
            axis.MaximumRange = max;
            axis.IsZoomEnabled = false;
            axis.Title = title; 
            axis.Key = title;
            return axis;
        }

        public bool AddTempReading()
        {
            ScatterSeries tempData = (ScatterSeries)Series[0];
            if (!csvParser.EndOfData)
            {
                string[] fields = csvParser.ReadFields();
                float temp = float.Parse(fields[0]);
                float timestamp = float.Parse(fields[1]);
                tempData.Points.Add(new ScatterPoint(timestamp, temp));
                return true;
            }
            else return false;

        }
    }
}
