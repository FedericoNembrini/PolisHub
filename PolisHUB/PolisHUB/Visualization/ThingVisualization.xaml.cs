using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace PolisHUB
{
    public sealed partial class ThingVisualization : Page
    {
        public ThingVisualization()
        {
            this.InitializeComponent();
        }

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			Object obj = e.Parameter as Object;
			Thing thing = obj as Thing;
			ThingName.Text = thing.Name;

			await thing.GetValue_Async();

			ThingVisualization_List.ItemsSource = thing.listValue[0];
		}
	}
}
