
using DungeonMaster.WebServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System;
using System.Threading.Tasks;

namespace DungeonMaster.WebServer.Middleware
{
    public class SetCurrentGuildMiddleware
    {
        private readonly RequestDelegate _next;

        public SetCurrentGuildMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, DiscordUser discord)
        {
            var routeData = httpContext.GetRouteData();

            if (routeData.Values.TryGetValue("currentGuildId", out Object currentGuildIdObject) && UInt64.TryParse(currentGuildIdObject as String, out UInt64 currentGuildId))
            {
                discord.SetCurrentGuildId(currentGuildId);
            }

            await _next(httpContext);
        }
    }
}
