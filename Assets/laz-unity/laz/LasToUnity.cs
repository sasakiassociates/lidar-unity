using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;
using laszip.net;
using Strategies.Objects;

public class LasToUnity : MonoBehaviour
{

	[SerializeField] private string filePath;

	private const string v353 = "115F75F2BEEC550E9246B2944870E38B";
	private const string v28 = "2022-04-13-45A54-equator-point";
	private readonly string testPath = "D:\\Projects\\ViewTo\\viewto-projects\\inglewood\\laz\\" + $"{v28}.laz";

	public async void Start()
	{
		Debug.Log("Starting read");

		var timer = Time.time;

		var las = await ReadFile();

		Debug.Log($"Completed read in {Time.time - timer}");

		var classifications = new HashSet<byte>();

		for (int i = 0; i < las.points.Count; i++)
		{
			var point = las.points[i];
			if (i < 100)			
				Debug.Log($"classification: {point.classification}\ncolor: {point.color}\nposition: {point.pos}");

			classifications.Add(point.classification);
			}

		Debug.Log($"Total classification count {classifications.Count}");
	}

	public async UniTask<LazWrapper> ReadFile()
	{
		var filename = Path.GetFullPath(testPath);

		var lazReader = new laszip_dll();

		var compressed = true;
		lazReader.laszip_open_reader(filename, ref compressed);
		var numberOfPoints = lazReader.header.number_of_point_records;
		
		var points = new List<LazPoint>();

		var coordArray = new double[3];

		UniTask.SwitchToThreadPool();

		await UniTask.RunOnThreadPool(() =>
		{
			for (int pointIndex = 0; pointIndex < numberOfPoints; pointIndex++)
			{
				lazReader.laszip_read_point();

				lazReader.laszip_get_coordinates(coordArray);

				var classification = lazReader.point.classification;

				var c = System.Drawing.Color.FromArgb(lazReader.point.rgb[0], lazReader.point.rgb[1], lazReader.point.rgb[2]);

				points.Add(new LazPoint(
					           classification,
					           new Color32(c.R, c.G, c.B, c.A),
					           new Vector3((float)coordArray[0], (float)coordArray[1], (float)coordArray[2])
				           ));
			}
		});

		UniTask.SwitchToMainThread();

		lazReader.laszip_close_reader();

		return new LazWrapper(points);
	}

	private BoundingBox CreateBoundingBox(laszip_header header)
	{
		return new BoundingBox(
			new Vector3((float)header.min_x, (float)header.min_z, (float)header.min_y),
			new Vector3((float)header.min_x, (float)header.min_z, (float)header.min_y));
	}

	// from blue heron comp https://github.com/blueherongis/Heron/blob/64df88cbf606995e4e70055c82a7ee9444f05840/Heron/Components/GIS%20Import-Export/ImportLAZ.cs#L83
	private string GetSRS(List<laszip_vlr> vlrs)
	{
		var pcSRS = "Data does not have associated spatial reference system (SRS).";

		foreach (var vlr in vlrs)
		{
			var description = Encoding.Default.GetString(vlr.description);
			if (description.Contains("SRS") || description.Contains("WKT"))
				pcSRS = Encoding.Default.GetString(vlr.data);
		}

		return pcSRS;
	}
}