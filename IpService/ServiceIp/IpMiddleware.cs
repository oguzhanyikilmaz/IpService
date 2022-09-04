using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IpService.ServiceIp
{
    public class IpMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IpList _ipList;
        public IpMiddleware(RequestDelegate next,IOptions<IpList> ipList)
        {
            _next = next;
            _ipList = ipList.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var getIpAddress = httpContext.Connection.RemoteIpAddress;
            var whiteIpList = _ipList.WhiteIpList.Any(x=>IPAddress.Parse(x).Equals(getIpAddress));

            if (!whiteIpList)
            {
                httpContext.Response.StatusCode = (int)StatusCodes.Status403Forbidden;
                return;
            }
            await _next(httpContext);
        }
    }
}
