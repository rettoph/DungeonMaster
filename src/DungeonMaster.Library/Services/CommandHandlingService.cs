using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DungeonMaster.Library.Database;
using DungeonMaster.Library.Utilities;
using DungeonMaster.Utilities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Library.Services
{
    public class CommandHandlingService
    {
        private readonly DungeonContext _dungeon;
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _discord;
        private readonly IServiceProvider _services;

        public CommandHandlingService(IServiceProvider services)
        {
            _dungeon = services.GetRequiredService<DungeonContext>();
            _commands = services.GetRequiredService<CommandService>();
            _discord = services.GetRequiredService<DiscordSocketClient>();
            _services = services;

            // Hook CommandExecuted to handle post-command-execution logic.
            _commands.CommandExecuted += CommandExecutedAsync;
            // Hook MessageReceived so we can process each message to see
            // if it qualifies as a command.
            _discord.MessageReceived += MessageReceivedAsync;
        }

        public async Task InitializeAsync()
        {
            // Register modules that are public and inherit ModuleBase<T>.
            foreach(Assembly ass in AssemblyHelper.Assemblies)
                await _commands.AddModulesAsync(ass, _services);
        }

        public async Task MessageReceivedAsync(SocketMessage rawMessage)
        {
            // Ignore system messages, or messages from other bots
            if (!(rawMessage is SocketUserMessage message)) return;
            if (message.Source != MessageSource.User) return;

            // This value holds the offset where the prefix ends
            var argPos = 0;
            // Perform prefix check. You may want to replace this with
            if (!message.HasCharPrefix('!', ref argPos)) return;

            // for a more traditional command format like !help.
            // if (!message.HasMentionPrefix(_discord.CurrentUser, ref argPos)) return;

            // var context = new SocketCommandContext(_discord, message);
            // Perform the execution of the command. In this method,
            // the command service will perform precondition and parsing check
            // then execute the command if one is matched.
            await this.TryExcecuteAsync(
                message.Content, 
                message.Author, 
                message.Channel, 
                (message.Channel as ITextChannel)?.Guild, 
                message);

            return;

            // Note that normally a result will be returned by this format, but here
            // we will handle the result in CommandExecutedAsync,
            await _commands.ExecuteAsync(new VirtualCommandContext()
            {
                Client = _discord,
                Channel = rawMessage.Channel,
                Guild = (rawMessage.Channel as SocketGuildChannel)?.Guild,
                Message = null,
                User = _discord.CurrentUser
            }, "role toggle <@&771512285679714325>", _services);
        }

        public async Task<IResult> TryExcecuteAsync(String command, IUser sender, IMessageChannel channel, IGuild guild, IUserMessage message = null)
        {
            var gm = _dungeon.GuildMasters.FirstOrDefault(gm => gm.Guild == guild);
            if (gm != default)
                gm.AuditLogger.Log(
                    message: $"**Executed:** {command}", gm.Guild.Users.FirstOrDefault(u => u.Id == sender.Id), Color.Blue);

            return await _commands.ExecuteAsync(new VirtualCommandContext()
            {
                Client = _discord,
                Channel = channel,
                Guild = guild,
                Message = message,
                User = message?.Author ?? _discord.CurrentUser
            }, command.Substring(1), _services);
        }

        public async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // command is unspecified when there was a search failure (command not found); we don't care about these errors
            if (!command.IsSpecified)
                return;

            // the command was successful, we don't care about this result, unless we want to log that a command succeeded.
            if (result.IsSuccess)
                return;

            // the command failed, let's notify the user that something happened.
            await context.Channel.SendMessageAsync($"error: {result}");
        }
    }
}
