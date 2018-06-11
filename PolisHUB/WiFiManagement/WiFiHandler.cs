using System;
using System.Threading.Tasks;

using Windows.UI;
using Windows.UI.Xaml.Media;

using Windows.Devices.Enumeration;
using Windows.Devices.WiFi;
using System.Collections.Generic;
using Windows.Security.Credentials;

namespace PolisHUB.WiFiManagement
{
	class WiFiHandler
	{
		private WiFiAdapter WiFiAdapter;
		private DeviceInformationCollection DeviceInformationResult;
		private WiFiAccessStatus VerifyAccessStatus;
		public string WiFiConnectedName;

		public WiFiHandler() {}

		public async Task WiFi_AdapterInitialization()
		{
			VerifyAccessStatus = await WiFiAdapter.RequestAccessAsync();

			DeviceInformationResult = await DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
			if(DeviceInformationResult.Count >= 1)
				WiFiAdapter = await WiFiAdapter.FromIdAsync(DeviceInformationResult[0].Id);
		}

		public async Task<SolidColorBrush> ScanAdapter()
		{
			if ((WiFiAccessStatus.Allowed == VerifyAccessStatus) && (DeviceInformationResult.Count >= 1))
				return (await WiFiController());
			else
				return (new SolidColorBrush(Colors.Black));
		}

		public async Task<SolidColorBrush> WiFiController()
		{
			if (WiFiAdapter.NetworkAdapter.GetConnectedProfileAsync() != null)
			{
				var connectedProfile = await WiFiAdapter.NetworkAdapter.GetConnectedProfileAsync();

				if (connectedProfile != null)
				{
					WiFiConnectedName = connectedProfile.ProfileName;
					return (new SolidColorBrush(Colors.LightGreen));
				}
				else
					return (new SolidColorBrush(Colors.DarkRed));
			}
			else
				return (new SolidColorBrush(Colors.DarkRed));
		}

		public async Task<List<WiFiAvailableNetworkAdapter>> ScanNetworks()
		{
			List<WiFiAvailableNetworkAdapter> WiFiListNetworksAdapted = new List<WiFiAvailableNetworkAdapter>();

			if (WiFiAdapter != null)
			{
				await WiFiAdapter.ScanAsync();

				foreach (var availableNetwork in WiFiAdapter.NetworkReport.AvailableNetworks)
				{
					if(availableNetwork.Ssid != "")
						WiFiListNetworksAdapted.Add(new WiFiAvailableNetworkAdapter(availableNetwork));
				}
				return (WiFiListNetworksAdapted);
			}
			return (null);
		}

		public async Task<WiFiConnectionStatus> ConnectToWiFi(WiFiAvailableNetwork AvailableNetowrk, string PasswordCredentialString)
		{
			PasswordCredential Credential = new PasswordCredential();
			Credential.Password = PasswordCredentialString;
			Credential.UserName = "PolisHUB";

			WiFiConnectionResult ConnectionResult = await WiFiAdapter.ConnectAsync(AvailableNetowrk, WiFiReconnectionKind.Automatic, Credential);

			return ConnectionResult.ConnectionStatus;
		}
	}
}
