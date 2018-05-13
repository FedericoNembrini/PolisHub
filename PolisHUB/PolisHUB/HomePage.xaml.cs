using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace PolisHUB
{
    public sealed partial class HomePage : Page
    {
        HTTPHandler handler;
        List<Thing> things = new List<Thing>();

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

            LoadThingList_Async();
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
    }
}
