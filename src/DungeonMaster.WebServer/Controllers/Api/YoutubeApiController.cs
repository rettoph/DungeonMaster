using DungeonMaster.Library.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DungeonMaster.WebServer.Controllers.Api
{
    [Authorize]
    [Route("api/youtube")]
    public class YoutubeApiController : Controller
    {
        [Route("search")]
        public async Task<IActionResult> Search(String query)
        {
            var result = await Youtube.Search(query);

            return this.Json(result);
        }

        [Route("info")]
        public async Task<IActionResult> Info(String videoId)
        {
            var result = await Youtube.Info(videoId);

            return this.Json(result);
        }
    }
}
