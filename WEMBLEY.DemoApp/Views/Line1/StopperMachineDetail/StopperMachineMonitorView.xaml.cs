using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using WEMBLEY.DemoApp.Core.Application.ViewModels.Line1.StopperMachineDetail;

namespace WEMBLEY.DemoApp.Views.Line1
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
