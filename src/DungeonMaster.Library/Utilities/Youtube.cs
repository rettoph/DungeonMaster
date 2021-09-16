using CliWrap;
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

            try
            {
                using (MemoryStream youtube = new MemoryStream())
                {
                    var youtubeCommand = Cli.Wrap(LibraryResolver.GetPath("youtube-dl"))
                        .WithArguments($"https://www.youtube.com/watch?v={videoId} -o - ")
                        .WithStandardOutputPipe(PipeTarget.ToStream(youtube));

                    await youtubeCommand.ExecuteAsync();

                    youtube.Position = 0;

                    var ffmpegCommand = Cli.Wrap(LibraryResolver.GetPath("ffmpeg"))
                        .WithArguments("-hide_banner -loglevel panic -i pipe: -ac 2 -f s16le -ar 48000 pipe:1")
                        .WithStandardInputPipe(PipeSource.FromStream(youtube)) | ffmpeg;

                    await ffmpegCommand.ExecuteAsync();
                }

                return ffmpeg;
            }
            finally
            {
                await ffmpeg.FlushAsync();
                ffmpeg.Position = 0;
            }
        }
    }
}
