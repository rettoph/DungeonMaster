using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DungeonMaster.WebServer.ViewModels
{
    public class ChannelViewModel
    {
        public UInt64 Id { get; set; }
        public String Name { get; set; }
    }
}
