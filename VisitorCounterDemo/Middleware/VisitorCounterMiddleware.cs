using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using VisitorCounter.Util;

namespace VisitorCounter.Middleware
{
    public class VisitorCounterMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public VisitorCounterMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            var visitorId = context.Request.Cookies["VisitorId"];
            
            if (visitorId == null)
            {
                visitorId = Guid.NewGuid().ToString();
                //don the necessary staffs here to save the count by one
                context.Response.Cookies.Append("VisitorId", visitorId, new CookieOptions()
                {
                    Path = "/",
                    HttpOnly = true,
                    Secure = false,
                    Expires = DateTime.Now.AddMonths(1)
                });
            }
            VisitCounter.Visit(visitorId);

            await _requestDelegate(context);
        }
    }

    public static class UseVisitorCounterExtension
    {
        public static IApplicationBuilder UseVisitorCounter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<VisitorCounterMiddleware>();
        }
    }
}
