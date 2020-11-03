using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DungeonMaster.Library.Extensions.Microsoft.EntityFramework
{
    public static class DbSetExtensions
    {
        public static T FindOrCreate<T>(this DbSet<T> context, Func<T, Boolean> filter, Action<T> builder)
            where T : class
        {
            T instance = context.FirstOrDefault(filter);

            if(instance == default)
            {
                instance = context.GetService<T>();
                builder(instance);
                context.Add(instance);
            }

            return instance;
        }
    }
}
