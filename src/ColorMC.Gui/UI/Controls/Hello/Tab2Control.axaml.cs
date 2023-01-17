using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using ColorMC.Gui.UI.Windows;
using ColorMC.Gui.UIBinding;
using ColorMC.Gui.Utils.LaunchSetting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ColorMC.Gui.UI.Controls.Hello;

public partial class Tab2Control : UserControl
{
    private IBaseWindow Window;
    public Tab2Control()
    {
        InitializeComponent();

        Button_SelectFile.Click += Button_SelectFile_Click;
        Button_Input.Click += Button_Input_Click;

        Button_SelectFile1.Click += Button_SelectFile1_Click;
        Button_Input1.Click += Button_Input1_Click;

        Button_SelectFile2.Click += Button_SelectFile2_Click;
        Button_Input2.Click += Button_Input2_Click;

        Button_Next.Click += Button_Next_Click;
    }

    private void Button_Input2_Click(object? sender, RoutedEventArgs e)
    {
        var local = TextBox_Local2.Text;
        if (string.IsNullOrWhiteSpace(local))
        {
            Window.Info.Show(Localizer.Instance["HelloWindow.Tab2.Error1"]);
            return;
        }
        Window.Info1.Show(Localizer.Instance["HelloWindow.Tab2.Info1"]);

        try
        {
            var res = ConfigBinding.LoadGuiConfig(local);
            if (!res)
            {
                Window.Info.Show(Localizer.Instance["HelloWindow.Tab2.Error2"]);
                return;
            }
            Window.Update();
            Window.Info2.Show(Localizer.Instance["HelloWindow.Tab2.Info2"]);
        }
        catch (Exception e1)
        {
            Window.Info.Show(Localizer.Instance["HelloWindow.Tab2.Error3"]);
            App.ShowError(Localizer.Instance["HelloWindow.Tab2.Error3"], e1);
        }
        finally
        {
            Window?.Info1.Close();
        }
    }

    private async void Button_SelectFile2_Click(object? sender, RoutedEventArgs e)
    {
        var file = await  Window.Window.StorageProvider.OpenFilePickerAsync(new()
        {
            Title = Localizer.Instance["HelloWindow.Tab2.Info3"],
            AllowMultiple = false,
            FileTypeFilter = new List<FilePickerFileType>()
            {
                new(Localizer.Instance["HelloWindow.Tab2.Text7"])
                { 
                     Patterns = new List<string>()
                     { 
                        "*.json"
                     }
                }
            }
        });

        if (file?.Any() == true)
        {
            var item = file[0];
            item.TryGetUri(out var uri);
            TextBox_Local2.Text = uri!.LocalPath;
        }
    }

    public void SetWindow(HelloWindow window)
    {
        Window = window;
    }

    public void SetWindow(SettingWindow window)
    {
        Window = window;
        Button_Next.IsVisible = false;
        Button_Next.IsEnabled = false;
    }

    private void Button_Next_Click(object? sender, RoutedEventArgs e)
    {
        Window.Next();
    }

    private void Button_Input_Click(object? sender, RoutedEventArgs e)
    {
        var local = TextBox_Local.Text;
        if (string.IsNullOrWhiteSpace(local))
        {
            Window.Info.Show(Localizer.Instance["HelloWindow.Tab2.Error1"]);
            return;
        }
        Window.Info1.Show(Localizer.Instance["HelloWindow.Tab2.Info1"]);

        try
        {
            var res = ConfigBinding.LoadConfig(local);
            if (!res)
            {
                Window.Info.Show(Localizer.Instance["HelloWindow.Tab2.Error2"]);
                return;
            }
            Window.Update();
            Window.Info2.Show(Localizer.Instance["HelloWindow.Tab2.Info2"]);
        }
        catch (Exception e1)
        {
            Window.Info.Show(Localizer.Instance["HelloWindow.Tab2.Error3"]);
            App.ShowError(Localizer.Instance["HelloWindow.Tab2.Error3"], e1);
        }
        finally
        {
            Window.Info1.Close();
        }
    }

    private async void Button_SelectFile_Click(object? sender, RoutedEventArgs e)
    {
        var file = await Window.Window.StorageProvider.OpenFilePickerAsync(new()
        {
            Title = Localizer.Instance["HelloWindow.Tab2.Info3"],
            AllowMultiple = false,
            FileTypeFilter = new List<FilePickerFileType>()
            {
                new(Localizer.Instance["HelloWindow.Tab2.Text7"])
                {
                     Patterns = new List<string>()
                     {
                        "*.json"
                     }
                }
            }
        });

        if (file?.Any() == true)
        {
            var item = file[0];
            item.TryGetUri(out var uri);
            TextBox_Local.Text = uri!.LocalPath;
        }
    }

    private void Button_Input1_Click(object? sender, RoutedEventArgs e)
    {
        var local = TextBox_Local1.Text;
        if (string.IsNullOrWhiteSpace(local))
        {
            Window.Info.Show(Localizer.Instance["HelloWindow.Tab2.Error1"]);
            return;
        }
        Window.Info1.Show(Localizer.Instance["HelloWindow.Tab2.Info4"]);

        try
        {
            var res = ConfigBinding.LoadAuthDatabase(local);
            if (!res)
            {
                Window.Info.Show(Localizer.Instance["HelloWindow.Tab2.Error4"]);
                return;
            }
            Window.Update();
            Window.Info2.Show(Localizer.Instance["HelloWindow.Tab2.Info5"]);
        }
        catch (Exception)
        {
            Window.Info.Show(Localizer.Instance["HelloWindow.Tab2.Error5"]);
        }
        finally
        {
            Window.Info1.Close();
        }
    }

    private async void Button_SelectFile1_Click(object? sender, RoutedEventArgs e)
    {
        var file = await Window.Window.StorageProvider.OpenFilePickerAsync(new()
        {
            Title = Localizer.Instance["HelloWindow.Tab2.Info6"],
            AllowMultiple = false,
            FileTypeFilter = new List<FilePickerFileType>()
            {
                new(Localizer.Instance["HelloWindow.Tab2.Text8"])
                {
                     Patterns = new List<string>()
                     {
                        "*.db"
                     }
                }
            }
        });

        if (file?.Any() == true)
        {
            var item = file[0];
            item.TryGetUri(out var uri);
            TextBox_Local1.Text = uri!.LocalPath;
        }
    }
}