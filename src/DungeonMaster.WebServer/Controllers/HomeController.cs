using DungeonMaster.Library.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DungeonMaster.WebServer.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private DungeonContext _context;

        public HomeController(DungeonContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("logs")]
        public IActionResult Logs()
        {
            return View(_context.LogMessages.AsQueryable().OrderByDescending(l => l.Timestamp).Take(250));
        }
    }
}
