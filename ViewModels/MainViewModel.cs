using OxyPlot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MargoThermtestAssessment.ViewModels
{


    public class MainViewModel 
    {
        private readonly Timer timer;


        public Models.TempControlPlot MainTempControlPlot { get; private set; }

        public MainViewModel() {
            this.timer = new Timer(AddTempReading);
            this.timer.Change(Timeout.Infinite, Timeout.Infinite);
            this.MainTempControlPlot = new Models.TempControlPlot();

            this.timer.Change(0, 200);
        }

        private void AddTempReading(object state)
        {
            if (MainTempControlPlot.AddTempReading())
            {
                MainTempControlPlot.InvalidatePlot(true);
            } else
            {
                this.timer.Dispose();
            }
        }

    }
}
