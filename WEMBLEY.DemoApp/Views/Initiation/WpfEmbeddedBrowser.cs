using IdentityModel.OidcClient.Browser;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WEMBLEY.DemoApp.Views.Initiation
{
    public class WpfEmbeddedBrowser : IBrowser
    {
        private readonly WebView2 webView;

        public WpfEmbeddedBrowser(WebView2 webView)
        {
            this.webView = webView;
        }

        public async Task<BrowserResult> InvokeAsync(BrowserOptions options, CancellationToken cancellationToken = default)
        {
            var semaphoreSlim = new SemaphoreSlim(0, 1);
            var browserResult = new BrowserResult()
            {
                ResultType = BrowserResultType.UserCancel
            };

            webView.NavigationStarting += (s, e) =>
            {
                if (IsBrowserNavigatingToRedirectUri(new Uri(e.Uri), options))
                {
                    e.Cancel = true;

                    browserResult = new BrowserResult()
                    {
                        ResultType = BrowserResultType.Success,
                        Response = new Uri(e.Uri).AbsoluteUri
                    };

                    semaphoreSlim.Release();
                }
            };

            await webView.EnsureCoreWebView2Async(null);
            webView.CoreWebView2.CookieManager.DeleteAllCookies();

            webView.CoreWebView2.Navigate(options.StartUrl);

            await semaphoreSlim.WaitAsync();

            return browserResult;
        }

        private bool IsBrowserNavigatingToRedirectUri(Uri uri, BrowserOptions options)
        {
            return uri.AbsoluteUri.StartsWith(options.EndUrl);
        }
    }
}
