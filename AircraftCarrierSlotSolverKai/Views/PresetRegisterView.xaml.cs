﻿using AircraftCarrierSlotSolverKai.ViewModels;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AircraftCarrierSlotSolverKai.Views
{
    /// <summary>
    /// PresetRegisterView.xaml の相互作用ロジック
    /// </summary>
    public partial class PresetRegisterView : MetroWindow
    {
        public PresetRegisterView()
        {
            InitializeComponent();
        }

        public string FleetName => ((PresetRegisterViewModel)DataContext).FleetName.Value;

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
