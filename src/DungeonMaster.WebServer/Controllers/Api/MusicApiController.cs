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
    [Route("api/guilds/{currentGuildId}/music")]
    public class MusicApiController : Controller
    {
        private DiscordUser _user;
        private DungeonBot _bot;
        private IMapper _mapper;

        public MusicApiController(DiscordUser user, DungeonBot bot, IMapper mapper)
        {
            _user = user;
            _bot = bot;
            _mapper = mapper;
        }

        public IActionResult Index(ChannelType type)
        {
            SocketVoiceChannel currentVoiceChannel = this.GetCurrentVoiceChannel();

            ChannelViewModel channelViewModel = _mapper.Map<ChannelViewModel>(currentVoiceChannel);

            return this.Json(channelViewModel);
        }

        [Route("join")]
        public IActionResult Join()
        {
            SocketVoiceChannel currentVoiceChannel = this.GetCurrentVoiceChannel();

            _user.CurrentGuild.Value.Music.Value.ConnectAsync(currentVoiceChannel.Id);

            return this.Json(true);
        }

        [Route("play")]
        public IActionResult Play(String videoId)
        {
            this.Join();

            _user.CurrentGuild.Value.Music.Value.Play(videoId);

            return this.Json(true);
        }

        [Route("stop")]
        public IActionResult Stop()
        {
            _user.CurrentGuild.Value.Music.Value.Stop();

            return this.Json(true);
        }

        private SocketVoiceChannel GetCurrentVoiceChannel()
        {
            SocketGuildUser currentGuildUser = _user.CurrentGuild.Value.SocketInstance.Value.GetUser(_user.CurrentUser.Value.Id);
            SocketVoiceChannel currentVoiceChannel = currentGuildUser.VoiceChannel;

            return currentVoiceChannel;
        }
    }
}
