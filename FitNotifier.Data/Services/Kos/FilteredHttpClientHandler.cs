using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FitNotifier.Data.Services.Kos
{
    public class FilteredHttpClientHandler : HttpClientHandler
    {
        private Uri filter;

        public FilteredHttpClientHandler(Uri filter)
        {
            this.filter = filter;
            AllowAutoRedirect = false;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);
            if (response.Headers.Location != null)
            {
                if (response.Headers.Location.AbsoluteUri.StartsWith(filter.AbsoluteUri))
                    throw new FilterMatchedException(response.Headers.Location);
                return await SendAsync(new HttpRequestMessage(request.Method, response.Headers.Location), cancellationToken);
            }
            else
                return response;
        }
    }

    public class FilterMatchedException : HttpRequestException
    {
        public Uri Uri { get; private set; }

        public FilterMatchedException(Uri uri)
        {
            Uri = uri;
        }
    }
}
