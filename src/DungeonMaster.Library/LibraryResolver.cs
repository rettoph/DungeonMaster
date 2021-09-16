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
    internal static class LibraryResolver
    {
        public static OSPlatform CurrentPlatform;
        public static String RootDirectory;
        public static Dictionary<(String libraryName, OSPlatform platform, Architecture architecture), String> Lookup = new Dictionary<(String libraryName, OSPlatform platform, Architecture architecture), string>()
        {
            [(libraryName: "opus", platform: OSPlatform.Windows, architecture: Architecture.X64)] = "binaries\\win-x64\\libopus.dll",
            [(libraryName: "libsodium", platform: OSPlatform.Windows, architecture: Architecture.X64)] = "binaries\\win-x64\\libsodium.dll",
            [(libraryName: "ffmpeg", platform: OSPlatform.Windows, architecture: Architecture.X64)] = "binaries\\win-x64\\ffmpeg.exe",
            [(libraryName: "youtube-dl", platform: OSPlatform.Windows, architecture: Architecture.X64)] = "binaries\\win-x64\\youtube-dl.exe"
        };

        public static void Configure()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                LibraryResolver.CurrentPlatform = OSPlatform.Windows;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                LibraryResolver.CurrentPlatform = OSPlatform.Linux;
            }
            else
            {
                throw new Exception($"Unsupported OsPlatform.");
            }

            LibraryResolver.RootDirectory = Path.GetDirectoryName(typeof(LibraryResolver).Assembly.Location);

            NativeLibrary.SetDllImportResolver(typeof(DiscordSocketClient).Assembly, Resolve);
        }

        private static IntPtr Resolve(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
        {
            String realPath = GetPath(libraryName);

            if(realPath == default)
            {
                return NativeLibrary.Load(libraryName);
            }
            else
            {
                return NativeLibrary.Load(realPath);
            }
        }

        public static String GetPath(String libraryName)
        {
            var key = (libraryName: libraryName, platform: LibraryResolver.CurrentPlatform, architecture: RuntimeInformation.OSArchitecture);

            if (LibraryResolver.Lookup.TryGetValue(key, out String realPath))
                return Path.Combine(LibraryResolver.RootDirectory, realPath);

            return default;
        }
    }
}
