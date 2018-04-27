using System;
using System.Net.Http;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace PolisHUB
{
    public sealed partial class HomePage : Page
    {
        HTTPHanderl handler;
        public HomePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Object[] obj = e.Parameter as Object[];
            handler = obj[0] as HTTPHanderl;
            UserAppBar.Label = obj[1] as String;

            handler.HTTPThingRequest_Async();
        }
    }
}
