using IdentityModel.OidcClient.Browser;
using IdentityModel.OidcClient;
using WEMBLEY.DemoApp.Core.Domain.Services;

namespace WEMBLEY.DemoApp.Core.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public event EventHandler<string>? UserLoggedIn;
        public event Action? LoggedInError;

        public async Task<string> LoginAsync(IBrowser browser)
        {
            var oidcClient = CreateOidcClient(browser);
            LoginResult loginResult;
            loginResult = await oidcClient.LoginAsync();

            string token = loginResult.AccessToken;
            UserLoggedIn?.Invoke(this, token);
            return token;
        }

        private OidcClient CreateOidcClient(IBrowser browser)
        {
            var options = new OidcClientOptions()
            {
                Authority = "https://authenticationserver20240411133358.azurewebsites.net",
                ClientId = "desktop-client",
                Scope = "openid native-client-scope profile",
                RedirectUri = "https://web-wembley-mes.vercel.app/signin-oidc",
                Browser = browser,
                Policy = new Policy
                {
                    RequireIdentityTokenSignature = false
                }
            };

            return new OidcClient(options);
        }
    }
}
