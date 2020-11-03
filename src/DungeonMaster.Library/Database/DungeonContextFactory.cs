using DungeonMaster.Utilities;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace DungeonMaster.Library.Database
{
    public class DungeonContextFactory : IDesignTimeDbContextFactory<DungeonContext>
    {
        public DungeonContext CreateDbContext(string[] args)
        {
            AssemblyHelper.AddAssembly(Assembly.GetAssembly(typeof(DungeonContext)));

            return DungeonMasterClient.BuildProvider().GetService<DungeonContext>();
        }
    }
}
