using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;
using System;

namespace MauiApp1;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class Lumememm : ContentPage
{
    private readonly Ellipse head, body, body2;
    private readonly BoxView bucket;
    private readonly Random random;
    private double startX, startY;

    public Lumememm()
    {
        random = new Random();

        bucket = new BoxView { Color = Colors.Brown, WidthRequest = 50, HeightRequest = 30 };
        head = new Ellipse { Stroke = Colors.Black, StrokeThickness = 2, Fill = Colors.White, WidthRequest = 60, HeightRequest = 60 };
        body = new Ellipse { Stroke = Colors.Black, StrokeThickness = 2, Fill = Colors.White, WidthRequest = 80, HeightRequest = 80 };
        body2 = new Ellipse { Stroke = Colors.Black, StrokeThickness = 2, Fill = Colors.White, WidthRequest = 120, HeightRequest = 120 };

        var snowmanLayout = new StackLayout
        {
            Spacing = -10,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Children = { bucket, head, body, body2 }
        };

    var panGesture = new PanGestureRecognizer();
    panGesture.PanUpdated += (s, e) =>
    {
        if (e.StatusType == GestureStatus.Started)
        {
            startX = snowmanLayout.TranslationX;
            startY = snowmanLayout.TranslationY;
        }
        else if (e.StatusType == GestureStatus.Running) 
        {
            snowmanLayout.TranslationX = startX + e.TotalX;
            snowmanLayout.TranslationY = startY + e.TotalY;
        }
    };

snowmanLayout.GestureRecognizers.Add(panGesture);


        Button toggleButton = new Button { Text = "Peida lumememm" };
        toggleButton.Clicked += (s, e) =>
        {
            bool isVisible = !bucket.IsVisible;
            bucket.IsVisible = head.IsVisible = body.IsVisible = body2.IsVisible = isVisible;
            toggleButton.Text = isVisible ? "Peida lumememm" : "Näita lumememme";
        };

        Button randomColorButton = new Button { Text = "Random värvi" };
        randomColorButton.Clicked += async (s, e) =>
        {
            int r = random.Next(0, 255);
            int g = random.Next(0, 255);
            int b = random.Next(0, 255);

            bool vastus = await DisplayAlert("Värvi muutus",
                $"Kas tahad värvi muuta? Uue värvi väärtused:\nRed: {r}, Green: {g}, Blue: {b}",
                "Jah", "Ei");

            if (vastus)
            {
                head.Fill = new SolidColorBrush(Color.FromRgb(r, g, b));
                body.Fill = new SolidColorBrush(Color.FromRgb(r, g, b));
                body2.Fill = new SolidColorBrush(Color.FromRgb(r, g, b));
            }
        };

        Slider opacitySlider = new Slider { Minimum = 0, Maximum = 1, Value = 1 };
        opacitySlider.ValueChanged += (s, e) =>
        {
            head.Opacity = body.Opacity = body2.Opacity = e.NewValue;
        };

        var buttonStack = new StackLayout
        {
            Children = { toggleButton, randomColorButton, opacitySlider },
            Orientation = StackOrientation.Vertical,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.End,
            Padding = new Thickness(10)
        };

        Content = new Grid
        {
            Children =
            {
                snowmanLayout,
                buttonStack
            }
        };
    }
}
