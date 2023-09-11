﻿using ColorMC.Core;
using ColorMC.Core.Objs;
using ColorMC.Gui.UI.Model.Main;
using ColorMC.Gui.UIBinding;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ColorMC.Gui.UI.Model.Add.AddGame;

public partial class AddGameModel : TopModel
{
    public List<string> PackTypeList { get; init; } = LanguageBinding.GetPackType();

    [ObservableProperty]
    private string _zipLocal;

    [ObservableProperty]
    private int _type = -1;

    partial void OnZipLocalChanged(string value)
    {
        Type = GameBinding.CheckType(value) switch
        {
            PackType.CurseForge => 1,
            PackType.Modrinth => 2,
            PackType.MMC => 3,
            PackType.HMCL => 4,
            _ => 0
        };
    }

    [RelayCommand]
    public void AddPackGame()
    {
        if (BaseBinding.IsDownload)
        {
            Model.Show(App.GetLanguage("AddGameWindow.Tab1.Error4"));
            return;
        }
        if (Type == -1)
        {
            Model.Show(App.GetLanguage("AddGameWindow.Tab2.Error3"));
            return;
        }

        switch (Type)
        {
            case 0:
                AddPack(PackType.ColorMC);
                break;
            case 1:
                AddPack(PackType.CurseForge);
                break;
            case 2:
                AddPack(PackType.Modrinth);
                break;
            case 3:
                AddPack(PackType.MMC);
                break;
            case 4:
                AddPack(PackType.HMCL);
                break;
        }
    }

    [RelayCommand]
    public async Task SelectPack()
    {
        var res = await PathBinding.SelectFile(FileType.ModPack);
        if (!string.IsNullOrWhiteSpace(res))
        {
            ZipLocal = res;
        }
    }

    private void PackUpdate(int size, int now)
    {
        Model.ProgressUpdate((double)now / size);
    }

    private void PackState(CoreRunState state)
    {
        if (state == CoreRunState.Read)
        {
            Model.Progress(App.GetLanguage("AddGameWindow.Tab2.Info1"));
        }
        else if (state == CoreRunState.Init)
        {
            Model.ProgressUpdate(App.GetLanguage("AddGameWindow.Tab2.Info2"));
        }
        else if (state == CoreRunState.GetInfo)
        {
            Model.ProgressUpdate(App.GetLanguage("AddGameWindow.Tab2.Info3"));
        }
        else if (state == CoreRunState.Download)
        {
            Model.ProgressUpdate(App.GetLanguage("AddGameWindow.Tab2.Info4"));
            Model.ProgressUpdate(-1);
        }
        else if (state == CoreRunState.End)
        {
            Name = "";
            Group = "";
        }
    }

    private async void AddPack(PackType type)
    {
        ColorMCCore.GameOverwirte = Tab2GameOverwirte;

        if (string.IsNullOrWhiteSpace(ZipLocal))
        {
            Model.Show(App.GetLanguage("AddGameWindow.Tab2.Error2"));
            return;
        }

        Model.Progress(App.GetLanguage("AddGameWindow.Tab2.Info6"));
        var res = await GameBinding.AddPack(ZipLocal, type, Name, Group);
        Model.ProgressClose();
        if (!res.Item1)
        {
            Model.Show(App.GetLanguage("AddGameWindow.Tab2.Error1"));
            return;
        }

        var model = (App.MainWindow?.DataContext as MainModel);
        model?.Model.Notify(App.GetLanguage("AddGameWindow.Tab2.Info5"));
        App.MainWindow?.LoadMain();
        WindowClose();
    }

    public void AddFile(string file)
    {
        ZipLocal = file;
    }

    private async Task<bool> Tab2GameOverwirte(GameSettingObj obj)
    {
        Model.ProgressClose();
        var test = await Model.ShowWait(
            string.Format(App.GetLanguage("AddGameWindow.Info2"), obj.Name));
        Model.Progress();
        return test;
    }
}