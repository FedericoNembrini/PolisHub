using System;
using System.Threading.Tasks;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

using Newtonsoft.Json.Linq;

using PolisHUB.Global;
using PolisHUB.Visualization;
using PolisHUB.WiFiManagement;

namespace PolisHUB
{
	public sealed partial class MainPage : Page
	{
		private HTTPHandler handler = new HTTPHandler();
		private WiFiHandler WiFiController = new WiFiHandler();

		public MainPage()
		{
			this.InitializeComponent();
			WiFiConnectionController();
		}

		private void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			LoadingProgressRing.IsActive = true;
			LoginRequestAsync();
		}

		private async void LoginRequestAsync()
		{
			//JObject status = await handler.HTTPLoginRequest_Async("admin", "password");
			JObject status = await handler.HTTPLoginRequest_Async(UsernameBox.Text, PasswordBox.Password);

			if (status == null)
			{
				Message.Visibility = Visibility.Visible;
				Message.Text = "Sito Ofline o Connessione Assente";
			}
			else if (status["status"].ToString() == "error")
			{
				Message.Text = status["error"].ToString();
				Message.Visibility = Visibility.Visible;
			}
			else
			{
				Object[] obj = new Object[3];
				obj[0] = handler;
				obj[1] = UsernameBox.Text;
				obj[2] = WiFiButton.Glyph;

				this.Frame.Navigate(typeof(HomePage), obj);
			}
			LoadingProgressRing.IsActive = false;
		}

		private async void WiFiConnectionController()
		{
			await WiFiController.WiFi_AdapterInitialization();
			SolidColorBrush ColorResult = await WiFiController.ScanAdapter();

			while (true)
			{
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

				ColorResult = await WiFiController.WiFiController();
			} 
		}

		private void WiFiConnection_Click(object sender, RoutedEventArgs e)
		{
			if (!(WiFiButton.Glyph.Equals(CommonElements.WiFiWarning4)))
			{
				Object obj = new Object();
				obj = WiFiController;

				this.Frame.Navigate(typeof(WiFiSettings), obj);
			}
		}

		private void ShutDown_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
		{
			Application.Current.Exit();
		}
	}
}
