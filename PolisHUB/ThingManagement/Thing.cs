using Newtonsoft.Json.Linq;

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Generic;

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

		public ObservableCollection<string> UnitName { get; set; } = new ObservableCollection<string>();
		public ObservableCollection<string> Unit { get; set; } = new ObservableCollection<string>();
		public ObservableCollection<string> LastValue { get; set; } = new ObservableCollection<string>();
		public List<ThingValue>[] listValue = new List<ThingValue>[2];

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
					int x = 0;
					while (Unit.Count < jsonArrayResult.Count)
					{
						UnitName.Add("");
						Unit.Add("");
						LastValue.Add("");
						
						listValue[x] = new List<ThingValue>();
						x++;
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

		public async Task<bool> GetValue_Async()
		{
			JObject jsonObjectResult = await handler.HTTPThingValueRequest_Async(Tag);
			
			if(jsonObjectResult != null)
			{
				int i = 0;
				foreach(JObject objectMetricValue in jsonObjectResult["metricList"])
				{
					UnitName[i] = objectMetricValue["metric"].ToString();
					foreach(JObject objectValue in objectMetricValue["list"])
					{
						listValue[i].Add(new ThingValue((float)objectValue["value"], (DateTime) objectValue["time_stamp"], objectMetricValue["unit"].ToString()));
					}
					i++;
				}
				return true;
			}
			return false;
		}
	}
}
