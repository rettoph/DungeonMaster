using DungeonMaster.Library;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace DungeonMaster.Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = DungeonMasterClient.BuildProvider().GetService<DungeonMasterClient>();

            // Start the client
            client.StartAsync();

            Console.ReadLine();
        }
    }
}
