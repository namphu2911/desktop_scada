using IdentityModel.OidcClient.Browser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Core.Domain.Services
{
    public interface IAuthenticationService
    {
        Task<string> LoginAsync(IBrowser browser);
        event EventHandler<string> UserLoggedIn;
    }
}
