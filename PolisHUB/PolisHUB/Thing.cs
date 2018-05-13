using Newtonsoft.Json.Linq;

using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace PolisHUB
{
	class Thing : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged = delegate { };
		private HTTPHandler handler;

		public string Tag { get; set; }
		public string Name { get; set; }
		public string FamilyTag { get; set; }
		public int UserType { get; set; }
		public int AccessLevel { get; set; }

		public ObservableCollection<string> Unit { get; set; } = new ObservableCollection<string>();
		public ObservableCollection<string> LastValue { get; set; } = new ObservableCollection<string>();

		public Thing(JObject jsonThing, HTTPHandler handler)
		{
			this.handler = handler;
			InitializeThing(jsonThing);
		}

		private void InitializeThing(JObject jsonThing)
		{
			Tag = jsonThing["tag"].ToString();
			Name = jsonThing["name"].ToString();
			FamilyTag = jsonThing["family_tag"].ToString();
			UserType = (int)(jsonThing["user_type"]);
			AccessLevel = (int)(jsonThing["access_level"]);

			GetLastValue_Async();
			//GetValue_Async();
		}

		public async Task<int> GetLastValue_Async()
		{
			JArray jsonArrayResult;

			int i = 0;

			while (true)
			{
				jsonArrayResult = await handler.HTTPThingLastValueRequest_Async(Tag);

				if (jsonArrayResult != null)
				{
					while (Unit.Count < jsonArrayResult.Count)
					{
						Unit.Add("");
						LastValue.Add("");
					}

					foreach (JObject jsonObject in jsonArrayResult)
					{
						LastValue[i] = jsonObject["value"].ToString();
						Unit[i] = jsonObject["unit"].ToString();
						i++;
					}
				}
				await Task.Delay(60000);
			}
		}

		private async void GetValue_Async()
		{
			JArray jsonArrayResult = await handler.HTTPThingValueRequest_Async(Tag);

			if (jsonArrayResult != null)
			{
				foreach (JObject jsonObject in jsonArrayResult)
				{
					Unit[0] = jsonObject["metric"].ToString();
				}
			}
		}
	}
}
