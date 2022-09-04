using IpService.ServiceIp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace IpService.Filters
{
    public class IpCheck:ActionFilterAttribute
    {
        private readonly IpList _ipList;
        public IpCheck(IOptions<IpList> ipList)
        {
            _ipList = ipList.Value;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var getIpAddress = context.HttpContext.Connection.RemoteIpAddress;
            var ipControl = _ipList.WhiteIpList.Any(x => IPAddress.Parse(x).Equals(getIpAddress));

            if (!ipControl)
            {
                context.Result = new StatusCodeResult((int)HttpStatusCode.Forbidden);
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}
