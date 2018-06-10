using System;
using System.Collections.Generic;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using PolisHUB.WiFiManagement;

namespace PolisHUB.Visualization
{
	public sealed partial class WiFiSettings : Page
	{
		WiFiHandler WiFiController;

		public WiFiSettings()
		{
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			Object obj = e.Parameter as Object;
			WiFiController = obj as WiFiHandler;

			WiFiScanning();
		}

		private async void WiFiScanning()
		{
			if (WiFiController.WiFiConnectedName != null)
			{
				//Connessione già stabilita
			}
			else
			{
				WiFiListStackPanel.Visibility = Visibility.Visible;

				WiFiListProgressRing.IsEnabled = true;
				WiFiListProgressRing.Visibility = Visibility.Visible;
				WiFiListProgressRing.IsActive = true;

				List<WiFiAvailableNetworkAdapter> WiFiAvailableListTemp = new List<WiFiAvailableNetworkAdapter>();
				WiFiAvailableListTemp = await WiFiController.ScanNetworks();

				WiFiListProgressRing.Visibility = Visibility.Collapsed;
				WiFiListProgressRing.IsActive = false;
				WiFiListProgressRing.IsActive = false;

				WiFiListListView.ItemsSource = WiFiAvailableListTemp;

				UpdateListIcon.Visibility = Visibility.Visible;
			}
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
	}
}
