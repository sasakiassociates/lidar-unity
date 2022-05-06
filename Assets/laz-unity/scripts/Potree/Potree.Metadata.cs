using System.IO;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Strategies.Objects
{
	public static partial class Potree
	{

		public static int ReadHierarchy(string path)
		{
			byte[] bytes = null;

			if (File.Exists(path))
				bytes = File.ReadAllBytes(path);

			if (bytes == null)
				return 0;

			return bytes.Length;
		}

		public static PotreeMetaData ReadJsonFromDir(string dir, bool moveToOrigin)
		{
			return ReadJsonFromPath(FormatJsonPath(dir), moveToOrigin);
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="moveToOrigin"></param>
		/// <returns></returns>
		public static PotreeMetaData ReadJsonFromPath(string path, bool moveToOrigin)
		{
			var jsonfile = "";

			using (StreamReader reader = new StreamReader(path, Encoding.Default))
			{
				jsonfile = reader.ReadToEnd();
				reader.Close();
			}

			var data = JsonUtility.FromJson<PotreeMetaData>(jsonfile);

			data.pointByteSize = 0;
			foreach (var pointAttribute in data.attributes)
				data.pointByteSize += pointAttribute.size;

			return data;
		}

		public static string FormatJsonPath(string path)
		{
			if (!path.EndsWith("/"))
				path += "/";

			return path + MetaDataFormats.v2;
		}

		public static class MetaDataFormats
		{
			public const string v2 = "metadata.json";
			public const string v1_8 = "cloud.js";
		}

	}
}