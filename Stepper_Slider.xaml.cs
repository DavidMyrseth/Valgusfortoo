using Microsoft.Maui.Layouts;

namespace MauiApp1;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class Stepper_Slider : ContentPage
{
    Stepper stp;
    Slider sld;
    Label lbl;

    public Stepper_Slider()
    {
        stp = new Stepper
        {
            Minimum = 0,
            Maximum = 100,
            Value = 20,
            Increment = 5,
        };
        stp.ValueChanged += stp_ValueChanged;

        sld = new Slider
        {
            Minimum = stp.Minimum,
            Maximum = stp.Maximum,
            Value = stp.Value,
            MinimumTrackColor = Colors.White,
            MaximumTrackColor = Colors.Black,
            ThumbColor = Colors.Red,
        };
        sld.ValueChanged += sld_ValueChanged;

        lbl = new Label
        {
            Text = "BIG",
            FontSize = stp.Value,
        };

        List<View> objects = new List<View> { stp, lbl, sld };
        AbsoluteLayout abs = new AbsoluteLayout();
        float y = 0;
        foreach (var item in objects)
        {
            y += 0.2f;
            AbsoluteLayout.SetLayoutBounds(item, new Rect(0.1f, y, 200f, 100f));
            AbsoluteLayout.SetLayoutFlags(item, AbsoluteLayoutFlags.PositionProportional);
            abs.Children.Add(item);
        }

        Content = abs;
    }

    void stp_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        lbl.FontSize = e.NewValue;
        lbl.Rotation = e.NewValue;
    }

    void sld_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        lbl.FontSize = e.NewValue;
        lbl.Rotation = e.NewValue;
    }
}
