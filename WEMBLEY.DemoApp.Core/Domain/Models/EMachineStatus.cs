using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Models
{
    public enum EMachineStatus
    {
        On = 0,
        Run = 1,
        Idle = 2,
        Alarm = 3,
        Setup = 4,
        Off = 5,
        Ready = 6,
        WifiDisconnted = 7
    }
}
