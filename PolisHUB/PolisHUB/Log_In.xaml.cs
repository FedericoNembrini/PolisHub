using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json.Linq;

namespace PolisHUB
{
    public sealed partial class MainPage : Page
    {
        HTTPHanderl handler = new HTTPHanderl();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginRequestAsync();
        }

        private async void LoginRequestAsync()
        {
			JObject status = await handler.HTTPLoginRequest_Async("admin", "password");
							//await handler.HTTPLoginRequest_Async(UsernameBox.Text, PasswordBox.Password);

			if (status == null)
            {
                Message.Text = "Site Offline or No Internet Connection";
            }
            else if(status["status"].ToString() == "error")
                    Message.Text = status["error"].ToString();
            else
            {
                Object[] obj = new Object[2];
                obj[0] = handler;
                obj[1] = UsernameBox.Text;

                this.Frame.Navigate(typeof(HomePage), obj);
            }
        }
    }
}
