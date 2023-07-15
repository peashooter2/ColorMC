using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Threading;
using ColorMC.Core;
using ColorMC.Gui.UI.Flyouts;
using ColorMC.Gui.UI.Model.User;
using ColorMC.Gui.UI.Windows;
using ColorMC.Gui.Utils;
using System.ComponentModel;
using System.Threading;

namespace ColorMC.Gui.UI.Controls.User;

public partial class UsersControl : UserControl, IUserControl
{
    private readonly UsersModel model;

    public IBaseWindow Window => App.FindRoot(VisualRoot);

    public UserControl Con => this;

    public string Title => App.GetLanguage("UserWindow.Title");

    public UsersControl()
    {
        InitializeComponent();

        model = new(this);
        model.PropertyChanged += Model_PropertyChanged;
        DataContext = model;

        DataGrid_User.DoubleTapped += DataGrid_User_DoubleTapped;
        DataGrid_User.CellPointerPressed += DataGrid_User_PointerPressed;

        TextBox_Input1.KeyDown += TextBox_Input1_KeyDown;
        TextBox_Input3.KeyDown += TextBox_Input3_KeyDown;

        AddHandler(DragDrop.DragEnterEvent, DragEnter);
        AddHandler(DragDrop.DragLeaveEvent, DragLeave);
        AddHandler(DragDrop.DropEvent, Drop);
    }

    private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "DisplayAdd")
        {
            Dispatcher.UIThread.Post(() =>
            {
                if (model.DisplayAdd)
                {
                    App.CrossFade300.Start(null, Grid1, CancellationToken.None);
                }
                else
                {
                    App.CrossFade300.Start(Grid1, null, CancellationToken.None);
                }
            });
        }
    }

    private async void TextBox_Input3_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            await model.Add();
        }
    }

    private async void TextBox_Input1_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (model.Type == 0)
            {
                await model.Add();
            }
        }
    }

    public void Opened()
    {
        Window.SetTitle(Title);

        Dispatcher.UIThread.Post(DataGrid_User.SetFontColor);
    }

    public void Closed()
    {
        ColorMCCore.LoginOAuthCode = null;

        App.UserWindow = null;
    }

    private void DragEnter(object? sender, DragEventArgs e)
    {
        if (e.Data.Contains(DataFormats.Text))
        {
            Grid2.IsVisible = true;
        }
    }

    private void DragLeave(object? sender, DragEventArgs e)
    {
        Grid2.IsVisible = false;
    }

    private void Drop(object? sender, DragEventArgs e)
    {
        Grid2.IsVisible = false;
        model.Drop(e.Data);
    }

    private void DataGrid_User_PointerPressed(object? sender,
        DataGridCellPointerPressedEventArgs e)
    {
        Dispatcher.UIThread.Post(() =>
        {
            if (model.Item == null)
                return;

            var pro = e.PointerPressedEventArgs.GetCurrentPoint(this);

            if (pro.Properties.IsRightButtonPressed)
            {
                _ = new UserFlyout(this, model);
            }
            else if (e.Column.DisplayIndex == 0 && pro.Properties.IsLeftButtonPressed)
            {
                model.Select(model.Item);
            }
        });
    }

    private void DataGrid_User_DoubleTapped(object? sender, RoutedEventArgs e)
    {
        model.Select();
    }
    public void AddUrl(string url)
    {
        model.AddUrl(url);
    }
}
