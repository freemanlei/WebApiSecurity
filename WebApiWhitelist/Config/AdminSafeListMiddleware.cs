using System.Net;

namespace WebApiWhitelist.Config
{
    public class AdminSafeListMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AdminSafeListMiddleware> _logger;
        private readonly string _safelist;

        public AdminSafeListMiddleware(RequestDelegate next, ILogger<AdminSafeListMiddleware> logger, string safelist)
        {
            _next = next;
            _logger = logger;
            _safelist = safelist;
        }

        public async Task Invokke(HttpContext context)
        {
            if (context.Request.Method != HttpMethod.Get.Method)
            {
                var remoteIp = context.Connection.RemoteIpAddress;
                _logger.LogDebug("Request from Remote Ip address:{RemoteIp}", remoteIp);
                string[] ip = _safelist.Split(';');
                var bytes = remoteIp.GetAddressBytes();
                var badIp = true;
                foreach(var address in ip)
                {
                    var testIp = IPAddress.Parse(address); 
                    if(testIp.GetAddressBytes().SequenceEqual(bytes))
                    {
                        badIp = false;
                        break; 
                    }
                }

                if (badIp)
                {
                    _logger.LogWarning(
                        "Forbidden Request from Remote IP address: {RemoteIp}", remoteIp);
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return;
                }
            }

            await _next.Invoke(context);
        }
    }
}
