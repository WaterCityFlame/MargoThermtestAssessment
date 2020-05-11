using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MargoThermtestAssessment.ViewModels
{


    public class MainViewModel
    {
        public MainViewModel() {


            this.MainTempControlPlot = new Models.TempControlPlot();
        }

        public Models.TempControlPlot MainTempControlPlot { get; private set; }

    }
}
