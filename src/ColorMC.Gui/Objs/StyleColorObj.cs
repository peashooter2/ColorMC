using Avalonia.Media;

namespace ColorMC.Gui.Objs;

public record StyleColorObj
{
    public static StyleColorObj LightBlue { get; } = new()
    {
        MainColor = Brush.Parse("#637df3"),
        TextLight = Brushes.White,
        TextLight1 = Brush.Parse("#a2affe"),
        WindowBack = Brush.Parse("#f1f3ff"),
        WindowBack1 = Brush.Parse("#172876"),
        PointerOver = Brush.Parse("#f2f1ff"),
        Select = Brushes.White,
    };

    public IBrush MainColor { get; init; }
    public IBrush WindowBack { get; init; }
    public IBrush WindowBack1 { get; init; }
    public IBrush TextLight { get; init; }
    public IBrush TextLight1 { get; init; }
    public IBrush PointerOver { get; init; }
    public IBrush Select { get; init; }

}
