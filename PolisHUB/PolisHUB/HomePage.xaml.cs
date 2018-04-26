using System;
using System.Net.Http;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace PolisHUB
{
    public sealed partial class HomePage : Page
    {
        HttpClient client;
        public HomePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Object[] obj = e.Parameter as Object[];
            client = obj[0] as HttpClient;
            UserAppBar.Label = obj[1] as String;
        }
    }
}
