using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel.Communication;
using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;

namespace MauiApp1;

public partial class Table_Page : ContentPage
{
    TableView tableView;
    SwitchCell sc;
    ImageCell ic;
    TableSection photoSection;
    EntryCell telNr, email, text;
    List<string> greetings = new List<string>
    {
        "Head uut aastat!",
        "Palju õnne!",
        "Häid pühi!",
        "Tervitused sõbrale!",
        "Kaunist päeva!"
    };

    public Table_Page()
    {
        // Установка стиля страницы
        BackgroundColor = Color.FromArgb("#F5F7FA");

        InitializeComponents();
        SetupUI();
    }

    private void InitializeComponents()
    {
        // Стилизованный SwitchCell
        sc = new SwitchCell
        {
            Text = "Näita veel",
            OnColor = Color.FromArgb("#4CAF50"),
        };
        sc.OnChanged += Sc_OnChanged;

        // Стилизованный ImageCell
        ic = new ImageCell
        {
            ImageSource = ImageSource.FromFile("bob.jpg"),
            Text = "Minu Sõber",
            Detail = "Väga ilus poiss",
            TextColor = Color.FromArgb("#263238"),
            DetailColor = Color.FromArgb("#546E7A")
        };
        ic.Tapped += ChangePhoto;

        // Стилизованные EntryCell
        telNr = new EntryCell
        {
            Label = "Telefon",
            Placeholder = "Sisesta tel. number",
            Keyboard = Keyboard.Telephone,
            LabelColor = Color.FromArgb("#455A64"),
        };

        email = new EntryCell
        {
            Label = "Email",
            Placeholder = "Sisesta email",
            Keyboard = Keyboard.Email,
            LabelColor = Color.FromArgb("#455A64"),
        };

        text = new EntryCell
        {
            Label = "Palun kirjuta tekst",
            Placeholder = "Sisesta tekst",
            Keyboard = Keyboard.Default,
            LabelColor = Color.FromArgb("#455A64"),
        };

        photoSection = new TableSection();
    }

    private void SetupUI()
    {
        // Стилизованный TableView
        tableView = new TableView
        {
            Intent = TableIntent.Form,
            BackgroundColor = Color.FromArgb("#FFFFFF"),
            Root = new TableRoot("Andmete sisestamine")
            {
                new TableSection("Põhiandmed:") {
                    text
                },
                new TableSection("Kontaktandmed:") {
                    telNr, email, sc
                },
                photoSection
            }
        };

        var buttons = CreateActionButtons();

        // Основной контейнер с тенью и скругленными углами
        var mainContainer = new Frame
        {
            Content = tableView,
            BackgroundColor = Colors.White,
            BorderColor = Color.FromArgb("#E0E0E0"),
            CornerRadius = 12,
            HasShadow = true,
            Padding = 0,
            Margin = new Thickness(20, 10)
        };

        Content = new StackLayout
        {
            Children = { mainContainer, buttons },
            Spacing = 10,
            Padding = new Thickness(0, 10),
            BackgroundColor = Color.FromArgb("#F5F7FA")
        };
    }

