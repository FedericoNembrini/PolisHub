using System;
using System.Collections.Generic;

using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using Windows.Devices.WiFi;

using PolisHUB.WiFiManagement;


namespace PolisHUB.Visualization
{
	public sealed partial class WiFiSettings : Page
	{
		WiFiHandler WiFiController;
		List<WiFiAvailableNetworkAdapter> WiFiAvailableListTemp = new List<WiFiAvailableNetworkAdapter>();

		public WiFiSettings()
		{
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			Object obj = e.Parameter as Object;
			WiFiController = obj as WiFiHandler;

			WiFiSetup();
		}

		private void WiFiSetup()
		{
			if (WiFiController.WiFiConnectedName != null)
			{
				SSIDConnectedTo.Text = WiFiController.WiFiConnectedName;
				WiFiConnectedStackPanel.Visibility = Visibility.Visible;
			}
			else
				WiFiScanning();
		}

		private async void WiFiScanning()
		{
			WiFiListStackPanel.Visibility = Visibility.Visible;

			WiFiListProgressRing.IsEnabled = true;
			WiFiListProgressRing.Visibility = Visibility.Visible;
			WiFiListProgressRing.IsActive = true;

			WiFiAvailableListTemp = await WiFiController.ScanNetworks();

			WiFiListProgressRing.Visibility = Visibility.Collapsed;
			WiFiListProgressRing.IsActive = false;
			WiFiListProgressRing.IsActive = false;

			WiFiListListView.ItemsSource = WiFiAvailableListTemp;

			UpdateListIcon.Visibility = Visibility.Visible;
		}

		private void UpdateWiFiList_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
		{
			UpdateListIcon.Visibility = Visibility.Collapsed;
			WiFiScanning();
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			On_BackRequested();
		}

		private bool On_BackRequested()
		{
			if (this.Frame.CanGoBack)
			{
				this.Frame.GoBack();
				return true;
			}
			return false;
		}

		private void SSIDList_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
		{
			WiFiListStackPanel.Visibility = Visibility.Collapsed;

			SSID_TextBox.Text = (e.OriginalSource as TextBlock).Text;
			WiFiConnectStackPanel.Visibility = Visibility.Visible;
		}

		private async void ConnectWiFi_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
		{
			WiFiAvailableNetworkAdapter WiFiNetworkToConnect = WiFiAvailableListTemp.Find(x => x.Ssid.Equals(SSID_TextBox.Text));

			WiFiConnectionStatus ConnectionStatusResult =await WiFiController.ConnectToWiFi(WiFiNetworkToConnect.AvailableNetworkCopy, PasswordBox.Password);

			if (ConnectionStatusResult == WiFiConnectionStatus.Success)
			{
				ConnectionResultTextBlock.Text = "Connessione Riuscita!";
				ConnectionResultTextBlock.Foreground = new SolidColorBrush(Colors.LightGreen);
			}
			else if(ConnectionStatusResult == WiFiConnectionStatus.InvalidCredential)
			{
				ConnectionResultTextBlock.Text = "Password Errata!";
				ConnectionResultTextBlock.Foreground = new SolidColorBrush(Colors.DarkRed);
			}
			else if(ConnectionStatusResult == WiFiConnectionStatus.Timeout)
			{
				ConnectionResultTextBlock.Text = "Timeout di Connessione";
				ConnectionResultTextBlock.Foreground = new SolidColorBrush(Colors.Yellow);
			}
			else
			{
				ConnectionResultTextBlock.Text = "Errore";
				ConnectionResultTextBlock.Foreground = new SolidColorBrush(Colors.Black);
			}
		}
	}
}
