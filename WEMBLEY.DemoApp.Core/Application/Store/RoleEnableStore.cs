using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WEMBLEY.DemoApp.Core.Application.Store
{
    public class RoleEnableStore
    {
        public bool IsMFCEnabled { get; private set; } = false;
        public bool IsLotEnabled { get; private set; } = false;
        public Visibility LotVis => IsLotEnabled ? Visibility.Visible : Visibility.Hidden;
        public void SetRole(string role)
        {
            if(role == "Manager")
            {
                IsMFCEnabled = true;
                IsLotEnabled = true;
            }
            else if (role == "Employee")
            {
                IsMFCEnabled = false;
                IsLotEnabled = false;
            }
            else if (role == "Engineer")
            {
                IsMFCEnabled = true;
                IsLotEnabled = false;
            }
            else
            {
                IsMFCEnabled = false;
                IsLotEnabled = true;
            }
        }
    }
}
