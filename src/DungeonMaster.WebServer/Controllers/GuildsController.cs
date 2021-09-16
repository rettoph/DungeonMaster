using DungeonMaster.WebServer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DungeonMaster.WebServer.Controllers
{
    [Authorize]
    [Route("Guilds/{currentGuildId}")]
    public class GuildsController : Controller
    {
        public GuildsController(DiscordUser user)
        {

        }

        public IActionResult Index()
        {
            return View("VueApp", VueConstants.GuildsIndex);
        }

        [Route("Categories")]
        public IActionResult Categories()
        {
            return View("VueApp", VueConstants.GuildsCategories);
        }
    }
}
