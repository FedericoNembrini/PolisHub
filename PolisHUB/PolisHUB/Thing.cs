using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Threading.Tasks;

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
		public string[] Unit { get; set; }

		private string[] lastValue;

		public string[] LastValue
		{
			get
			{
				return lastValue;
			}

			set
			{
				lastValue = value;
				OnPropertyChanged("LastValue");
			}
		}

		public void OnPropertyChanged(string propertyName)
		{
			// Raise the PropertyChanged event, passing the name of the property whose value has changed.
			this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		public Thing(JObject jsonThing, HTTPHandler handler)
		{
			Unit = new string[2];

			this.handler = handler;
			InitializeThing(jsonThing);
		}

		private async void InitializeThing(JObject jsonThing)
		{
			Tag = jsonThing["tag"].ToString();
			Name = jsonThing["name"].ToString();
			FamilyTag = jsonThing["family_tag"].ToString();
			UserType = (int)(jsonThing["user_type"]);
			AccessLevel = (int)(jsonThing["access_level"]);

			if(Tag == "aaaaaaaaaaaa")
				await GetLastValue_Async();
			//GetValue_Async();
		}

		public async Task<int> GetLastValue_Async()
		{
			var jsonArrayResult = await handler.HTTPThingLastValueRequest_Async(Tag);

			int i = 0;

			foreach (JObject jsonObject in jsonArrayResult)
			{
				LastValue[i] = jsonObject["value"].ToString();
				Unit[i] = jsonObject["unit"].ToString();
				i++;
			}

			return 1;
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
