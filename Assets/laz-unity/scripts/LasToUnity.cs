using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Timers;
using Cysharp.Threading.Tasks;
using UnityEngine;
using laszip.net;
using Strategies.Objects;

public class LasToUnity : MonoBehaviour
{

	[SerializeField] private string filePath;

	private const string v353 = "115F75F2BEEC550E9246B2944870E38B";
	private const string v28 = "2022-04-13-45A54-equator-point";
	private readonly string testPath = "D:\\Projects\\ViewTo\\viewto-projects\\inglewood\\" + $"{v28}.laz";

	public async void Start()
	{
		Debug.Log("Starting read");

		var timer = Time.time;

		await UniTask.SwitchToThreadPool();
		
		var las = await ReadFile();

		await UniTask.SwitchToMainThread();
		
		Debug.Log($"Completed read in {Time.time - timer}");
		Debug.Log(las.ToString());

		var setClass = new HashSet<byte>();
		var setColor = new HashSet<ushort>();

		foreach (var co in las.classifications)
			setClass.Add(co);

		foreach (var co in las.colors)
			setColor.Add(co);

		foreach (var c in setColor)
			Debug.Log($"Color as Ushort {c}");
		foreach (var c in setClass)
			Debug.Log($"Class as Byte {c}");
	}

	public async UniTask<Las> ReadFile()
	{
		var filename = Path.GetFullPath(testPath);

		var lazReader = new laszip_dll();

		var compressed = true;
		lazReader.laszip_open_reader(filename, ref compressed);
		var numberOfPoints = lazReader.header.number_of_point_records;

		var vlrs = lazReader.header.vlrs;

		var colors = new List<ushort>();
		var points = new List<Vector3>();
		var classifications = new List<Byte>();

		var coordArray = new double[3];

		for (int pointIndex = 0; pointIndex < numberOfPoints; pointIndex++)
		{
			// Read the point
			lazReader.laszip_read_point();

			// Get precision coordinates
			lazReader.laszip_get_coordinates(coordArray);
			points.Add(new Vector3((float)coordArray[0], (float)coordArray[1], (float)coordArray[2]));

			// Get classification value for sorting into branches
			var classification = lazReader.point.classification;
			classifications.Add(classification);

			colors.AddRange(lazReader.point.rgb);
		}
		lazReader.laszip_close_reader();

		return new Las(points, colors, classifications);
	}
}