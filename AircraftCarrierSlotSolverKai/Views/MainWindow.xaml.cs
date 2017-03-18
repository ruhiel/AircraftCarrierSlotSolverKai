using AircraftCarrierSlotSolverKai.ViewModels;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Prism.Events;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AircraftCarrierSlotSolverKai
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            Messenger.Instance.GetEvent<PubSubEvent<(bool result, string message)>>().Subscribe(
                async d =>
                {
                    if (!d.result)
                    {
                        await this.ShowMessageAsync("計算結果", d.message);
                    }
                }
            );
        }

        private void AirCraftSettingButton_Click(object sender, RoutedEventArgs e)
        {
            AirCraftSettingFlyout.DataContext = (sender as Button).DataContext;
            AirCraftSettingFlyout.IsOpen = true;
        }
    }
}
