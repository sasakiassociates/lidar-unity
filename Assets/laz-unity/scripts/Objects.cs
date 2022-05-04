using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


namespace Strategies.Objects
{

	public struct OctreeNode
	{
		// node value
		public float3 position;
		public float3 voxelPosition;
		public float size;
		public byte lodLevel;

		public OctreeNode
		(
			float3 position,
			float3 voxelPosition,
			float size,
			byte lodLevel
		)
		{
			this.position = position;
			this.voxelPosition = voxelPosition;
			this.size = size;
			this.lodLevel = lodLevel;
		}
	}

	
	public class Octree<TType>
	{
		private OctreeNode<TType> node;
		
		/// <summary>
		/// number of sub divisions
		/// </summary>
		private int depth;

		private class OctreeNode<TType>
		{
			private OctreeNode<TType> subNodes;
			private IList<TType> values;
		}

	}

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