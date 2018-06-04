using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json.Linq;

namespace PolisHUB
{
    public sealed partial class MainPage : Page
    {
        HTTPHandler handler = new HTTPHandler();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
			LoadingProgressRing.IsActive = true;
			LoginRequestAsync();
		}

        private async void LoginRequestAsync()
        {
			JObject status = await handler.HTTPLoginRequest_Async("admin", "password");
			//JObject status = await handler.HTTPLoginRequest_Async(UsernameBox.Text, PasswordBox.Password);

			if (status == null)
            {
				Message.Visibility = Visibility.Visible;
                Message.Text = "Site Offline or No Internet Connection";
            }
            else if(status["status"].ToString() == "error")
			{
				Message.Text = status["error"].ToString();
				Message.Visibility = Visibility.Visible;
			}
            else
            {
                Object[] obj = new Object[2];
                obj[0] = handler;
                obj[1] = UsernameBox.Text;

                this.Frame.Navigate(typeof(HomePage), obj);
			}
			LoadingProgressRing.IsActive = false;
		}
    }
}
