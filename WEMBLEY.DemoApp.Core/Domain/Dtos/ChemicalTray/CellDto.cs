using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos.ChemicalTray
{
    public class CellDto
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int Value { get; set; }

        public CellDto(int row, int column, int value)
        {
            Row = row;
            Column = column;
            Value = value;
        }
    }
}
