using System;
using System.Collections.Generic;
using UnityEngine;

namespace Strategies.Objects
{

	public struct BoundingBox
	{
		public readonly Vector3 min;
		public readonly Vector3 max;

		public BoundingBox(Vector3 min, Vector3 max)
		{
			this.min = min;
			this.max = max;
		}
	}

	public struct LazWrapper
	{
		public List<LazPoint> points;
		
		public LazWrapper(List<LazPoint> pts = null)
		{
			points = pts != null && pts.Count > 0 ? pts : new List<LazPoint>();
		}

	}

	public readonly struct LazPoint
	{
		public readonly byte classification;
		public readonly Color32 color;
		public readonly Vector3 pos;

		public LazPoint(byte classification, Color32 color, Vector3 pos)
		{
			this.classification = classification;
			this.color = color;
			this.pos = pos;
		}

	}

	public readonly struct Las
	{
		public readonly List<Vector3> points;
		public readonly List<ushort> colors;
		public readonly List<byte> classifications;

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