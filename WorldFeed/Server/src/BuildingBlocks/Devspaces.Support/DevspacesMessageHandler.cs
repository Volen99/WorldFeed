﻿namespace Devspaces.Support
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;

    public class DevspacesMessageHandler : DelegatingHandler
    {
        private const string DevspacesHeaderName = "azds-route-as";
        private readonly IHttpContextAccessor httpContextAccessor;
        public DevspacesMessageHandler(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var req = this.httpContextAccessor.HttpContext.Request;

            if (req.Headers.ContainsKey(DevspacesHeaderName))
            {
                request.Headers.Add(DevspacesHeaderName, req.Headers[DevspacesHeaderName] as IEnumerable<string>);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
