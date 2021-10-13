using Discord;
using Discord.Audio;
using Discord.WebSocket;
using DungeonMaster.Library.Enums;
using DungeonMaster.Library.Models;
using DungeonMaster.Library.Structs;
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
        #region Private Fields
        /// <summary>
        /// The current <see cref="SocketGuild"/> this <see cref="MusicService"/>
        /// manages.
        /// </summary>
        private SocketGuild _guild;
        private List<PlaybackRequest> _playlist;
        private PlaybackRequest? _nowPlaying;
        private ITextChannel _textChannel;
        private SocketVoiceChannel _voiceChannel;
        #endregion

        public Boolean Playing => _nowPlaying is not null;

        public PlaybackRequest? NowPlaying => _nowPlaying;

        public IReadOnlyList<PlaybackRequest> Playlist => _playlist;

        public ConnectionState ConnectionState => _guild.AudioClient?.ConnectionState ?? ConnectionState.Disconnected;

        public MusicService(SocketGuild guild)
        {
            _guild = guild;
            _playlist = new List<PlaybackRequest>();
        }

        #region Audio Methods
        public async Task ConnectAsync(UInt64 id, ITextChannel text = default)
        {
            try
            {
                this.SetTextChannel(text, true);

                if(this.ConnectionState == ConnectionState.Connected)
                { // There is already a connected audio client...
                    if (_guild.CurrentUser.VoiceChannel.Id == id)
                    { // We are already in the requested channel...
                        return;
                    }
                    else
                    { // Disconnect from the old channel first...
                        await this.Disconnect();
                    }
                }

                // Search for the requested channel...
                _voiceChannel = _guild.GetVoiceChannel(id);

                if (_voiceChannel == default)
                {
                    DungeonBot.Logger.Error($"Attempted to join non-existant SocketVoiceChannel<{_voiceChannel.Id}> in Guild('{_guild.Name}')<{_guild.Id}>", _guild.Id);
                    return;
                }

                DungeonBot.Logger.Verbose($"Attempting to join SocketVoiceChannel('{_voiceChannel.Name}')<{_voiceChannel.Id}> in Guild('{_guild.Name}')<{_guild.Id}>", _guild.Id);

                await _voiceChannel.ConnectAsync();

                DungeonBot.Logger.Verbose($"Connected to SocketVoiceChannel('{_voiceChannel.Name}')<{_voiceChannel.Id}> in Guild('{_guild.Name}')<{_guild.Id}>", _guild.Id);
            }
            catch(Exception e)
            {
                DungeonBot.Logger.Critical(message: $"{e.Message}\n{e.StackTrace}", type: LogMessageType.System);
            }
        }

        public async Task Play(PlaybackRequest request)
        {
            // Ensure nothing is playing...
            this.Stop();

            // Create new local task
            _nowPlaying = request;

            await Task.Run(async () =>
            {
                DungeonBot.Logger.Verbose($"MusicService::Play - Attempting to play {request}.", _guild.Id);

                using (var stream = await Youtube.GetStream(request.Video.Id, request.CancellationTokenSource.Token))
                {
                    using (var discord = _guild.AudioClient.CreatePCMStream(AudioApplication.Mixed, 1024 * 96))
                    {
                        try
                        {
                            if (discord != default && stream != default)
                            {
                                DungeonBot.Logger.Verbose($"MusicService::Play - Now playing {request}.", _guild.Id);

                                Int32 bytes = 3840;
                                Byte[] buffer = new Byte[bytes];
                                while (stream.Position < stream.Length && this.Playing)
                                {
                                    if (request.CancellationTokenSource.Token.IsCancellationRequested)
                                    {
                                        DungeonBot.Logger.Verbose($"MusicService::Play - Playback stopped for {request}.", _guild.Id);
                                        break;
                                    }

                                    var read = await stream.ReadAsync(buffer, 0, bytes);
                                    await discord.WriteAsync(buffer, 0, read);
                                }

                                await stream.FlushAsync();
                            }
                        }
                        catch (TaskCanceledException e)
                        {
                            DungeonBot.Logger.Error($"MusicService::Play - Unexpected error occured during playback for {request}.\n{e.Message}", _guild.Id);
                        }
                        finally
                        {
                            DungeonBot.Logger.Verbose($"MusicService::Play - Playback for {request} finished.", _guild.Id);

                            if(_nowPlaying is not null & _nowPlaying.Value.Id == request.Id)
                            {
                                this.Stop();
                            }

                            this.PlayNext();
                        }
                    }
                }

            }, request.CancellationTokenSource.Token);
        }

        public async Task PlayNext()
        {
            PlaybackRequest? next = default;

            lock(_playlist)
            {
                if (_playlist.Any())
                {
                    next = _playlist.ElementAt(0);
                    _playlist.RemoveAt(0);
                }
            }

            if (next is null)
                return;

            await this.Play(next.Value);
        }

        public void Stop()
        {
            if (_nowPlaying is null)
                return;

            DungeonBot.Logger.Verbose($"Stopping playback for {_nowPlaying}", _guild.Id);

            _nowPlaying.Value.CancellationTokenSource.Cancel();
            _nowPlaying = default;
        }

        public void Enqueue(PlaybackRequest request)
        {
            _playlist.Add(request);

            if (_nowPlaying is null)
            {
                this.PlayNext();
            }
        }

        public void PlayNext(PlaybackRequest request)
        {
            _playlist.Insert(0, request);

            if (_nowPlaying is null)
            {
                this.PlayNext();
            }
        }

        public async Task Disconnect()
        {
            await _guild.CurrentUser.VoiceChannel.DisconnectAsync();
            _voiceChannel = default;
            _textChannel = default;
        }
        #endregion

        #region Helper Methods
        public void SetTextChannel(ITextChannel channel, Boolean force = false)
        {
            if(channel is null && !force)
            {
                return;
            }

            _textChannel = channel;

            if(_textChannel is not null)
                DungeonBot.Logger.Verbose($"Bound to ITextChannel<{_textChannel.Id}> in Guild('{_guild.Name}')<{_guild.Id}>", _guild.Id);
        }
        #endregion

        #region IDisposable Implementation
        public void Dispose()
        {
            _guild = default;
        }
        #endregion
    }
}
