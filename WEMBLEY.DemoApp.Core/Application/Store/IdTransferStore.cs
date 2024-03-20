using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WEMBLEY.DemoApp.Core.Application.Store
{
    public class IdTransferStore
    {
        public string Id { get; private set; } = "";
        public string IsSelected { get; private set; } = "";
        public Visibility OEERowVis { get; set; } = Visibility.Collapsed;
        public Visibility ARowVis { get; set; } = Visibility.Collapsed;
        public Visibility PRowVis { get; set; } = Visibility.Collapsed;
        public Visibility QRowVis { get; set; } = Visibility.Collapsed;
        public void SetIdTransfer(string id, string isSelected, Visibility oEERowVis, Visibility aRowVis, Visibility pRowVis, Visibility qRowVis)
        {
            Id = id;
            IsSelected = isSelected;
            OEERowVis = oEERowVis;
            ARowVis = aRowVis;
            PRowVis = pRowVis;
            QRowVis = qRowVis;
        }
    }
}
