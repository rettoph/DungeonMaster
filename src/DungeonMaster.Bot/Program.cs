using DungeonMaster.Library;
using Guppy;
using System;

namespace DungeonMaster.Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            var guppy = new GuppyLoader();
            var client = guppy
                .Initialize()
                .BuildServiceProvider()
                .GetService<DungeonMasterClient>();

            // Start the client
            client.StartAsync();

            Console.ReadLine();
        }
    }
}
