﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MargoThermtestAssessment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private ViewModels.MainViewModel dc;

        public MainWindow()
        {
            InitializeComponent();
            dc =  new ViewModels.MainViewModel();
            DataContext = dc;
        }

        private void OpenFile_Click(object sender, EventArgs e) {
            dc.OpenFile(sender, e);
        }
        private void SaveFile_Click(object sender, EventArgs e) {
            dc.SaveFile(sender, e);
        }
    }   
}
