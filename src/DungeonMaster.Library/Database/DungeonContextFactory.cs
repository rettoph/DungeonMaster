using Guppy;
using Guppy.Utilities;
using Microsoft.EntityFrameworkCore.Design;
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

            return new GuppyLoader()
                .Initialize()
                .BuildServiceProvider()
                .GetService<DungeonContext>();
        }
    }
}
