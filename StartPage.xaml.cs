using Naidis_App;

namespace MauiApp1;

public partial class StartPage : ContentPage
{
	public List<ContentPage> lehed = new List<ContentPage>() { new TextPage(0), new FigurePage(1), new Valgusfloor(), new DateTimePage(),  new Stepper_Slider() , new RGB()};
	public List<string> Tekstid = new List<string>{"Tee lahti TekstPage", "Tee lahti Figure", "Tee lahti Valgusfoor", "Tee lahti DateTime", "Tee lahti Stepper ja Slider", "Tee lahti RGB Slider" };
	ScrollView sv;
	VerticalStackLayout vsl;
	public StartPage()
	{
		Title = "Avaleht";
		vsl = new VerticalStackLayout { BackgroundColor = Color.FromRgb(169, 169, 169) };
		for (int i = 0; i < Tekstid.Count; i++)
		{
			Button nupp = new Button
			{
				Text = Tekstid[i],
				BackgroundColor = Color.FromRgb(245, 255, 250),
				TextColor = Color.FromRgb(0, 0, 0),
				BorderWidth = 10,
				ZIndex = i,
				FontFamily="Dis"

			};
			vsl.Add(nupp);
			nupp.Clicked += Lehte_avamine;
		}
		sv=new ScrollView { Content=vsl};
		Content = sv;
	}
	private void Lehte_avamine (object sender, EventArgs e)
	{
		Button btn = (Button)sender;
		Navigation.PushAsync(lehed[btn.ZIndex]);
	}
}