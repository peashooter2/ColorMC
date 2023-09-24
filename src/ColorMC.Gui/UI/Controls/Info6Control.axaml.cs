using Avalonia.Controls;
using ColorMC.Gui.UI.Model;
using System;
using System.ComponentModel;

namespace ColorMC.Gui.UI.Controls;

public partial class Info6Control : UserControl
{
    private bool _display = false;

    public Info6Control()
    {
        InitializeComponent();

        DataContextChanged += Info6Control_DataContextChanged;
    }

    private void Info6Control_DataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is BaseModel model)
        {
            model.PropertyChanged += Model_PropertyChanged;
        }
    }

    private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "Info6Show")
        {
            if (!_display)
            {
                _display = true;
                App.CrossFade300.Start(null, this);
            }
        }
        else if (e.PropertyName == "Info6Close")
        {
            if (_display)
            {
                _display = false;
                App.CrossFade300.Start(this, null);
            }
        }
    }
}
