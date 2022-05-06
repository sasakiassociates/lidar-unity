using System.Collections.Generic;
using System.Linq;

namespace Strategies.Objects
{
	public static partial class Potree
	{
		public static string Log(this double[] items)
		{
			var log = "";
			if (items != null && items.Any())
				for (int i = 0; i < items.Count(); i++)
					log += i <= items.Length ? items[i].ToString() : items[i] + ",";

			return log;
		}

		public static string Log(this IEnumerable<PotreeAttribute> attributes)
		{
			var log = "Attributes\n";

			if (attributes != null)
				foreach (var attribute in attributes)
					log += attribute.ToString();

			return log;
		}
	}
}

// /// <summary>
// /// Loads the hierarchy, but no points are loaded
// /// </summary>
// /// <param name="metaData">MetaData-Object, as received by LoadMetaData</param>
// /// <returns>The Root Node of the point cloud</returns>
// public static Node LoadHierarchyOnly(PotreeMetaData metaData) {
// 	string dataRPath = metaData.octreeDir + "/r/";
// 	// Node rootNode = new Node("", metaData, metaData.boundingBox, null);
// 	LoadHierarchy(dataRPath, metaData, rootNode);
// 	return rootNode;
// }
//
//
// /* Finds a file for a node in the hierarchy.
//     * Assuming hierarchyStepSize is 3 and we are looking for the file 0123456765.bin, it is in:
//     * 012/012345/012345676/r0123456765.bin
//     * 012/345/676/r012345676.bin
//     */
// private static byte[] FindAndLoadFile(string dataRPath, PotreeMetaData metaData, string id, string fileending) {
// 	
// 	int levels = id.Length / metaData.hierarchy.stepSize;
// 	
// 	string path = "";
// 	for (int i = 0; i < levels; i++) {
// 		path += id.Substring(i * metaData.hierarchyStepSize, metaData.hierarchyStepSize) + "/";
// 	}
// 	
// 	path += "r" + id + fileending;
// 	if (File.Exists(metaData.cloudPath + dataRPath + path)){
// 		return File.ReadAllBytes(metaData.cloudPath + dataRPath + path);
// 	}else if(metaData.cloudUrl != null){
// 		Directory.CreateDirectory(Path.GetDirectoryName(metaData.cloudPath + dataRPath + path));
// 		WebClient webClient = new WebClient();
// 		webClient.DownloadFile(metaData.cloudUrl + dataRPath + path, metaData.cloudPath + dataRPath + path);
// 		return File.ReadAllBytes(metaData.cloudPath + dataRPath + path);
// 	}
// 	return null;
// }
//
//         /* Loads the points for just that one node
//        */
//       private static void LoadPoints(string dataRPath, PointCloudMetaData metaData, Node node) {
//           byte[] data = FindAndLoadFile(dataRPath, metaData, node.Name, ".bin");
//           int pointByteSize = metaData.pointByteSize;
//           int numPoints = data.Length / pointByteSize;
//           int offset = 0;
//
//           Vector3[] vertices = new Vector3[numPoints];
//           Color[] colors = new Color[numPoints];
//           //Read in data
//           foreach (PointAttribute1_9 pointAttribute in metaData.pointAttributesList) {
//               if (pointAttribute.name.Equals(PointAttributes.POSITION_CARTESIAN)) {
//                   for (int i = 0; i < numPoints; i++) {
//                       //Reduction to single precision!
//                       //Note: y and z are switched
//                       float x = (float)(System.BitConverter.ToUInt32(data, offset + i * pointByteSize + 0) * metaData.scale/* + node.BoundingBox.lx*/);
//                       float y = (float)(System.BitConverter.ToUInt32(data, offset + i * pointByteSize + 8) * metaData.scale/* + node.BoundingBox.lz*/);
//                       float z = (float)(System.BitConverter.ToUInt32(data, offset + i * pointByteSize + 4) * metaData.scale/* + node.BoundingBox.ly*/);
//                       vertices[i] = new Vector3(x, y, z);
//                   }
//                   offset += 12;
//               } else if (pointAttribute.name.Equals(PointAttributes.COLOR_PACKED)) {
//                   for (int i = 0; i < numPoints; i++) {
//                       byte r = data[offset + i * pointByteSize + 0];
//                       byte g = data[offset + i * pointByteSize + 1];
//                       byte b = data[offset + i * pointByteSize + 2];
//                       colors[i] = new Color32(r, g, b, 255);
//                   }
//                   offset += 3;
//               }else if (pointAttribute.name.Equals(PointAttributes.RGBA)) {
//                   for (int i = 0; i < numPoints; i++) {
//                       byte r = data[offset + i * pointByteSize + 0];
//                       byte g = data[offset + i * pointByteSize + 1];
//                       byte b = data[offset + i * pointByteSize + 2];
//                       byte a = data[offset + i * pointByteSize + 3];
//                       colors[i] = new Color32(r, g, b, a);
//                   }
//                   offset += 4;
//               }
//           }
//           node.SetPoints(vertices, colors);
//       }
//
// private static void LoadHierarchy(string dataRPath, PointCloudMetaData metaData, Node root) {
// 	byte[] data = FindAndLoadFile(dataRPath, metaData, root.Name, ".hrc");
// 	int nodeByteSize = 5;
// 	int numNodes = data.Length / nodeByteSize;
// 	int offset = 0;
// 	Queue<Node> nextNodes = new Queue<Node>();
// 	nextNodes.Enqueue(root);
//
// 	for (int i = 0; i < numNodes; i++) {
// 		Node n = nextNodes.Dequeue();
// 		byte configuration = data[offset];
// 		//uint pointcount = System.BitConverter.ToUInt32(data, offset + 1);
// 		//n.PointCount = pointcount; //TODO: Pointcount is wrong
// 		for (int j = 0; j < 8; j++) {
// 			//check bits
// 			if ((configuration & (1 << j)) != 0) {
// 				//This is done twice for some nodes
// 				Node child = new Node(n.Name + j, metaData, calculateBoundingBox(n.BoundingBox, j), n);
// 				n.SetChild(j, child);
// 				nextNodes.Enqueue(child);
// 			}
// 		}
// 		offset += 5;
// 	}
// 	HashSet<Node> parentsOfNextNodes = new HashSet<Node>();
// 	while (nextNodes.Count != 0) {
// 		Node n = nextNodes.Dequeue().Parent;
// 		if (!parentsOfNextNodes.Contains(n)) {
// 			parentsOfNextNodes.Add(n);
// 			LoadHierarchy(dataRPath, metaData, n);
// 		}
// 		//Node n = nextNodes.Dequeue();
// 		//LoadHierarchy(dataRPath, metaData, n);
// 	}
// }