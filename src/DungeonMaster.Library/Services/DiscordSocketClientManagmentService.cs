using Discord;
using Discord.WebSocket;
using DungeonMaster.Library.Enums;
using DungeonMaster.Library.Extensions.DependencyInjection;
using DungeonMaster.Library.Extensions.EntityFrameworkCore;
using DungeonMaster.Library.Models;
using DungeonMaster.Library.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DungeonMaster.Library.Services
{
    public class DiscordSocketClientManagmentService : IHostedService
    {
        #region Private Fields
        private DiscordSocketClient _client;
        private IConfiguration _configuration;
        private IServiceProvider _provider;
        #endregion

        #region Constructor
        public DiscordSocketClientManagmentService(DiscordSocketClient client, Logger logger, IConfiguration configuration, IServiceProvider provider)
        {
            _client = client;
            _configuration = configuration;
            _provider = provider;

            DungeonBot.ClientInstance = client;
            DungeonBot.Logger = logger;
        }
        #endregion

        #region IHostedService Implementation
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            String token = _configuration["Discord:Bot:Token"];

            _client.LoggedIn += this.HandleClientLoggedIn;
            _client.LoggedOut += this.HandleClientLoggedOut;
            _client.CurrentUserUpdated += this.HandleCurrentUserUpdated;
            _client.Ready += this.HandleReady;
            _client.GuildAvailable += this.HandleGuildAvailable;
            _client.Log += this.HandleLog;

            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _client.LogoutAsync();
            _client.Dispose();
        }
        #endregion

        #region Event Handlers
        private Task HandleClientLoggedIn()
        {
            DungeonBot.Logger.Verbose($"DungeonMaster has logged in.");

            return Task.CompletedTask;
        }

        private Task HandleClientLoggedOut()
        {
            DungeonBot.Logger.Verbose($"DungeonMaster has logged out.");

            return Task.CompletedTask;
        }

        private Task HandleCurrentUserUpdated(SocketSelfUser arg1, SocketSelfUser arg2)
        {
            DungeonBot.Logger.Verbose($"DungeonMaster's current user has been updated => {_client.CurrentUser?.Username}({_client.CurrentUser?.Id}).");

            return Task.CompletedTask;
        }

        private Task HandleReady()
        {
            DungeonBot.Logger.Verbose($"DungeonMaster is ready.");

            return Task.CompletedTask;
        }

        private async Task HandleGuildAvailable(SocketGuild arg)
        {
            await _provider.UsingContextAsync(async context =>
            {
                context.Guilds.AddOrUpdate(new Guild()
                {
                    Id = arg.Id,
                    Name = arg.Name
                }, g => g.Id == arg.Id);

                await context.SaveChangesAsync();

                DungeonBot.Logger.Verbose($"Guild('{arg.Name}')<{arg.Id}> is now available to DungeonMaster.", arg.Id);
            });
        }

        private Task HandleLog(Discord.LogMessage arg)
        {
            String message = arg.Exception == default ? arg.Message : $"{arg.Message}\n{arg.Exception.ToString()}";

            switch (arg.Severity)
            {
                case LogSeverity.Critical:
                    DungeonBot.Logger.Critical(message: message, type: LogMessageType.Discord);
                    break;
                case LogSeverity.Error:
                    DungeonBot.Logger.Error(message: message, type: LogMessageType.Discord);
                    break;
                case LogSeverity.Warning:
                    DungeonBot.Logger.Warning(message: message, type: LogMessageType.Discord);
                    break;
                case LogSeverity.Info:
                    DungeonBot.Logger.Info(message: message, type: LogMessageType.Discord);
                    break;
                case LogSeverity.Verbose:
                    DungeonBot.Logger.Verbose(message: message, type: LogMessageType.Discord);
                    break;
                case LogSeverity.Debug:
                    DungeonBot.Logger.Debug(message: message, type: LogMessageType.Discord);
                    break;
            }

            return Task.CompletedTask;
        }
        #endregion
    }
}
