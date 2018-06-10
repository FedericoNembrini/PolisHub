using Windows.Devices.WiFi;

using PolisHUB.Global;

namespace PolisHUB.WiFiManagement
{
	class WiFiAvailableNetworkAdapter
	{
		public string Ssid { get; set; }
		public string SignalBarsGlyph { get; set; }

		public WiFiAvailableNetworkAdapter(WiFiAvailableNetwork Network)
		{
			Ssid = Network.Ssid;

			switch (Network.SignalBars)
			{
				case 1:
					SignalBarsGlyph = CommonElements.WiFiSignal1;
					break;
				case 2:
					SignalBarsGlyph = CommonElements.WiFiSignal2;
					break;
				case 3:
					SignalBarsGlyph = CommonElements.WiFiSignal3;
					break;
				case 4:
					SignalBarsGlyph = CommonElements.WiFiSignal4;
					break;
				default:
					break;
			}
		}
	}
}
