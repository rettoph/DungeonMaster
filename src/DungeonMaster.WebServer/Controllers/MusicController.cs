using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DungeonMaster.WebServer.Controllers
{
    [Authorize]
    [Route("Guilds/{currentGuildId}/Music")]
    public class MusicController : Controller
    {
        public IActionResult Index()
        {
            return View("VueApp", VueConstants.MusicIndex);
        }
    }
}
