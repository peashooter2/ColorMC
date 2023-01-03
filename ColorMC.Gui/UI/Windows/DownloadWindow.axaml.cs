using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using ColorMC.Core;
using ColorMC.Core.Net.Downloader;
using ColorMC.Core.Objs;
using ColorMC.Core.Utils;
using ColorMC.Gui.UIBinding;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ColorMC.Gui.UI.Windows;

public partial class DownloadWindow : Window
{
    private readonly ObservableCollection<DownloadDisplayObj> List = new();
    private readonly Dictionary<string, DownloadDisplayObj> List1 = new();

    private bool pause = false;

    public DownloadWindow()
    {
        InitializeComponent();

        this.MakeItNoChrome();
        FontFamily = Program.FontFamily;

        CoreMain.DownloadItemStateUpdate = DownloadItemStateUpdate;

        DataGrid_Download.Items = List;

        if (App.BackBitmap != null)
        {
            Image_Back.Source = App.BackBitmap;
        }

        Expander_P.ContentTransition = new CrossFade(TimeSpan.FromMilliseconds(100));
        Expander_S.ContentTransition = new CrossFade(TimeSpan.FromMilliseconds(100));

        Button_P1.PointerLeave += Button_P1_PointerLeave;
        Button_P.PointerEnter += Button_P_PointerEnter;

        Button_S1.PointerLeave += Button_S1_PointerLeave;
        Button_S.PointerEnter += Button_S_PointerEnter;

        Button_P1.Click += Button_P_Click;
        Button_S1.Click += Button_S_Click;

        Closing += DownloadWindow_Closing;
        Closed += DownloadWindow_Closed;
        Opened += DownloadWindow_Opened;

        ProgressBar1.Value = 0;
    }

    private async void Button_S_Click(object? sender, RoutedEventArgs e)
    {
        var res = await Info.ShowWait("�Ƿ�Ҫֹͣ����");
        if (res)
        {
            List.Clear();
            List1.Clear();
            DownloadManager.Stop();
        }
    }

    private void Button_P_Click(object? sender, RoutedEventArgs e)
    {
        if (!pause)
        {
            DownloadManager.Pause();
            pause = true;
            Button_P.Content = "R";
            Button_P1.Content = "��������";
            Info2.Show("��������ͣ");
        }
        else
        {
            DownloadManager.Resume();
            Button_P.Content = "P";
            Button_P1.Content = "��ͣ����";
            pause = false;
            Info2.Show("�����Ѽ���");
        }
    }

    private void Button_S1_PointerLeave(object? sender, PointerEventArgs e)
    {
        Expander_S.IsExpanded = false;
    }

    private void Button_S_PointerEnter(object? sender, PointerEventArgs e)
    {
        Expander_S.IsExpanded = true;
    }

    private void Button_P1_PointerLeave(object? sender, PointerEventArgs e)
    {
        Expander_P.IsExpanded = false;
    }

    private void Button_P_PointerEnter(object? sender, PointerEventArgs e)
    {
        Expander_P.IsExpanded = true;
    }

    private void DownloadWindow_Opened(object? sender, EventArgs e)
    {
        DataGrid_Download.MakeTran();
        Expander_P.MakePadingNull();
        Expander_S.MakePadingNull();
    }

    private void DownloadWindow_Closed(object? sender, EventArgs e)
    {
        CoreMain.DownloadItemStateUpdate = null;
        App.DownloadWindow = null;
    }

    private void DownloadWindow_Closing(object? sender, CancelEventArgs e)
    {
        if (List.Count != 0)
        {
            Info.Show("���ػ�δ���");
            e.Cancel = true;
        }
    }

    public void DownloadItemStateUpdate(int index, DownloadItem item)
    {
        if (item.State == DownloadItemState.Init)
        {
            var item1 = new DownloadDisplayObj()
            {
                Name = item.Name,
                State = item.State.GetName(),
            };
            List.Add(item1);
            List1.Add(item.Name, item1);

            return;
        }

        Dispatcher.UIThread.Post(() =>
        {
            if (!List1.ContainsKey(item.Name))
                return;

            List1[item.Name].State = item.State.GetName();

            if (item.State == DownloadItemState.Done
                && List1.TryGetValue(item.Name, out var item1))
            {
                var data = OtherBinding.GetDownloadState();
                Load();
                Label1.Content = $"{(double)data.Item2 / data.Item1 * 100:0.##}";
                List.Remove(item1);
            }
            else if (item.State == DownloadItemState.GetInfo)
            {
                List1[item.Name].AllSize = $"{(double)item.AllSize / 1000 / 1000:0.##}";
            }
            else if (item.State == DownloadItemState.Download)
            {
                List1[item.Name].NowSize = $"{(double)item.NowSize / item.AllSize * 100:0.##} %";
            }
            else if (item.State == DownloadItemState.Error)
            {
                List1[item.Name].ErrorTime = item.ErrorTime;
            }
        });
    }

    public void Load()
    {
        var data = OtherBinding.GetDownloadState();
        ProgressBar1.Maximum = data.Item1;
        ProgressBar1.Value = data.Item2;
    }
}