using CliWrap;
using DungeonMaster.Library.Enums;
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
        public static async Task<Stream> GetStream(String videoId)
        {
            MemoryStream ffmpeg = new MemoryStream();

            using(MemoryStream error = new MemoryStream())
            {
                try
                {
                    using (MemoryStream youtube = new MemoryStream())
                    {
                        var youtubeCommand = Cli.Wrap(LibraryResolver.youtubeDl)
                            .WithArguments($"https://www.youtube.com/watch?v=G9RA5v9Hy44 -o - ")
                            .WithStandardOutputPipe(PipeTarget.ToStream(youtube))
                            .WithStandardErrorPipe(PipeTarget.ToStream(error));

                        await youtubeCommand.ExecuteAsync();

                        youtube.Position = 0;

                        var ffmpegCommand = Cli.Wrap(LibraryResolver.ffmpeg)
                            .WithArguments("-hide_banner -loglevel panic -i pipe: -ac 2 -f s16le -ar 48000 pipe:1")
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
    }
}
