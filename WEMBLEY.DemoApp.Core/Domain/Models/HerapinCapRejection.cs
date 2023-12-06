using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Models
{
    public class HerapinCapRejection
    {
        public long? BOTTOMCAP { get; set; }
        public long? SILICONPRESENCE { get; set; }
        public long? COVERPRESENCE { get; set; }
        public long? HEIGHTCHK { get; set; }
        public long? LEAKTESTCHKOK { get; set; }
        public HerapinCapRejection(long? bOTTOMCAP, long? sILICONPRESENCE, long? cOVERPRESENCE, long? hEIGHTCHK, long? lEAKTESTCHKOK)
        {
            BOTTOMCAP = bOTTOMCAP;
            SILICONPRESENCE = sILICONPRESENCE;
            COVERPRESENCE = cOVERPRESENCE;
            HEIGHTCHK = hEIGHTCHK;
            LEAKTESTCHKOK = lEAKTESTCHKOK;
        }
    }
}
