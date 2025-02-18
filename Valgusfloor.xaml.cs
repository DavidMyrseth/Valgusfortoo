using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Controls; 

namespace MauiApp1;

    public partial class Valgusfloor : ContentPage
    {
        BoxView red, yellow, green;
        bool onoff;
        Label sonad;

        public Valgusfloor()
        {

            Button Tagasi_btn = new Button
            {
                Text = "Tagasi",
                BackgroundColor = Colors.AliceBlue,
                TextColor = Colors.Black,
            };
            Tagasi_btn.Clicked += Tagasi_btn_Clicked;

            TapGestureRecognizer tap = new TapGestureRecognizer();
            tap.Tapped += Tap_Tapped;

            red = new BoxView
            {
                Color = Colors.Red,
                CornerRadius = 100,
                WidthRequest = 100,
                HeightRequest = 100,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };
            red.GestureRecognizers.Add(tap);

            yellow = new BoxView
            {
                Color = Colors.Yellow,
                CornerRadius = 100,
                WidthRequest = 100,
                HeightRequest = 100,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };
            yellow.GestureRecognizers.Add(tap);

            green = new BoxView
            {
                Color = Colors.Green,
                CornerRadius = 100,
                WidthRequest = 100,
                HeightRequest = 100,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };
            green.GestureRecognizers.Add(tap);

            Button start = new Button
            {
                Text = "Start",
                BackgroundColor = Colors.AliceBlue,
                TextColor = Colors.Black,
            };
            start.Clicked += Start_Clicked;

            Button start_night = new Button
            {
                Text = "Night",
                BackgroundColor = Colors.AliceBlue,
                TextColor = Colors.Black,
            };
        start_night.Clicked += Start_Clicked_night;

            Button finish = new Button
            {
                Text = "Finish",
                BackgroundColor = Colors.AliceBlue,
                TextColor = Colors.Black,
            };
            finish.Clicked += Finish_Clicked;

            sonad = new Label
            {
                Text = "",
                FontSize = 20,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };

            StackLayout st2 = new StackLayout
            {
                Children = { red, yellow, green },
                BackgroundColor = Colors.DarkGray,
                WidthRequest = 125,
                HeightRequest = 310,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Padding = new Thickness(10),
            };

            StackLayout st3 = new StackLayout
            {
                Children = { start, finish, start_night, Tagasi_btn },
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Center,
            };

            StackLayout st = new StackLayout
            {
                Children = { st2, sonad, st3 },
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 15
            };

            Content = st;
        }


        private void Tap_Tapped(object sender, EventArgs e)
        {
            BoxView circle = sender as BoxView;
            if (circle.Color == Colors.Red)
            {
                sonad.Text = "Stop";
            }
            else if (circle.Color == Colors.Yellow)
            {
                sonad.Text = "Oota";
            }
            else if (circle.Color == Colors.Green)
            {
                sonad.Text = "Go";
            }
        }

        async void Tagasi_btn_Clicked(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.Count > 1)
            {
                await Navigation.PopAsync();
            }
        }

        async void Start_Clicked(object sender, EventArgs e)
        {
            onoff = true;

            while (onoff)
            {
                red.Color = Colors.Black;
                yellow.Color = Colors.Black;
                green.Color = Colors.Green;

                await Task.Delay(1000);

                green.Color = Colors.Black;
                yellow.Color = Colors.Yellow;

                await Task.Delay(500);

                yellow.Color = Colors.Black;
                red.Color = Colors.Red;

                await Task.Delay(1000);

                yellow.Color = Colors.Yellow;

                await Task.Delay(500);
            }
        }
    async void Start_Clicked_night(object sender, EventArgs e)
    {
        onoff = true;

        while (onoff)
        {
            red.Color = Colors.Black;
            green.Color = Colors.Black;
            yellow.Color = Color.FromRgb(255, 255, 0); 

            await Task.Delay(500);

            yellow.Color = Colors.Black;

            await Task.Delay(500);
        }
    }

    async void Finish_Clicked(object sender, EventArgs e)
        {
            onoff = false;
            await Task.Delay(500);
            green.Color = Colors.Black;
            yellow.Color = Colors.Black;
            red.Color = Colors.Black;
        }
    }
    
