using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace PolisHUB
{
    class HTTPHandler
    {
        HttpClient client = new HttpClient();
        const string LOGIN = "/polis/php/api/login.php";

        public HTTPHandler()
        {
            client.BaseAddress = new Uri("http://polis.inno-school.org");
        }

        #region Login

        public async Task<JObject> HTTPLoginRequest_Async(string Username, string Password)
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, LOGIN);

                string data = string.Format("{0}\"user\":\"{1}\",\"pass\":\"{2}\"{3}", "{", Username, Password, "}");

                var form = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("data", data)
                };

                request.Content = new FormUrlEncodedContent(form);
                var response = await client.SendAsync(request);
                var result = await response.Content.ReadAsStringAsync();

                JObject jsonResult = JObject.Parse(result);

                return jsonResult;
            }
            catch (Exception mex)
            {
                Debug.WriteLine(mex.Message);
            }
            return null;
        }

        #endregion

        #region HomePage

        public async Task<JArray> HTTPThingRequest_Async()
        {
            try
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "/polis/php/api/getUserThingList.php?data=true");

                var response = await client.SendAsync(request);
                var stringResults = await response.Content.ReadAsStringAsync();

                JArray jsonResults = JArray.Parse(stringResults);

                return jsonResults;
            }
            catch(Exception mex)
            {
                Debug.WriteLine(mex.Message);
            }
            return null;
        }

		public async Task<JArray> HTTPThingValueRequest_Async(string thing)
		{
			try
			{
				HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/polis/php/api/getMetricLogs.php");

				string data = string.Format("{0}\"thingTag\":\"{1}\"{2}", "{", thing, "}");

				var form = new List<KeyValuePair<string, string>>
				{
					new KeyValuePair<string, string>("data", data)
				};

				request.Content = new FormUrlEncodedContent(form);
				var response = await client.SendAsync(request);
				var stringResult = await response.Content.ReadAsStringAsync();

				JArray jsonResults = JArray.Parse(stringResult);

				return jsonResults;
			}
			catch (Exception mex)
			{
				Debug.WriteLine(mex.Message);
			}

			return null;
		}
        
        #endregion
    }
}
