using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library.Database
{
    public struct ConnectionInfo
    {
        public String Host;
        public Int32 Port;
        public String User;
        public String Pass;
        public String Database;

        public override string ToString()
            => $"Server={this.Host};Port={this.Port};Database={this.Database};Uid={this.User};Pwd={this.Pass};";
    }
}
