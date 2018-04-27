using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolisHUB
{
    class Thing
    {
        public string Tag { get; set; }
        public string Name { get; set; }
        public string FamilyTag { get; set; }
        public int UserType { get; set; }
        public int AccessLevel { get; set; }

        public Thing(JObject jsonThing)
        {
            InitializeThing(jsonThing);
        }

        private void InitializeThing(JObject jsonThing)
        {
            Tag = jsonThing["tag"].ToString();
            Name = jsonThing["name"].ToString();
            FamilyTag = jsonThing["family_tag"].ToString();
            UserType = (int) (jsonThing["user_type"]);
            AccessLevel = (int)(jsonThing["access_level"]);
        }
    }
}
