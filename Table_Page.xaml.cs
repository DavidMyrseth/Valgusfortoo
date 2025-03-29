using System;
using Microsoft.Maui.Controls;

using System.Collections.Generic;

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
        sc = new SwitchCell { Text = "Näita veel" };
        sc.OnChanged += Sc_OnChanged;

        ic = new ImageCell
        {
            ImageSource = ImageSource.FromFile("bob.jpg"),
            Text = "Minu Sõber",
            Detail = "Väga ilus poiss"
        };
        ic.Tapped += ChangePhoto;

        telNr = new EntryCell
        {
            Label = "Telefon",
            Placeholder = "Sisesta tel. number",
            Keyboard = Keyboard.Telephone
        };

        email = new EntryCell
        {
            Label = "Email",
            Placeholder = "Sisesta email",
            Keyboard = Keyboard.Email
        };

        text = new EntryCell
        {
            Label = "Palun kirjuta tekst",
            Placeholder = "Sisesta tekst",
            Keyboard = Keyboard.Default
        };

        photoSection = new TableSection();

        tableView = new TableView
        {
            Intent = TableIntent.Form,
            Root = new TableRoot("Andmete sisestamine")
            {
                new TableSection("Põhiandmed:") { text },
                new TableSection("Kontaktandmed:") { telNr, email, sc },
                photoSection
            }
        };

        Button smsBtn = new Button { Text = "Saada SMS" };
        smsBtn.Clicked += SmsBtn_Clicked;

        Button callBtn = new Button { Text = "Helista" };
        callBtn.Clicked += CallBtn_Clicked;

        Button mailBtn = new Button { Text = "Kirjuta kiri" };
        mailBtn.Clicked += MailBtn_Clicked;

        Button greetingBtn = new Button { Text = "Õnnitlused" };
        greetingBtn.Clicked += GreetingBtn_Clicked;

        Button cameraBtn = new Button { Text = "Photo" };
        cameraBtn.Clicked += Button_ClickedAsync;

        Grid actionStackLayout = new Grid
        {
            HorizontalOptions = LayoutOptions.FillAndExpand,
            ColumnDefinitions =
            {
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
                new ColumnDefinition { Width = GridLength.Star },
                 new ColumnDefinition { Width = GridLength.Star }
            }
        };
        actionStackLayout.Children.Add(callBtn);
        Grid.SetColumn(callBtn, 0);

        actionStackLayout.Children.Add(smsBtn);
        Grid.SetColumn(smsBtn, 1);

        actionStackLayout.Children.Add(mailBtn);
        Grid.SetColumn(mailBtn, 2);

        actionStackLayout.Children.Add(greetingBtn);
        Grid.SetColumn(greetingBtn, 3);

        actionStackLayout.Children.Add(cameraBtn);
        Grid.SetColumn(cameraBtn, 4);



        Content = new StackLayout { Children = { tableView, actionStackLayout } };
    }

    private void Sc_OnChanged(object sender, ToggledEventArgs e)
    {
        if (e.Value)
        {
            if (!photoSection.Contains(ic)) // Проверяем, добавлен ли уже
            {
                photoSection.Title = "Foto";
                photoSection.Add(ic);
            }
            sc.Text = "Peida";
        }
        else
        {
            if (photoSection.Contains(ic)) // Проверяем перед удалением
            {
                photoSection.Title = "";
                photoSection.Remove(ic);
            }
            sc.Text = "Näita veel";
        }
    }


    private async void ChangePhoto(object sender, EventArgs e)
    {
        var result = await MediaPicker.CapturePhotoAsync();
        if (result != null)
        {
            var stream = await result.OpenReadAsync();
            ic.ImageSource = ImageSource.FromStream(() => stream);
        }
    }

    private async void SmsBtn_Clicked(object sender, EventArgs e)
    {
        if (Sms.Default.IsComposeSupported)
        {
            try
            {
                var message = new SmsMessage(text.Text, new[] { telNr.Text });
                await Sms.Default.ComposeAsync(message);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Viga", $"SMS-i saatmine ebaõnnestus: {ex.Message}", "OK");
            }
        }
        else
        {
            await DisplayAlert("Viga", "Sinu seadmes SMS saatmine ei ole toetatud", "OK");
        }
    }


    private void CallBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            PhoneDialer.Open(telNr.Text);
        }
        catch (Exception ex)
        {
            DisplayAlert("Viga", "Helistamine ebaõnnestus", "OK");
        }
    }

    private async void MailBtn_Clicked(object sender, EventArgs e)
    {
        if (Email.Default.IsComposeSupported)
        {
            try
            {
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
                await DisplayAlert("Viga", $"E-kirja saatmine ebaõnnestus: {ex.Message}", "OK");
            }
        }
        else
        {
            await DisplayAlert("Viga", "Sinu seadmes e-kirja saatmine ei ole toetatud", "OK");
        }
    }


    private void GreetingBtn_Clicked(object sender, EventArgs e)
    {
        var random = new Random();
        var message = greetings[random.Next(greetings.Count)];

        DisplayActionSheet("Vali saatmisviis", "Loobu", null, "SMS", "Email").ContinueWith(t =>
        {
            if (t.Result == "SMS")
            {
                Sms.ComposeAsync(new SmsMessage(message, telNr.Text));
            }
            else if (t.Result == "Email")
            {
                Email.ComposeAsync("Õnnitlus!", message, email.Text);
            }
        });
    }

    private async void Button_ClickedAsync(object sender, EventArgs e)
    {
        if (MediaPicker.Default.IsCaptureSupported)
        {
            FileResult myPhoto = await MediaPicker.Default.CapturePhotoAsync();
            if (myPhoto != null)
            {
                string localFilePath = Path.Combine(FileSystem.AppDataDirectory, myPhoto.FileName);
                using (Stream sourceStream = await myPhoto.OpenReadAsync())
                using (FileStream localFileStream = File.Create(localFilePath))
                {
                    await sourceStream.CopyToAsync(localFileStream);
                }

                // Убедимся, что файл еще доступен
                if (File.Exists(localFilePath))
                {
                    ic.ImageSource = ImageSource.FromFile(localFilePath);
                }
                else
                {
                    await DisplayAlert("Ошибка", "Файл был удален или недоступен", "OK");
                }
            }
        }
        else
        {
            await DisplayAlert("OOPS", "Midagi läks valesti", "OK");
        }
    }

}
