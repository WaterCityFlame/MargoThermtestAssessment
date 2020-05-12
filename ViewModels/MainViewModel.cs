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
    using Microsoft.Win32;
    using OxyPlot;
    using System.Linq.Expressions;
    using System.Security;
    using System.Windows;
    using System.Windows.Controls;
    using System.Xml;
    using Forms = System.Windows.Forms;

    public class MainViewModel : INotifyPropertyChanged
    {
        private Timer timer;
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
            try {
                if ( controllerData.PollTemp())
                {
                    this.tempReadings = controllerData.tempReadings;
                    this.averageTemp = controllerData.averageTemp;
                    this.RSD = controllerData.RSD;

                    double currentTime = tempReadings[tempReadings.Count() - 1].X;
                    double currentTempDouble = tempReadings[tempReadings.Count() - 1].Y;
                    this.CurrentTemp = currentTempDouble.ToString("N2");
                    this.AverageTemp = averageTemp[averageTemp.Count() - 1].Y.ToString("N2");

                    double rsd = RSD[RSD.Count() - 1].Value;
                    FormatRSDData(currentTime, currentTempDouble, rsd);

                    this.Refresh++;
                }
                else
                {
                    this.timer.Dispose();
                }
            } catch (Exception ex) {
                this.timer.Dispose();

                Forms.MessageBox.Show($"Error reading CSV file.\n\nError message: {ex.Message}\n\n" +
                $"Details:\n\n{ex.StackTrace}");
            }
        }

        private void FormatRSDData(double currentTime, double currentTemp, double rsd)
        {
            this.CurrentRSD = rsd.ToString("N2");
            if (rsd < 1 && rsd > 0)
            {
                if (TempStabilized == "hidden")
                {
                    ArrowStart = new DataPoint(currentTime, currentTemp + 4);
                    ArrowEnd = new DataPoint(currentTime, currentTemp);
                }
                TempStabilized = "visible";
                TempAnnotationThickness = 2;
            }
            else
            {
                TempStabilized = "hidden";
                TempAnnotationThickness = 0;
            }
        }

        private void NotifyPropertyChanged(string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OpenFile(object sender, EventArgs e)
        {
            //pause timer
            this.timer.Dispose();
            this.timer = new Timer(AddTempReading);
            this.timer.Change(Timeout.Infinite, Timeout.Infinite);


            Forms.OpenFileDialog openFileDialog = new Forms.OpenFileDialog();
            if (openFileDialog.ShowDialog() == Forms.DialogResult.OK)
            {
                try
                {
                    string filePath = openFileDialog.FileName;
                    controllerData.ReadFile(filePath);

                    //restart timer
                    this.timer.Change(0, 200);
                }
                catch (SecurityException ex)
                {
                    Forms.MessageBox.Show($"Security error.\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }

        }
        
        public void SaveFile(object sender, EventArgs e)
        {
            Forms.SaveFileDialog saveFileDialog = new Forms.SaveFileDialog();
            saveFileDialog.Filter = "XML File | *.xml";

            if (saveFileDialog.ShowDialog() == Forms.DialogResult.OK)
            {
                if (saveFileDialog.FileName != "")
                {
                    System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog.OpenFile();

                    XmlWriterSettings settings = new XmlWriterSettings();
                    settings.NewLineOnAttributes = true;
                    settings.Indent = true;
                    using (XmlWriter writer = XmlWriter.Create(fs, settings))
                    {
                        //make copies so that the graph can keep reading data without messing with what we're trying to save here
                        IList<ScatterPoint> savedTempReadings = new List<ScatterPoint>(tempReadings.ToArray());
                        IList<DataPoint> savedAverageTemp = new List<DataPoint>(averageTemp.ToArray());
                        IList<RSDPoint> savedRSD = new List<RSDPoint>(RSD.ToArray());

                        writer.WriteStartElement("TestResults");
                        for (int i= 0; i < savedTempReadings.Count(); i++)
                        {
                            writer.WriteStartElement("TestResult");
                            writer.WriteElementString("Timestamp", savedTempReadings[i].X.ToString());
                            writer.WriteElementString("Temperature", savedTempReadings[i].Y.ToString());
                            writer.WriteElementString("RollingAverage", savedAverageTemp[i].Y.ToString());
                            writer.WriteElementString("RollingStandardDeviation", savedRSD[i].Value.ToString());
                            writer.WriteEndElement();
                        }
                        writer.WriteEndElement();
                        writer.Flush();
                    }

                }
            }

        }



    }

}
