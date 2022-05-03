using System;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.Objects
{



	public readonly struct Las
	{
		public readonly List<Vector3> points;
		public readonly List<ushort> colors;
		public readonly List<Byte> classifications;

		public Las(List<Vector3> points, List<ushort> colors, List<byte> classifications)
		{
			this.points = points;
			this.colors = colors;
			this.classifications = classifications;
		}

		public override string ToString()
		{
			return$"Total Point Count: {points.Count}\nTotal Color Count: {colors.Count}\nTotal Class Count: {classifications.Count}";
		}
	}

}