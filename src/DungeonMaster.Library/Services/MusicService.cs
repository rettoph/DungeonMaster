using Discord;
using Discord.Audio;
using Discord.WebSocket;
using DungeonMaster.Library.Enums;
using DungeonMaster.Library.Models;
using DungeonMaster.Library.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Library.Services
{
    public class MusicService : IDisposable
    {
        private Guild _guild;

        public MusicService(Guild guild)
        {
            _guild = guild;
        }

        public async void ConnectAsync(UInt64 id)
        {
            try
            {
                if((_guild.SocketInstance.Value.AudioClient?.ConnectionState ?? ConnectionState.Disconnected) == ConnectionState.Connected)
                {
                    if (_guild.SocketInstance.Value.CurrentUser.VoiceChannel.Id == id)
                    { // We are already in this channel...
                        return;
                    }
                    else
                    { // Disconnect first...
                        await _guild.SocketInstance.Value.CurrentUser.VoiceChannel.DisconnectAsync();
                    }
                }

                SocketVoiceChannel channel = _guild.SocketInstance.Value.GetVoiceChannel(id);

                if (channel == default)
                {
                    DungeonBot.Logger.Warning($"Attempted to join non-existant SocketCoiceChannel({channel.Id}) in Guild('{_guild.Name}')<{_guild.Id}>");
                }

                DungeonBot.Logger.Info($"Attempting to join SocketVoiceChannel('{channel.Name}')<{channel.Id}> in Guild('{_guild.Name}')<{_guild.Id}>");

                await _guild.SocketInstance.Value.GetVoiceChannel(id).ConnectAsync();
            }
            catch(Exception e)
            {
                DungeonBot.Logger.Critical($"{e.Message}\n{e.StackTrace}", type: LogMessageType.System);
            }
        }

        public async void Play(String videoId)
        {
            if (_guild.SocketInstance.Value.AudioClient == default)
                return;

            using (var stream = await Youtube.GetStream(videoId))
            {
                using (var discord = _guild.SocketInstance.Value.AudioClient.CreatePCMStream(AudioApplication.Mixed))
                {
                    try
                    {
                        await stream.CopyToAsync(discord);
                    }
                    catch(TaskCanceledException e)
                    {
                        DungeonBot.Logger.Warning($"MusicService Task Canceled Exception.", _guild.Id);
                    }
                    finally
                    {
                        await stream.FlushAsync();
                    }
                }
            }
        }

        public async void Stop()
        {
            if (_guild.SocketInstance.Value.AudioClient == default)
                return;

            await _guild.SocketInstance.Value.AudioClient.StopAsync();
        }

        public void Dispose()
        {
            _guild = default;
        }
    }
}
