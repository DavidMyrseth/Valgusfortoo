using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Layouts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MauiApp1;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class RGB : ContentPage
{
    BoxView box;
    Label label;
    Random rnd = new Random();
    List<Slider> sliderList = new List<Slider>();
    List<Label> labelList = new List<Label>();
    List<string> labelListText = new List<string>() { "Red", "Green", "Blue" };
    List<int> ints = new List<int>() { 0, 0, 0 };

    public RGB()
    {
        AbsoluteLayout abs = new AbsoluteLayout();
        StackLayout stackLayout = new StackLayout();

        box = new BoxView()
        {
            WidthRequest = 380,
            HeightRequest = 380,
            Margin = 20,
            Color = Colors.Black,
        };

        AbsoluteLayout.SetLayoutBounds(box, new Rect(0.1f, 0f, 400f, 400f));  // Исправлено
        AbsoluteLayout.SetLayoutFlags(box, AbsoluteLayoutFlags.PositionProportional);
        abs.Children.Add(box);

        for (int i = 0; i < 3; i++)
        {
            Slider slider = new Slider
            {
                AutomationId = i.ToString(), // Вместо TabIndex
                Minimum = 0,
                Maximum = 255,
                Value = 0,
                Margin = 5,
                MinimumTrackColor = Colors.Black,
                MaximumTrackColor = Colors.Black,
            };
            slider.ValueChanged += Slider_ValueChanged;
            sliderList.Add(slider);

            label = new Label
            {
                Text = labelListText[i],
                FontSize = 24,
                TextColor = Colors.Black,
                HorizontalOptions = LayoutOptions.Center,
            };
            labelList.Add(label);

            stackLayout.Children.Add(label);
            stackLayout.Children.Add(slider);
        }

        AbsoluteLayout.SetLayoutBounds(stackLayout, new Rect(0.1f, 1.17f, 400f, 400f));  // Исправлено
        AbsoluteLayout.SetLayoutFlags(stackLayout, AbsoluteLayoutFlags.PositionProportional);
        abs.Children.Add(stackLayout);

        Button button = new Button { Text = "Random Color" };
        button.Clicked += Button_Clicked;

        AbsoluteLayout.SetLayoutBounds(button, new Rect(0.1f, 0.9f, 400f, 50f));  // Исправлено
        AbsoluteLayout.SetLayoutFlags(button, AbsoluteLayoutFlags.PositionProportional);
        abs.Children.Add(button);

        Content = abs;
    }

    void Button_Clicked(object sender, EventArgs e)
    {
        for (int i = 0; i < ints.Count; i++)
        {
            ints[i] = rnd.Next(256);
            sliderList[i].Value = ints[i];
        }
        box.Color = new Color(ints[0] / 255f, ints[1] / 255f, ints[2] / 255f);  // Исправлено
    }

    void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
    {
        Slider slider = (Slider)sender;
        int index = int.Parse(slider.AutomationId); // Получаем индекс

        ints[index] = Convert.ToInt32(e.NewValue);
        labelList[index].Text = $"{labelListText[index]} = {ints[index]:X2}";

        box.Color = new Color(ints[0] / 255f, ints[1] / 255f, ints[2] / 255f);  // Исправлено
    }
}
