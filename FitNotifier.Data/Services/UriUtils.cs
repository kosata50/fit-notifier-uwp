using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitNotifier.Data.Services
{
    public static class UriUtils
    {
        public static string AppendQueryString(string url, Dictionary<string, string> parameters)
        {
            UriBuilder uri = new UriBuilder(url);
            uri.Query += String.Join("&", parameters.Select(p => p.Key + "=" + Uri.EscapeUriString(p.Value)));
            return uri.ToString();
        }

        public static Dictionary<string, string> ParseQueryString(Uri uri)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (string parameter in uri.Query.Substring(1).Split('&'))
            {
                string[] split = parameter.Split('=');
                dic[split[0]] = split[1];
            }
            return dic;
        }
    }
}
