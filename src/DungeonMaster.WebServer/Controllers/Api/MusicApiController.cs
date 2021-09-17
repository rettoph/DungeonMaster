using AutoMapper;
using Discord;
using Discord.WebSocket;
using DungeonMaster.Library;
using DungeonMaster.Library.Utilities;
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

        public IActionResult Index()
        {
            return this.Json(new MusicInfoViewModel()
            {
                NowPlaying = _user.CurrentGuild.Value.Music.Value.NowPlaying,
                Queue = _user.CurrentGuild.Value.Music.Value.Queue
            });
        }

        [Route("join")]
        public IActionResult Join()
        {
            SocketVoiceChannel currentVoiceChannel = this.GetCurrentVoiceChannel();

            _user.CurrentGuild.Value.Music.Value.ConnectAsync(currentVoiceChannel.Id);

            return this.Json(true);
        }

        [Route("play")]
        public async Task<IActionResult> Play(String videoId)
        {
            if (_user.CurrentGuild.Value.Music.Value.ConnectionState == ConnectionState.Disconnected)
                this.Join();

            _user.CurrentGuild.Value.Music.Value.Play(await Youtube.Info(videoId));

            return this.Index();
        }

        [Route("play-next")]
        public async Task<IActionResult> PlayNext(String videoId)
        {
            if (_user.CurrentGuild.Value.Music.Value.ConnectionState == ConnectionState.Disconnected)
                this.Join();

            _user.CurrentGuild.Value.Music.Value.PlayNext(await Youtube.Info(videoId));

            return this.Index();
        }

        [Route("stop")]
        public IActionResult Stop()
        {
            _user.CurrentGuild.Value.Music.Value.Stop();

            return this.Index();
        }

        [Route("enqueue")]
        public async Task<IActionResult> Enqueue(String videoId)
        {
            if (_user.CurrentGuild.Value.Music.Value.ConnectionState == ConnectionState.Disconnected)
                this.Join();

            _user.CurrentGuild.Value.Music.Value.Enqueue(await Youtube.Info(videoId));

            return this.Index();
        }

        private SocketVoiceChannel GetCurrentVoiceChannel()
        {
            SocketGuildUser currentGuildUser = _user.CurrentGuild.Value.SocketInstance.Value.GetUser(_user.CurrentUser.Value.Id);
            SocketVoiceChannel currentVoiceChannel = currentGuildUser.VoiceChannel;

            return currentVoiceChannel;
        }
    }
}
