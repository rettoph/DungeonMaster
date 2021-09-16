using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DungeonMaster.WebServer.Dtos
{
    public class GuildDto
    {
        [JsonPropertyName("id")]
        [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
        public UInt64 Id { get; set; }

        [JsonPropertyName("name")]
        public String Name { get; set; }

        [JsonPropertyName("icon")]
        public String Icon { get; set; }

        public String IconUrl => $"https://cdn.discordapp.com/icons/{this.Id}/{this.Icon}";
    }
}
