using System;

namespace PolisHUB
{
	class ThingValue
	{
		public string Unit { get; set; }
		public float Value { get; set; }
		public DateTime  TimeStamp{ get; set; }

		public ThingValue(float value, DateTime time, string unit)
		{
			Value = value;
			TimeStamp = time;
			Unit = unit;
		}
	}
}
