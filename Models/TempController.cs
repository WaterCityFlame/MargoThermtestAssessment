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
    using System.Collections.Generic;

    public class TempController
    {
        private string path = "Resources\\test_out.txt";
        private TextFieldParser csvParser;
        private Queue<float> lastTenSeconds; //filtered temp values, last 10 seconds
        private Queue<float> lastFiveReadings; //raw temp values, last second

        public List<ScatterPoint> tempReadings { get; private set; }
        public List<DataPoint> averageTemp { get; private set; }
        public List<RSDPoint> RSD { get; private set; }

        public TempController()
        {
            this.csvParser = new TextFieldParser(this.path);
            this.csvParser.SetDelimiters(new string[] { "," });
            this.csvParser.HasFieldsEnclosedInQuotes = false;

            this.lastTenSeconds = new Queue<float>();
            this.lastFiveReadings = new Queue<float>();

            this.tempReadings = new List<ScatterPoint>();
            this.averageTemp = new List<DataPoint>();
            this.RSD = new List<RSDPoint>();

        }

        public bool PollTemp()
        {
            if (!csvParser.EndOfData)
            {
                string[] fields = csvParser.ReadFields();
                float rawTemp = float.Parse(fields[0]);
                float timestamp = float.Parse(fields[1]);
                float temp = FilterNoise(rawTemp);

                if(lastTenSeconds.Count == 50) {
                    lastTenSeconds.Dequeue();
                }
                lastTenSeconds.Enqueue(temp);

                this.tempReadings.Add(new ScatterPoint(timestamp, temp));

                float average = CalculateRollingAverage();
                this.averageTemp.Add(new DataPoint(timestamp, average));

                double latestRSD = CalculateRSD();
                RSD.Add(new RSDPoint {
                    Time = timestamp,
                    Value = latestRSD,
                    Maximum = average + latestRSD,
                    Minimum = average - latestRSD
                });
                return true;
            }
            else return false;

        }

        private float FilterNoise(float rawTemp)
        {
            if (lastFiveReadings.Count == 5)
            {
                lastFiveReadings.Dequeue();
            }
            lastFiveReadings.Enqueue(rawTemp);

            float temp = (lastFiveReadings.Sum()) / (lastFiveReadings.Count());

            return temp;
        }

        private float CalculateRollingAverage()
        { 
            return lastTenSeconds.Sum() / lastTenSeconds.Count();
        }

        private double CalculateRSD()
        {
            if (lastTenSeconds.Count < 2)
            {
                return -1;
            } else {
                double sum = lastTenSeconds.Sum();
                double sumOfSquares = 0;
                foreach (float i in lastTenSeconds)
                {
                    sumOfSquares += (i * i);
                }
                double diff = sumOfSquares - (sum * sum / lastTenSeconds.Count());
                return Math.Sqrt(diff/(lastTenSeconds.Count()-1));
            }
        }
    }


    public class RSDPoint
    {
        public double Time { get; set; }
        public double Value { get; set; }
        public double Minimum { get; set; }
        public double Maximum { get; set; }
        public override string ToString()
        {
            return String.Format("{0:#0.0} {1:##0.0}", Time, Value);
        }
    }
}
