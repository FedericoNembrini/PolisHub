using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace PolisHUB
{
    public sealed partial class ThingVisualization : Page
    {
		Thing thing;

        public ThingVisualization()
        {
            this.InitializeComponent();
        }

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			Object obj = e.Parameter as Object;
			thing = obj as Thing;
			this.DataContext = thing;
			 
			InitializeUI();
		}

		private async void InitializeUI()
		{
			ProgressRing_01.IsActive = true;

			ThingName.Text = thing.Name;

			int counterRetry = 0;
			while (!await thing.GetValue_Async())
			{
				if (counterRetry > 4)
					TextBlock_Connection_Error.Visibility = Windows.UI.Xaml.Visibility.Visible;
				counterRetry++;

				await Task.Delay(5000);
			}

			RadioButtonBindings();

			ThingVisualization_List.ItemsSource = thing.listValue[0];
			radioButton1.IsChecked = true;

			ProgressRing_01.IsActive = false;
			ProgressRingPanel.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

			ThingVisualizationSettings.Visibility = Windows.UI.Xaml.Visibility.Visible;
		}

		private void RadioButtonBindings()
		{
			if (thing.Unit.Count > 1)
				radioButton2.Visibility = 0;
		}

		private void RadioButtonChecked_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			RadioButton radioButton = (sender as RadioButton);
			int i = 0;

			while (thing.Unit.Count > i)
			{
				if (radioButton.Content.ToString() == thing.UnitName[i])
				{
					ThingVisualization_List.ItemsSource = thing.listValue[i];
					break;
				}
				i++;
			}
		}

		private void BackButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
		{
			On_BackRequested();
		}

		private bool On_BackRequested()
		{
			if(this.Frame.CanGoBack)
			{
				this.Frame.GoBack();
				return true;
			}
			return false;
		}
	}
}
