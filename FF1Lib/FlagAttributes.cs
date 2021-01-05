using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FF1Lib
{
	[AttributeUsage(AttributeTargets.Property)]
	public class IntegerFlagAttribute : Attribute
	{
		public int Min { get; set; }

		public int Max { get; set; }

		public int SmallestStep { get; set; }

		public IntegerFlagAttribute(int min, int max, int smalleststep = 1)
		{
			Min = min;
			Max = max;
			SmallestStep = smalleststep;
		}
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class DoubleFlagAttribute : Attribute
	{
		public double Min { get; set; }

		public double Max { get; set; }

		public double SmallestStep { get; set; }

		public DoubleFlagAttribute(double min, double max, double smalleststep)
		{
			Min = min;
			Max = max;
			SmallestStep = smalleststep;
		}
	}
}
