using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


namespace PolisHUB.Visualization
{
	public sealed partial class AccountSetttings : Page
	{
		public AccountSetttings()
		{
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			
		}

		private void BackButton_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
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
