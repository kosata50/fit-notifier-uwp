using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Windows.Data.Json;

namespace FitNotifier.Data.Services.Kos
{
    public class KosAuthService
    {
        private static readonly string AuthEndpoint = "https://auth.fit.cvut.cz/oauth/authorize";
        private static readonly string LoginUrl = "https://auth.fit.cvut.cz/login.do";
        private static readonly string TokenEndpoint = "https://auth.fit.cvut.cz/oauth/token";
        private static readonly string UsernameField = "j_username";
        private static readonly string PasswordField = "j_password";
        private static readonly string ApprovalField = "user_oauth_approval";

        private static readonly string Scope = "cvut:kosapi:read";
        private static readonly string RedirectUri = "https://localhost";
        private static readonly string ClientID = "8cfa60d1-9c8f-42a1-ad1b-c9bf32ff0431";
        private static readonly string ClientSecret = "";

        private static readonly string CookieUrl = "https://auth.fit.cvut.cz/";

        static KosAuthService()
        {
            var res = new Windows.ApplicationModel.Resources.ResourceLoader("FitNotifier.Data/Configuration");
            ClientSecret = res.GetString("KOSapi_secret");
        }


        public async Task<KosCredencials> RequestCredencials(UserCredencials user)
        {
            string code = await GetAuthCode(user);
            if (code != null)
                return await GetCredencials(code);
            else
                return null;
        }

        private async Task<string> GetAuthCode(UserCredencials user)
        {
            string url = UriUtils.AppendQueryString(AuthEndpoint, new Dictionary<string, string>() {
                { "response_type", "code" },
                { "client_id", ClientID },
                { "redirect_uri", RedirectUri },
                { "scope", Scope }
            });

            using (HttpClient client = new HttpClient(new FilteredHttpClientHandler(new Uri(RedirectUri))))
            {
                try
                {
                    HttpResponseMessage response;
                    string html;
                    using (response = await client.GetAsync(new Uri(url)))
                    {
                        html = await response.Content.ReadAsStringAsync();
                    }

                    if (html.Contains(UsernameField))
                    {
                        FormUrlEncodedContent post = new FormUrlEncodedContent(new Dictionary<string, string>()
                        {
                            { UsernameField, user.Username },
                            { PasswordField, user.Password }
                        });
                        using (response = await client.PostAsync(new Uri(LoginUrl), post))
                        {
                            html = await response.Content.ReadAsStringAsync();
                        }
                    }

                    if (html.Contains(ApprovalField))
                    {
                        FormUrlEncodedContent post = new FormUrlEncodedContent(new Dictionary<string, string>()
                        {
                            { ApprovalField, "true" }
                        });
                        using (response = await client.PostAsync(new Uri(AuthEndpoint), post))
                        {
                            html = await response.Content.ReadAsStringAsync();
                        }
                    }
                }
                catch (FilterMatchedException e)
                {
                    return UriUtils.ParseQueryString(e.Uri)["code"];
                }
            }
            return null;
        }

        private async Task<KosCredencials> GetCredencials(string code)
        {
            using (HttpClient client = new HttpClient())
            {
                FormUrlEncodedContent post = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    { "code", code },
                    { "grant_type", "authorization_code" },
                    { "redirect_uri", RedirectUri },
                    { "client_id", ClientID }
                });
                string authorization = Convert.ToBase64String(Encoding.ASCII.GetBytes(ClientID + ":" + ClientSecret));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authorization);
                using (HttpResponseMessage response = await client.PostAsync(new Uri(TokenEndpoint), post))
                {
                    string data = await response.Content.ReadAsStringAsync();
                    JsonObject json = JsonObject.Parse(data);
                    return new KosCredencials()
                    {
                        AccessToken = json["access_token"].GetString(),
                        TokenType = json["token_type"].GetString(),
                        RefreshToken = json["refresh_token"].GetString(),
                        Expiration = DateTime.Now.AddSeconds(json["expires_in"].GetNumber()),
                        Scope = json["scope"].GetString()
                    };
                }
            }
        }

        public void Logout()
        {
            Windows.Web.Http.Filters.HttpBaseProtocolFilter filter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();

            Windows.Web.Http.HttpCookieManager cookieManager = filter.CookieManager;
            Windows.Web.Http.HttpCookieCollection cookieCollection = cookieManager.GetCookies(new Uri(CookieUrl));
            foreach (var cookie in cookieCollection)
            {
                cookieManager.DeleteCookie(cookie);
            }
        }
    }
}
