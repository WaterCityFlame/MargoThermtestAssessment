using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MargoThermtestAssessment.ViewModels
{
    using MargoThermtestAssessment.Models;
    using OxyPlot;

    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly Timer timer;
        private Models.TempController controllerData;

        public IList<ScatterPoint> tempReadings { get; set; }
        public IList<DataPoint> averageTemp { get; set; }
        public IList<RSDPoint> RSD { get; set; }


        public DataPoint ArrowStart { get; set; }
        public DataPoint ArrowEnd { get; set; }

        public string TempStabilized { get; set; }
        public int TempAnnotationThickness { get; set; } //hack, because "visibility" doesn't work on oxyplot annotations
        public string CurrentTemp { get; set; }
        public string AverageTemp { get; set; }
        public string CurrentRSD { get; set; }

        private int refresh;
        public int Refresh { get {
                return this.refresh;
            } 
            set {
                this.refresh = value;
                this.NotifyPropertyChanged();
            } }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel() {
            this.Refresh=0;

            this.timer = new Timer(AddTempReading);
            this.timer.Change(Timeout.Infinite, Timeout.Infinite);
            this.controllerData = new Models.TempController();

            TempStabilized = "hidden";
            TempAnnotationThickness = 0;

            this.Refresh++;
            this.timer.Change(0, 200);

        }

        private void AddTempReading(object state)
        {
            if (controllerData.PollTemp()) {
                this.tempReadings = controllerData.tempReadings;
                this.averageTemp = controllerData.averageTemp;
                this.RSD = controllerData.RSD;

                double currentTime = tempReadings[tempReadings.Count() - 1].X;
                double currentTempDouble = tempReadings[tempReadings.Count() - 1].Y;
                this.CurrentTemp = currentTempDouble.ToString("N2");
                this.AverageTemp = averageTemp[averageTemp.Count() - 1].Y.ToString("N2");

                double rsdDouble = RSD[RSD.Count() - 1].Value;
                this.CurrentRSD = rsdDouble.ToString("N2");
                if (rsdDouble < 1 && rsdDouble > 0)
                {
                    if (TempStabilized == "hidden")
                    {
                        ArrowStart = new DataPoint(currentTime, currentTempDouble+4);
                        ArrowEnd = new DataPoint(currentTime, currentTempDouble);
                    }
                    TempStabilized = "visible";
                    TempAnnotationThickness = 2;
                } else
                {
                    TempStabilized = "hidden";
                    TempAnnotationThickness = 0;
                }

                this.Refresh++;
            } else {
                this.timer.Dispose();
            }
        }

        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



    }

}
