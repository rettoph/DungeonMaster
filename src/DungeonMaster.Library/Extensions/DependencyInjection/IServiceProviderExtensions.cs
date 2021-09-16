using DungeonMaster.Library.Database;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Library.Extensions.DependencyInjection
{
    public static class IServiceProviderExtensions
    {
        public static void UsingContext(this IServiceProvider provider, Action<DungeonContext> action)
        {
            using (DungeonContext context = provider.GetRequiredService<DungeonContext>())
            {
                action(context);
            }
        }

        public static T UsingContext<T>(this IServiceProvider provider, Func<DungeonContext, T> func)
        {
            using (DungeonContext context = provider.GetRequiredService<DungeonContext>())
            {
                return func(context);
            }
        }

        public static async Task UsingContextAsync(this IServiceProvider provider, Func<DungeonContext, Task> func)
        {
            using (DungeonContext context = provider.GetRequiredService<DungeonContext>())
            {
                await func(context);
            }
        }

        public static async Task<T> UsingContextAsync<T>(this IServiceProvider provider, Func<DungeonContext, Task<T>> func)
        {
            using (DungeonContext context = provider.GetRequiredService<DungeonContext>())
            {
                return await func(context);
            }
        }
    }
}
