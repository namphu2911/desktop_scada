using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WEMBLEY.DemoApp.Core.Application.ViewModels.SeedWork;
using WEMBLEY.DemoApp.Core.Domain.Dtos.References;

namespace WEMBLEY.DemoApp.Core.Application.Store
{
    public class HomeDataStore : BaseViewModel
    {
        public List<ParameterDto> HomeDatas { get; private set; }
        public long FSMin { get; private set; } = 0;
        public long FSMax { get; private set; } = 0;
        public HomeDataStore()
        {
            HomeDatas = new();
        }

        public void SetHomeRef(IEnumerable<ParameterDto> dtos)
        {
            HomeDatas = dtos.ToList();
            var station = dtos.SelectMany(dto => dto.Stations).FirstOrDefault(station => station.StationId == "IE-F3-BLO06");
            if (station != null)
            {
                FSMin = (long)((station.MFCs.First(mfc => mfc.MFCName == "FS Min").Value));
                FSMax = (long)((station.MFCs.First(mfc => mfc.MFCName == "FS Max").Value));
            }
        }

    }
}
