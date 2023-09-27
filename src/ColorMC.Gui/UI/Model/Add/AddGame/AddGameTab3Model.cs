﻿using Avalonia.Controls;
using ColorMC.Core.Helpers;
using ColorMC.Core.Objs;
using ColorMC.Gui.UI.Model.Items;
using ColorMC.Gui.UI.Model.Main;
using ColorMC.Gui.UIBinding;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.IO;
using System.Threading.Tasks;

namespace ColorMC.Gui.UI.Model.Add.AddGame;

public partial class AddGameModel : TopModel
{
    private FilesPage _fileModel;

    [ObservableProperty]
    private string _selectPath;

    [ObservableProperty]
    private HierarchicalTreeDataGridSource<FileTreeNodeModel> _files;

    [ObservableProperty]
    private bool _canInput;

    [RelayCommand]
    public async Task RefashFiles()
    {
        if (Directory.Exists(SelectPath))
        {
            var res = await Model.ShowWait(string.Format(App.GetLanguage("AddGameWindow.Tab3.Info3"), SelectPath));
            if (!res)
            {
                return;
            }
            Model.Progress(App.GetLanguage("AddGameWindow.Tab3.Info2"));
            await Task.Run(() =>
            {
                _fileModel = new FilesPage(SelectPath, true, new()
                { "assets", "libraries", "versions", "launcher_profiles.json" });
            });
            Model.ProgressClose();
            Files = _fileModel.Source;

            CanInput = true;
        }
        else
        {
            CanInput = false;
            Files = null;
            _fileModel = null;
            Model.Show(string.Format(App.GetLanguage("AddGameWindow.Tab1.Error2"), SelectPath));
        }
    }

    [RelayCommand]
    public async Task AddFiles()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            Model.Show(string.Format(App.GetLanguage("AddGameWindow.Tab1.Error2"), SelectPath));
            return;
        }

        if (PathHelper.FilePathHasInvalidChars(Name))
        {
            Model.Show(App.GetLanguage("AddGameWindow.Tab1.Error13"));
            return;
        }

        Model.Progress(App.GetLanguage("AddGameWindow.Tab3.Info1"));
        var res = await GameBinding.AddGame(Name, SelectPath, _fileModel.GetUnSelectItems(), Group);
        Model.ProgressClose();

        if (!res)
        {
            Model.Show(App.GetLanguage("AddGameWindow.Tab3.Error1"));
            return;
        }

        var model = (App.MainWindow?.DataContext as MainModel);
        model?.Model.Notify(App.GetLanguage("AddGameWindow.Tab2.Info5"));
        App.MainWindow?.LoadMain();
        WindowClose();
    }

    [RelayCommand]
    public async Task SelectLocal()
    {
        var res = await PathBinding.SelectPath(FileType.Game);
        if (string.IsNullOrWhiteSpace(res))
        {
            return;
        }

        if (Directory.Exists(res))
        {
            SelectPath = res;

            await RefashFiles();
        }
        else
        {
            Model.Show(string.Format(App.GetLanguage("AddGameWindow.Tab3.Error2"), res));
        }
    }
}
