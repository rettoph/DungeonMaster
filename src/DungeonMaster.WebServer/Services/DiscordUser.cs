using Discord.WebSocket;
using DungeonMaster.Library;
using DungeonMaster.Library.Database;
using DungeonMaster.Library.Models;
using DungeonMaster.WebServer.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace DungeonMaster.WebServer.Services
{
    public class DiscordUser : IDisposable
    {
        private HttpClient _client;
        private DungeonBot _bot;
        private UInt64 _currentGuildId;
        private UInt64 _currentUserId;
        private DungeonContext _context;

        public Lazy<Task<IEnumerable<Guild>>> Guilds { get; private set; }
        public Lazy<Guild> CurrentGuild { get; private set; }
        public Lazy<SocketUser> CurrentUser { get; private set; }

        public DiscordUser(DungeonBot bot, IHttpContextAccessor httpContextAccessor)
        {
            _bot = bot;

            var accessToken = httpContextAccessor.HttpContext.GetTokenAsync("access_token").GetAwaiter().GetResult();

            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            this.Guilds = new Lazy<Task<IEnumerable<Guild>>>(this.GetGuilds);
            this.CurrentGuild = new Lazy<Guild>(this.GetCurrentGuild);
            this.CurrentUser = new Lazy<SocketUser>(this.GetCurrentUser);

            if(httpContextAccessor.HttpContext.User.Identity is ClaimsIdentity claimsIdentity && claimsIdentity.IsAuthenticated)
            {
                _currentUserId = UInt64.Parse(claimsIdentity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value);
            }
        }

        private async Task<IEnumerable<Guild>> GetGuilds()
        {
            var userGuilds = await this.Get<IEnumerable<GuildDto>>("users/@me/guilds");
            Guild guild;
            List<Guild> guilds = new List<Guild>();

            foreach (GuildDto userGuild in userGuilds)
            {
                if((guild = _bot.Guilds.FirstOrDefault(g => g.Id == userGuild.Id)) != default)
                {
                    guilds.Add(guild);
                }
            }

            return guilds;
        }

        private Guild GetCurrentGuild()
        {
            SocketGuild socketGuild = this.CurrentUser.Value?.MutualGuilds.FirstOrDefault(g => g.Id == _currentGuildId);

            if (socketGuild == default)
                return default;

            return _bot.Guilds.FirstOrDefault(g => g.Id == socketGuild.Id);
        }

        private SocketUser GetCurrentUser()
            => _bot.GetUser(_currentUserId);

        private async Task<T> Get<T>(String path)
        {
            Stream resultStream = await _client.GetStreamAsync($"https://discordapp.com/api/{path}");
            T result = await JsonSerializer.DeserializeAsync<T>(resultStream);

            return result;
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        internal void SetCurrentGuildId(UInt64 id)
            => _currentGuildId = id;
    }
}
