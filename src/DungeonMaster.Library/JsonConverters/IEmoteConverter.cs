using Discord;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace DungeonMaster.Library.JsonConverters
{
    class IEmoteConverter : JsonConverter<IEmote>
    {
        public override IEmote ReadJson(JsonReader reader, Type objectType, [AllowNull] IEmote existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            JObject o = JObject.Load(reader);

            switch((String)o.Property("type").Value)
            {
                case "emoji":
                    return new Emoji((String)o.Property("unicode"));
                case "emote":
                    return Emote.Parse((String)o.Property("name"));
            }

            return default;
        }

        public override void WriteJson(JsonWriter writer, [AllowNull] IEmote value, JsonSerializer serializer)
        {
            JObject o = new JObject();

            if(value is Emoji emoji)
            {
                o.Add("type", "emoji");
                o.Add("unicode", emoji.Name);
            }
            else if(value is Emote emote)
            {
                o.Add("type", "emote");
                o.Add("text", emote.ToString());
            }

            o.WriteTo(writer);
        }
    }
}
