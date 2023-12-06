using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Dtos
{
    public class ServerSideError
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public ServerSideError(string errorCode, string errorMessage)
        {
            Code = errorCode;
            Message = errorMessage;
        }
    }
}
