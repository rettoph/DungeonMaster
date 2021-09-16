using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DungeonMaster.Library.Extensions.EntityFrameworkCore
{
    public static class DbContextExtensions
    {
        public static void AddOrUpdate<T>(this DbSet<T> dbSet, T instance, Expression<Func<T, Boolean>> predicate)
            where T : class
        {
            var exists = dbSet.AsNoTracking().Any(predicate);

            if (exists)
            {
                dbSet.Update(instance);
            }
            else
            {
                dbSet.Add(instance);
            }
        }
    }
}
