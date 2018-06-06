using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using Newtonsoft.Json.Linq;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media;

using PolisHUB.Global;
using PolisHUB.WiFiManagement;

namespace PolisHUB
{
	public sealed partial class HomePage : Page
	{
		private HTTPHandler handler;
		private WiFiHandler WiFiController = new WiFiHandler();
		private List<Thing> things = new List<Thing>();

		public HomePage()
		{
			this.InitializeComponent();
			ThingVisualization_Grid.DataContext = this;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			Object[] obj = e.Parameter as Object[];
			handler = obj[0] as HTTPHandler;
			UserAppBar.Label = obj[1] as String;

			SetWiFiIcon(obj[2].ToString());

			LoadThingList_Async();
		}

		private void SetWiFiIcon(string Glyph)
		{
			if (!Glyph.Equals(CommonElements.WiFiWarning4))
			{
				WiFiButton.Visibility = Visibility.Visible;
				WiFiConnectionController();
			}
		}

		private async void WiFiConnectionController()
		{
			SolidColorBrush ColorResult;

			do
			{
				ColorResult = await WiFiController.ScanAdapter();

				if (ColorResult.Color.Equals(CommonElements.ColorWiFiMissing.Color))
				{
					WiFiButton.Glyph = CommonElements.WiFiWarning4;
					WiFiButton.Foreground = new SolidColorBrush(Colors.Black);
					break;
				}
				else
				{
					WiFiButton.Foreground = ColorResult;
				}
				await Task.Delay(15000);
			} while (true);
		}

		private async void LoadThingList_Async()
		{
			JArray jsonThingList = await handler.HTTPThingRequest_Async();

			foreach (JObject thing in jsonThingList)
			{
				things.Add(new Thing(thing, handler));
			}

			ThingVisualization_Grid.ItemsSource = things;
		}

		private void ThingVisualization_Grid_ThingClick(object sender, ItemClickEventArgs e)
		{
			Thing thing = ((e.ClickedItem) as Thing);
			this.Frame.Navigate(typeof(ThingVisualization), thing);
		}
	}
}