    private Grid CreateActionButtons()
    {
        // Функция для создания стилизованных кнопок
        Button CreateStyledButton(string text, string backgroundColor, string textColor)
        {
            return new Button
            {
                Text = text,
                BackgroundColor = Color.FromArgb(backgroundColor),
                TextColor = Color.FromArgb(textColor),
                CornerRadius = 8,
                FontSize = 14,
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(5),
                Padding = new Thickness(10, 8)
            };
        }

        Button smsBtn = CreateStyledButton("Saada SMS", "#2196F3", "#FFFFFF");
        smsBtn.Clicked += SmsBtn_Clicked;

        Button callBtn = CreateStyledButton("Helista", "#4CAF50", "#FFFFFF");
        callBtn.Clicked += CallBtn_Clicked;

        Button mailBtn = CreateStyledButton("Kirjuta kiri", "#FF9800", "#FFFFFF");
        mailBtn.Clicked += MailBtn_Clicked;

        Button greetingBtn = CreateStyledButton("Õnnitlused", "#9C27B0", "#FFFFFF");
        greetingBtn.Clicked += GreetingBtn_Clicked;

        Button cameraBtn = CreateStyledButton("Photo", "#607D8B", "#FFFFFF");
        cameraBtn.Clicked += Button_ClickedAsync;

        var actionGrid = new Grid
        {
            HorizontalOptions = LayoutOptions.FillAndExpand,
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star }
            },
            ColumnSpacing = 5,
            Padding = new Thickness(15, 0),
            Margin = new Thickness(0, 5)
        };

        actionGrid.Children.Add(callBtn);
        Grid.SetColumn(callBtn, 0);

        actionGrid.Children.Add(smsBtn);
        Grid.SetColumn(smsBtn, 1);

        actionGrid.Children.Add(mailBtn);
        Grid.SetColumn(mailBtn, 2);

        actionGrid.Children.Add(greetingBtn);
        Grid.SetColumn(greetingBtn, 3);

        actionGrid.Children.Add(cameraBtn);
        Grid.SetColumn(cameraBtn, 4);

        return actionGrid;
    }

    // Остальные методы остаются без изменений
    private async void Sc_OnChanged(object sender, ToggledEventArgs e)
    {
        try
        {
            if (e.Value)
            {
                if (!photoSection.Contains(ic))
                {
                    photoSection.Title = "Foto";
                    photoSection.Add(ic);
                }
                sc.Text = "Peida";
            }
            else
            {
                if (photoSection.Contains(ic))
                {
                    photoSection.Title = "";
                    photoSection.Remove(ic);
                }
                sc.Text = "Näita veel";
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in Sc_OnChanged: {ex.Message}");
            await DisplayAlert("Viga", "Tekkis vaja lüliti muutmisel", "OK");
        }
    }

    private async void ChangePhoto(object sender, EventArgs e)
    {
        try
        {
            if (!MediaPicker.IsCaptureSupported)
            {
                await DisplayAlert("Viga", "Pildistamine pole toetatud sellel seadmel", "OK");
                return;
            }

            var status = await Permissions.RequestAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Viga", "Puudub luba kaamera kasutamiseks", "OK");
                return;
            }

            var result = await MediaPicker.CapturePhotoAsync();
            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                ic.ImageSource = ImageSource.FromStream(() => stream);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in ChangePhoto: {ex.Message}");
            await DisplayAlert("Viga", "Pildi muutmine ebaõnnestus", "OK");
        }
    }

    private async void SmsBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(telNr.Text) || string.IsNullOrWhiteSpace(text.Text))
            {
                await DisplayAlert("Viga", "Palun sisesta telefoninumber ja tekst", "OK");
                return;
            }

            if (!Sms.Default.IsComposeSupported)
            {
                await DisplayAlert("Viga", "SMS saatmine pole toetatud sellel seadmel", "OK");
                return;
            }

            var message = new SmsMessage(text.Text, new[] { telNr.Text });
            await Sms.Default.ComposeAsync(message);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in SmsBtn_Clicked: {ex.Message}");
            await DisplayAlert("Viga", $"SMS-i saatmine ebaõnnestus: {ex.Message}", "OK");
        }
    }

    private async void CallBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(telNr.Text))
            {
                await DisplayAlert("Viga", "Palun sisesta telefoninumber", "OK");
                return;
            }

            if (!PhoneDialer.Default.IsSupported)
            {
                await DisplayAlert("Viga", "Helistamine pole toetatud sellel seadmel", "OK");
                return;
            }

            PhoneDialer.Default.Open(telNr.Text);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in CallBtn_Clicked: {ex.Message}");
            await DisplayAlert("Viga", "Helistamine ebaõnnestus", "OK");
        }
    }

    private async void MailBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(email.Text) || string.IsNullOrWhiteSpace(text.Text))
            {
                await DisplayAlert("Viga", "Palun sisesta email ja tekst", "OK");
                return;
            }

            if (!Email.Default.IsComposeSupported)
            {
                await DisplayAlert("Viga", "E-kirja saatmine pole toetatud sellel seadmel", "OK");
                return;
            }

            var emailMessage = new EmailMessage
            {
                Subject = "Tervitus!",
                Body = text.Text,
                To = new List<string> { email.Text }
            };
            await Email.Default.ComposeAsync(emailMessage);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in MailBtn_Clicked: {ex.Message}");
            await DisplayAlert("Viga", $"E-kirja saatmine ebaõnnestus: {ex.Message}", "OK");
        }
    }

    private async void GreetingBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            var random = new Random();
            var message = greetings[random.Next(greetings.Count)];

            var result = await DisplayActionSheet("Vali saatmisviis", "Loobu", null, "SMS", "Email");

            if (result == "SMS")
            {
                if (string.IsNullOrWhiteSpace(telNr.Text))
                {
                    await DisplayAlert("Viga", "Palun sisesta telefoninumber", "OK");
                    return;
                }

                if (Sms.Default.IsComposeSupported)
                {
                    await Sms.Default.ComposeAsync(new SmsMessage(message, telNr.Text));
                }
            }
            else if (result == "Email")
            {
                if (string.IsNullOrWhiteSpace(email.Text))
                {
                    await DisplayAlert("Viga", "Palun sisesta email", "OK");
                    return;
                }

                if (Email.Default.IsComposeSupported)
                {
                    await Email.Default.ComposeAsync("Õnnitlus!", message, email.Text);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in GreetingBtn_Clicked: {ex.Message}");
            await DisplayAlert("Viga", "Õnnitluse saatmine ebaõnnestus", "OK");
        }
    }

    private async void Button_ClickedAsync(object sender, EventArgs e)
    {
        try
        {
            if (!MediaPicker.Default.IsCaptureSupported)
            {
                await DisplayAlert("Viga", "Pildistamine pole toetatud sellel seadmel", "OK");
                return;
            }

            var status = await Permissions.RequestAsync<Permissions.Camera>();
            if (status != PermissionStatus.Granted)
            {
                await DisplayAlert("Viga", "Puudub luba kaamera kasutamiseks", "OK");
                return;
            }

            var myPhoto = await MediaPicker.Default.CapturePhotoAsync();
            if (myPhoto != null)
            {
                string localFilePath = Path.Combine(FileSystem.AppDataDirectory, myPhoto.FileName);

                using (Stream sourceStream = await myPhoto.OpenReadAsync())
                using (FileStream localFileStream = File.Create(localFilePath))
                {
                    await sourceStream.CopyToAsync(localFileStream);
                }

                if (File.Exists(localFilePath))
                {
                    ic.ImageSource = ImageSource.FromFile(localFilePath);
                }
                else
                {
                    await DisplayAlert("Viga", "Pildi salvestamine ebaõnnestus", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error in Button_ClickedAsync: {ex.Message}");
            await DisplayAlert("Viga", "Pildistamine ebaõnnestus", "OK");
        }
    }
}
