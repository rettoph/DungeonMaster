using AutoMapper;
using Discord;
using Discord.WebSocket;
using DungeonMaster.Library;
using DungeonMaster.WebServer.Services;
using DungeonMaster.WebServer.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DungeonMaster.WebServer.Controllers.Api
{
    [Authorize]
    [Route("api/guilds/{currentGuildId}/channels")]
    public class ChannelsApiController : Controller
    {
        private DiscordUser _user;
        private DungeonBot _bot;
        private IMapper _mapper;

        public ChannelsApiController(DiscordUser user, DungeonBot bot, IMapper mapper)
        {
            _user = user;
            _bot = bot;
            _mapper = mapper;
        }

        public IActionResult Index(ChannelType type)
        {
            IEnumerable<SocketGuildChannel> channels = _user.CurrentGuild.Value.GetChannels(type)
                .Where(channel => channel.GetUser(_user.CurrentUser.Value.Id) != default);

            IEnumerable<ChannelViewModel> channelViewModels = _mapper.Map<IEnumerable<ChannelViewModel>>(channels);

            return this.Json(channelViewModels);
        }
    }
}
