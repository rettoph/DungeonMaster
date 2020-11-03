using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DungeonMaster.Library.Interfaces
{
    public interface IServiceLoader
    {
        void RegisterServices(IServiceCollection services);
    }
}
