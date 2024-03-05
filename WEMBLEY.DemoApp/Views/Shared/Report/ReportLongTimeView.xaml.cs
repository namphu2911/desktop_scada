using Microsoft.Win32;
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
using WEMBLEY.DemoApp.Core.Application.ViewModels.Shared.Report;

namespace WEMBLEY.DemoApp.Views.Shared.Report
{
    /// <summary>
    /// Interaction logic for ReportLongTimeView.xaml
    /// </summary>
    public partial class ReportLongTimeView : UserControl
    {
        public ReportLongTimeView()
        {
            InitializeComponent();
        }

        private void SaveFileButtonClicked(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() == true)
            {
                var viewModel = (ReportLongTimeViewModel)DataContext;
                if (viewModel.ExportReportCommand.CanExecute(saveFileDialog.FileName))
                    viewModel.ExportReportCommand.Execute(saveFileDialog.FileName);
            }
        }
    }
}
