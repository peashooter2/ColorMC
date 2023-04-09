﻿using Avalonia.Controls.Primitives;
using Avalonia.Controls;
using ColorMC.Core.Objs.Minecraft;
using ColorMC.Gui.UI.Controls.GameEdit;
using Avalonia.Interactivity;
using ColorMC.Gui.UIBinding;

namespace ColorMC.Gui.UI.Flyouts;

public class GameEditFlyout5
{
    private readonly ServerInfoObj Obj;
    private readonly Tab10Control Con;
    public GameEditFlyout5(Tab10Control con, ServerInfoObj obj)
    {
        Con = con;
        Obj = obj;

        var fy = new FlyoutsControl(new()
        {
            (App.GetLanguage("Button.OpFile"), true, Button1_Click),
            (App.GetLanguage("GameEditWindow.Tab10.Text2"), true, Button2_Click)
        }, con);
    }

    private void Button2_Click()
    {
        GameBinding.CopyServer(Obj);
    }

    private void Button1_Click()
    {
        Con.Delete(Obj);
    }
}