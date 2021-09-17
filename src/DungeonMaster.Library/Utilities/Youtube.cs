using CliWrap;
using DungeonMaster.Library.Enums;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DungeonMaster.Library.Utilities
{
    public static class Youtube
    {
        private static YouTubeService _service;

        internal static void Configure(String name, String key)
        {
            _service = new YouTubeService(new BaseClientService.Initializer()
            {
                ApplicationName = name,
                ApiKey = key
            });
        }

        #region Data Methods
        public static async Task<Video> Info(String videoId)
        {
            VideosResource.ListRequest request = _service.Videos.List(new Repeatable<String>(new[] { "snippet", "contentDetails" }));
            request.Id = videoId;

            VideoListResponse response = await request.ExecuteAsync();

            foreach(var result in response.Items)
            {
                return result;
            }

            return default;
        }

        public static async Task<IEnumerable<Video>> Info(IEnumerable<String> videoIds)
        {
            VideosResource.ListRequest request = _service.Videos.List(new Repeatable<String>(new[] { "snippet", "contentDetails" }));
            request.Id = new Repeatable<String>(videoIds);

            VideoListResponse response = await request.ExecuteAsync();

            return response.Items;
        }

        public static async Task<IEnumerable<Video>> Search(String query)
        {
            SearchResource.ListRequest request = _service.Search.List("id");
            request.Q = query;
            request.MaxResults = 10;
            request.Type = "video";

            SearchListResponse response = await request.ExecuteAsync();

            return await Youtube.Info(response.Items.Select(result => result.Id.VideoId));
        }
        #endregion

        #region Stream Methods
        public static async Task<Stream> GetStream(String videoId)
        {
            MemoryStream ffmpeg = new MemoryStream();

            using(MemoryStream error = new MemoryStream())
            {
                try
                {
                    using (MemoryStream youtube = new MemoryStream())
                    {
                        var youtubeCommand = Cli.Wrap(Libraries.YOUTUBE_DL)
                            .WithArguments($"https://www.youtube.com/watch?v={videoId} -o - ")
                            .WithStandardOutputPipe(PipeTarget.ToStream(youtube))
                            .WithStandardErrorPipe(PipeTarget.ToStream(error));

                        await youtubeCommand.ExecuteAsync();

                        youtube.Position = 0;

                        var ffmpegCommand = Cli.Wrap(Libraries.FFMPEG)
                            .WithArguments("-hide_banner -loglevel panic -i pipe: -ac 2 -f s16le -ar 48000 -filter:a \"volume = 0.25\" pipe:1")
                            .WithStandardInputPipe(PipeSource.FromStream(youtube)) | ffmpeg;

                        await ffmpegCommand.ExecuteAsync();
                    }

                    return ffmpeg;
                }
                catch (Exception e)
                {
                    error.Position = 0;

                    using(StreamReader reader = new StreamReader(error))
                    {
                        DungeonBot.Logger.Critical(reader.ReadToEnd());
                        DungeonBot.Logger.Critical(message: e.Message, type: LogMessageType.System);
                    }

                    return default;
                }
                finally
                {
                    await ffmpeg.FlushAsync();
                    ffmpeg.Position = 0;
                }
            }
        }
        #endregion
    }
}
