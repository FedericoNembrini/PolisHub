using System;
using System.Net.Http;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace PolisHUB
{
    public sealed partial class MainPage : Page
    {
        HttpClient client = new HttpClient();
        public MainPage()
        {
            this.InitializeComponent();
            client.BaseAddress = new Uri("http://polis.inno-school.org");
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/polis/php/api/login.php");

                string data = string.Format("{0}\"user\":\"{1}\",\"pass\":\"{2}\"{3}","{", UsernameBox.Text, PasswordBox.Password, "}");

                var form = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("data", data)
                };

                request.Content = new FormUrlEncodedContent(form);
                var response = await client.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();

                JObject jsonResult = JObject.Parse(result);

                if (jsonResult["status"].ToString() == "error")
                    Message.Text = jsonResult["error"].ToString();
                else
                {
                    Object[] obj = new Object[2];
                    obj[0] = client;
                    obj[1] = UsernameBox.Text;

                    Message.Text = jsonResult["status"].ToString();

                    this.Frame.Navigate(typeof(HomePage), obj);
                }
            }
            catch(Exception mex)
            {
                Debug.WriteLine(mex.Message);
            }


            /*request = new HttpRequestMessage(HttpMethod.Get, "/polis/php/api/getUserThingList.php?data=true");
           
            response = await client.SendAsync(request);
            
            Message.Text = await response.Content.ReadAsStringAsync();
            */
        }
    }
}
