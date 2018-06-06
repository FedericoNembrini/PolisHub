using System;
using System.Threading.Tasks;

using Windows.UI;
using Windows.UI.Xaml.Media;

using Windows.Devices.Enumeration;
using Windows.Devices.WiFi;

namespace PolisHUB.WiFiManagement
{
    class WiFiHandler
    {
		private WiFiAdapter WiFi_Adapter;

		public WiFiHandler()
		{

		}

		public async Task<SolidColorBrush> ScanAdapter()
		{
			DeviceInformationCollection result = await DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());

			if (result.Count >= 1)
			{
				WiFi_Adapter = await WiFiAdapter.FromIdAsync(result[0].Id);
				return await WiFiController();
			}
			else
			{
				return new SolidColorBrush(Colors.Black);
			}
		}

		private async Task<SolidColorBrush> WiFiController()
		{
			if (WiFi_Adapter.NetworkAdapter.GetConnectedProfileAsync() != null)
			{
				var connectedProfile = await WiFi_Adapter.NetworkAdapter.GetConnectedProfileAsync();
				return new SolidColorBrush(Colors.LightGreen);
			}
			else
			{
				return new SolidColorBrush(Colors.DarkRed);
			}
		}
	}
}
