﻿using ColorMC.Core;
using ColorMC.Core.Objs;
using ColorMC.Core.Utils;

namespace ColorMC.Cmd;

public static class DownloadBar
{
    private static DownloadItemObj[] Items1;
    private static ProgressBar Bar;

    public static void Init()
    {
        ColorMCCore.DownloadItemStateUpdate = DownloadUpdate;
        ColorMCCore.DownloaderUpdate = DownloaderUpdate;

        Items1 = new DownloadItemObj[ConfigUtils.Config.Http.DownloadThread];
    }

    public static void DownloaderUpdate(CoreRunState state)
    {
        if (state == CoreRunState.Start)
        {
            ConsoleUtils.Info("开始下载文件");
            Console.ForegroundColor = ConsoleColor.White;
            Bar = new ProgressBar(ConfigUtils.Config.Http.DownloadThread);
        }
        else
        {
            Bar.Dispose();
        }
    }

    public static void DownloadUpdate(int index, DownloadItemObj item)
    {
        if (item.State == DownloadItemState.Done)
        {
            Items1[index] = null;
            Bar.Done(index, $"{item.Name} 下载完成");
        }
        else if (item.State == DownloadItemState.Error)
        {
            Console.WriteLine(item.Name);
        }
        else if (item.State != DownloadItemState.Init)
        {
            Items1[index] = item;
            Bar.SetName(index, item.Name);
            Bar.SetAllSize(index, item.AllSize);
            Bar.SetValue(index, item.NowSize);
        }

    }
}
