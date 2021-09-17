using Discord.Audio;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DungeonMaster.Library
{
    internal static class Libraries
    {
        public static String FFMPEG { get; private set; }
        public static String YOUTUBE_DL { get; private set; }
        public static String LIB_OPUS { get; private set; }
        public static String LIB_SODIUM { get; private set; }

        public static void Configure(string ffmpeg, string youtubeDl, string libOpus, string libSodium)
        {
            FFMPEG = ffmpeg;
            YOUTUBE_DL = youtubeDl;
            LIB_OPUS = libOpus;
            LIB_SODIUM = libSodium;

            NativeLibrary.SetDllImportResolver(typeof(DiscordSocketClient).Assembly, Resolve);
        }

        private static IntPtr Resolve(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            switch(libraryName)
            {
                case "opus":
                    return NativeLibrary.Load(LIB_OPUS);
                case "libsodium":
                    return NativeLibrary.Load(LIB_SODIUM);
                default:
                    return NativeLibrary.Load(libraryName);
            }
        }
    }
}
