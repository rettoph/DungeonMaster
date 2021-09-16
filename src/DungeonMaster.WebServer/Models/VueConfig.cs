using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DungeonMaster.WebServer.Models
{
    public class VueConfig
    {
        public String AppName { get; }
        public String ComponentName { get; }
        public String ScriptPath { get; }

        public VueConfig(String element, String componentName, String scriptPath)
        {
            AppName = element;
            ComponentName = componentName;
            ScriptPath = scriptPath;
        }
        public VueConfig(String componentName, String scriptPath) : this("app", componentName, scriptPath)
        {
        }
    }
}
