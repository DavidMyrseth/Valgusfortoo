namespace MauiApp1;

public partial class FigurePage : ContentPage
{
    BoxView bw;
    Random rnd = new Random();
    HorizontalStackLayout hsl;
    List<string> buttons = new List<string> { "Tagasi", "Avaleht", "Edasi" };

    public FigurePage(int k)
    {
        int r = rnd.Next(0, 255);
        int g = rnd.Next(0, 255);
        int b = rnd.Next(0, 255);
        bw = new BoxView
        {
            Color = Color.FromRgb(r, g, b),
            CornerRadius = 20,
            WidthRequest = 200,
            HeightRequest = 200,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            BackgroundColor = Color.FromRgba(0, 0, 0, 0)
        };

        TapGestureRecognizer tap = new TapGestureRecognizer();
        tap.Tapped += Klik_boksi_peal;
        bw.GestureRecognizers.Add(tap);

        hsl = new HorizontalStackLayout { };
        for (int i = 0; i < 3; i++)
        {
            Button nupp = new Button
            {
                Text = buttons[i],
                ZIndex = i,
                WidthRequest = DeviceDisplay.Current.MainDisplayInfo.Width / 8.3
            };
            hsl.Add(nupp);
            nupp.Clicked += Liikumine;
        }

        VerticalStackLayout vsl = new VerticalStackLayout
        {
            Children = { bw, hsl },
            VerticalOptions = LayoutOptions.End
        };
        Content = vsl;
    }
    private async void Liikumine(object? sender, EventArgs e)
    {
        // Реализация метода отсутствует на изображении
    }
    //
    private void Klik_boksi_peal(object? sender, TappedEventArgs e)
    {
        bw.Color = Color.FromRgb(rnd.Next(0, 255), rnd.Next(0, 255), rnd.Next(0, 255));
        // bw.RotateXTo(10, 10);
        bw.WidthRequest = bw.Width + 20;
        bw.HeightRequest = bw.HeightRequest + 20;

        if (bw.WidthRequest > (int)DeviceDisplay.MainDisplayInfo.Width / 3)
        {
            bw.HeightRequest = 200;
            bw.WidthRequest = 200;
        }
    }

}
