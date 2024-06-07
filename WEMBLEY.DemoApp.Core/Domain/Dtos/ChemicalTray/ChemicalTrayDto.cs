using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos.ChemicalTray
{
    public class ChemicalTrayDto
    {
        public DateTime TimeStamp { get; set; }
        public List<CellDto> Cells { get; set; }

        public ChemicalTrayDto(DateTime timeStamp, List<CellDto> cells)
        {
            TimeStamp = timeStamp;
            Cells = cells;
        }
    }
}
