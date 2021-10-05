using Discord;
using Discord.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Library.Modules
{
    public class CatFactsModule : ModuleBase<SocketCommandContext>
    {
        public class CatFactJson
        {
            [JsonProperty("fact")]
            public String Fact { get; set; }

            [JsonProperty("length")]
            public Int32 Length { get; set; }
        }

        [Command("cat")]
        [Alias("catpic", "catfact")]
        public async Task PingAsync()
        {
            using (this.Context.Channel.EnterTypingState())
            {
                using (WebClient client = new WebClient())
                {
                    CatFactJson fact = JsonConvert.DeserializeObject<CatFactJson>(client.DownloadString("https://catfact.ninja/fact"));

                    EmbedBuilder response = new EmbedBuilder();

                    response.Description = fact.Fact;
                    response.WithImageUrl("attachment://catpic.jpg");

                    using (Stream webCat = client.OpenRead($"https://lorempixel.com/420/320/cats/?{Guid.NewGuid().GetHashCode()}"))
                    {
                        using (MemoryStream fileCat = new MemoryStream())
                        {
                            webCat.CopyTo(fileCat);
                            fileCat.Position = 0;

                            await this.Context.Channel.SendFileAsync(fileCat, "catpic.jpg", embed: response.Build());
                        }
                    }
                }
            }
        }

    }
}
