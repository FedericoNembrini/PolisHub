using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace PolisHUB
{
    class Thing
    {
		private HTTPHandler handler;

        public string Tag { get; set; }
        public string Name { get; set; }
        public string FamilyTag { get; set; }
        public int UserType { get; set; }
        public int AccessLevel { get; set; }
		public int[] LastValue { get; set; }

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
            UserType = (int) (jsonThing["user_type"]);
            AccessLevel = (int)(jsonThing["access_level"]);

			GetValue_Async();
        }

		private async void GetValue_Async()
		{
			JArray jsonArrayResult = await handler.HTTPThingValueRequest_Async(Tag);
			
			foreach(JObject jsonObject in jsonArrayResult)
			{
				Debug.WriteLine(jsonObject);
			}
		}
    }
}
