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

namespace WEMBLEY.DemoApp.Resources.Components
{
    /// <summary>
    /// Interaction logic for ErrorMessage.xaml
    /// </summary>
    public partial class ErrorMessage : UserControl
    {
        public static readonly DependencyProperty MessageProperty = DependencyProperty
        .Register("Message", typeof(string), typeof(ErrorMessage), new PropertyMetadata(string.Empty));

        public string Message
        {
            get
            {
                return (string)GetValue(MessageProperty);
            }
            set
            {
                SetValue(MessageProperty, value);
            }
        }

        public ErrorMessage()
        {
            InitializeComponent();
        }
    }
}
