using Discord;
using Discord.Audio;
using Discord.WebSocket;
using DungeonMaster.Library.Enums;
using DungeonMaster.Library.Models;
using DungeonMaster.Library.Utilities;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DungeonMaster.Library.Services
{
    public class MusicService : IDisposable
    {
        private SocketGuild _guild;
        private CancellationTokenSource _playback;
        private List<Video> _playlist;
        private Video _nowPlaying;

        public Boolean Playing => _nowPlaying != default;

        public Video NowPlaying => _nowPlaying;

        public IReadOnlyList<Video> Queue => _playlist;

        public ConnectionState ConnectionState => _guild.AudioClient?.ConnectionState ?? ConnectionState.Disconnected;

        public MusicService(SocketGuild guild)
        {
            _guild = guild;
            _playlist = new List<Video>();
        }

        public async void ConnectAsync(UInt64 id)
        {
            try
            {
                if(this.ConnectionState == ConnectionState.Connected)
                {
                    if (_guild.CurrentUser.VoiceChannel.Id == id)
                    { // We are already in this channel...
                        return;
                    }
                    else
                    { // Disconnect first...
                        await _guild.CurrentUser.VoiceChannel.DisconnectAsync();
                    }
                }

                SocketVoiceChannel channel = _guild.GetVoiceChannel(id);

                if (channel == default)
                {
                    DungeonBot.Logger.Warning($"Attempted to join non-existant SocketCoiceChannel({channel.Id}) in Guild('{_guild.Name}')<{_guild.Id}>");
                }

                DungeonBot.Logger.Verbose($"Attempting to join SocketVoiceChannel('{channel.Name}')<{channel.Id}> in Guild('{_guild.Name}')<{_guild.Id}>");

                await _guild.GetVoiceChannel(id).ConnectAsync();
            }
            catch(Exception e)
            {
                DungeonBot.Logger.Critical(message: $"{e.Message}\n{e.StackTrace}", type: LogMessageType.System);
            }
        }

        public async void Play(Video video)
        {
            if (this.ConnectionState != ConnectionState.Connected && this.ConnectionState != ConnectionState.Connecting)
                return; // Make sure we're connected in discord...
            if (video == default)
                return; // Make sure the video exists

            // Ensure nothing is playing...
            this.Stop();

            // Create new local task
            var localPlayback = new CancellationTokenSource();
            _playback = localPlayback;
            _nowPlaying = video;

            await Task.Run(async () =>
            {
                using (var stream = await Youtube.GetStream(video.Id))
                {
                    using (var discord = _guild.AudioClient?.CreatePCMStream(AudioApplication.Mixed, 1024 * 96))
                    {
                        try
                        {
                            if (discord != default && stream != default)
                            {
                                Int32 bytes = 3840;
                                Byte[] buffer = new Byte[bytes];

                                DungeonBot.Logger.Verbose($"Now playing '{video.Snippet.Description}' ({video.Id})", _guild.Id);

                                while (stream.Position < stream.Length && this.Playing)
                                {
                                    if (localPlayback.Token.IsCancellationRequested)
                                    {
                                        _nowPlaying = default;
                                        DungeonBot.Logger.Verbose($"Playback stopped.", _guild.Id);
                                    }

                                    var read = await stream.ReadAsync(buffer, 0, bytes);
                                    await discord.WriteAsync(buffer, 0, read);
                                }

                                await stream.FlushAsync();
                            }
                        }
                        catch (TaskCanceledException e)
                        {
                            DungeonBot.Logger.Error($"Unexpected error occured during playback.\n{e.Message}", _guild.Id);
                        }
                        finally
                        {
                            this.Next();
                        }
                    }
                }

            }, _playback.Token);
        }

        public void Next()
        {
            this.Stop();

            if(_playlist.Any())
            {
                Video next = _playlist.ElementAt(0);
                _playlist.RemoveAt(0);

                DungeonBot.Logger.Verbose($"Playing next song: {next}");
                this.Play(next);
            }
        }

        public void Stop()
        {
            _playback?.Cancel();
            _playback = default;
        }

        public void Enqueue(Video video)
        {
            _playlist.Add(video);

            if (_playback == default)
                this.Next();
        }

        public void PlayNext(Video video)
        {
            _playlist.Insert(0, video);

            if (_playback == default)
                this.Next();
        }

        public void Dispose()
        {
            _guild = default;
        }
    }
}
