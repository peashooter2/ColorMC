﻿namespace ColorMC.Core.LaunchPath;

public static class MCPath
{
    private const string Name = "minecraft";
    public static string BaseDir { get; private set; }

    public static void Init(string dir)
    {
        BaseDir = dir + Name;

        Logs.Info($"MC文件夹在{BaseDir}");

        Directory.CreateDirectory(BaseDir);

        AssetsPath.Init(BaseDir);
        LibrariesPath.Init(BaseDir);
        InstancesPath.Init(BaseDir);
        VersionPath.Init(BaseDir);
    }
}
