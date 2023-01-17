using Avalonia.Controls;
using Avalonia.Threading;
using ColorMC.Gui.UIBinding;
using System;
using System.Threading.Tasks;

namespace ColorMC.Gui.UI.Windows;

public partial class InitWindow : Window
{
    public InitWindow()
    {
        InitializeComponent();

        Rectangle1.MakeResizeDrag(this);

        Opened += MainWindow_Opened;
    }

    private void MainWindow_Opened(object? sender, EventArgs e)
    {
        Task.Run(async () =>
        {
            BaseBinding.Init();

            if (GuiConfigUtils.Config != null)
            {
                await App.LoadImage(GuiConfigUtils.Config.BackImage,
                    GuiConfigUtils.Config.BackEffect);
            }

            Dispatcher.UIThread.Post(() =>
            {
                if (BaseBinding.ISNewStart)
                {
                    App.ShowHello();
                }
                else
                {
                    App.ShowMain();
                }
                Close();
            });
        });
    }
}