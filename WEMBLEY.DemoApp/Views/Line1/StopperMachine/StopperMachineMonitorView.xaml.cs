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
using System.Windows.Threading;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachine;

namespace WEMBLEY.DemoApp.Views.Line1.StopperMachine
{
    /// <summary>
    /// Interaction logic for StopperMachineMonitorView.xaml
    /// </summary>
    public partial class StopperMachineMonitorView : UserControl
    {
        public StopperMachineMonitorView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = (StopperMachineMonitorViewModel)DataContext;
            viewModel.ChartUpdated += ViewModel_ChartUpdated;
        }
        private void ViewModel_ChartUpdated()
        {
            Dispatcher.BeginInvoke(() => chart.Update(false, true), DispatcherPriority.Render);
        }
    }
}
