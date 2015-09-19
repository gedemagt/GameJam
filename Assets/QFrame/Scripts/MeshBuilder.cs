using UnityEngine;
using System.Collections;
using System;

public class MeshBuilder : MonoBehaviour
{
		public MeshBuilder ()
		{
		}

		static public Mesh fillLineMesh (float[] topXValues, float[] topYValues, float[] bottomXValues, float[] bottomYValues, float width)
		{
				Mesh mesh = new Mesh ();
				Vector3[] newVertices;
				Vector2[] newUVs;
				int[] newTriangles;

				int resolutionOfPath = topXValues.Length;

				Vector3[] topVerts = makeStripVertices (topXValues, topYValues, width);
				Vector2[] topUVs = makeTopUVs (topVerts);
				int[] topTris = makeTopTris (topVerts);

				Vector3[] bottomVerts = makeStripVertices (bottomXValues, bottomYValues, width);
				Vector2[] bottomUVs = makeBottomUVs (bottomVerts);
				int[] bottomTris = makeBottomTris (bottomVerts);

				for (int i = 0; i< bottomTris.Length; i++) {
						bottomTris [i] += topVerts.Length;
				}

				Vector3[] endVerts = MakeEndVertices (topXValues,topYValues,bottomYValues,width);
			
				Vector2[] endUVs = MakeEndUVs (topXValues, width);
			
				int endsOffset = topVerts.Length + bottomVerts.Length;
				int[] ends = MakeEndTris (endsOffset);
			
			
				Vector3[] side1Verts = MakeSide1Vertices(topXValues,topYValues,bottomXValues,bottomYValues);
			
				Vector2[] side1UVs = MakeSide1UVs (side1Verts,topYValues,resolutionOfPath);

				int side1Offset = endsOffset + endVerts.Length;
				int[] side1 = MakeSide1Tris (side1Offset,topVerts);
			
				Vector3[] side2Verts = MakeSide2Vertices (topXValues,topYValues,bottomXValues,bottomYValues,width);
			
				Vector2[] side2UVs = MakeSide2UVs (side2Verts,topYValues,resolutionOfPath);
			
				int side2Offset = side1Offset + side1Verts.Length;
				int[] side2 = MakeSide2Tris (side2Offset,topVerts);
			
				newVertices = new Vector3[topVerts.Length + bottomVerts.Length + endVerts.Length + side1Verts.Length + side2Verts.Length];
				topVerts.CopyTo (newVertices, 0);
				bottomVerts.CopyTo (newVertices, topVerts.Length);
				endVerts.CopyTo (newVertices, topVerts.Length + bottomVerts.Length);
				side1Verts.CopyTo (newVertices, topVerts.Length + bottomVerts.Length + endVerts.Length);
				side2Verts.CopyTo (newVertices, topVerts.Length + bottomVerts.Length + endVerts.Length + side1Verts.Length);
			
				newUVs = new Vector2[topUVs.Length + bottomUVs.Length + endUVs.Length + side1UVs.Length + side2UVs.Length];
				topUVs.CopyTo (newUVs, 0);
				bottomUVs.CopyTo (newUVs, topUVs.Length);
				endUVs.CopyTo (newUVs, topUVs.Length + bottomUVs.Length);
				side1UVs.CopyTo (newUVs, topUVs.Length + bottomUVs.Length + endUVs.Length);
				side2UVs.CopyTo (newUVs, topUVs.Length + bottomUVs.Length + endUVs.Length + side1UVs.Length);
			
				newTriangles = new int[topTris.Length + bottomTris.Length + ends.Length + side1.Length + side2.Length];
				topTris.CopyTo (newTriangles, 0);
				bottomTris.CopyTo (newTriangles, topTris.Length);
				ends.CopyTo (newTriangles, topTris.Length + bottomTris.Length);
				side1.CopyTo (newTriangles, topTris.Length + bottomTris.Length + ends.Length);
				side2.CopyTo (newTriangles, topTris.Length + bottomTris.Length + ends.Length + side1.Length);
			
				mesh.vertices = newVertices;
				mesh.uv = newUVs;
				mesh.triangles = newTriangles;
				mesh.RecalculateNormals ();
			
				//Debug.Log ("Built new line mesh!");
				return mesh;
		}

		/*****************************************************************************
		 * 	Recomputation of vertices assume that the mesh has completely standard rotation
		 * 
		 *****************************************************************************/
//		public static Vector3[] RecomputeVertices (Potential potential, Mesh mesh){
//			Vector3[] newVertices;
//			Vector3[] oldVertices = mesh.vertices;
//
//			int resolutionOfPath = (oldVertices.Length-8)/8;
//
//			double minXValue = oldVertices [0].x;
//			double maxXValue = oldVertices [2*resolutionOfPath - 1].x;
//
//			//Debug.Log ("Min xvalue was " + minXValue + " and max XValue was " + maxXValue);
//			double[] xDoubles = Grid.linspace (minXValue, maxXValue, resolutionOfPath);
//			//double[] topXValues = ArrayUtils.toFloatArray (xDoubles);
//		double[] topXValues = xDoubles;
//		
//			//double[] topYValues = ArrayUtils.toFloatArray(potential.getValues(xDoubles,0));
//			double[] topYValues = potential.getValues(xDoubles,0);
//
//			double[] bottomXValues = topXValues;
//
//			// We assume a flat bottom
//			double minYValue = oldVertices [2*resolutionOfPath + 2].y;
//			//double[] bottomYValues = ArrayUtils.todoubleArray(Grid.linspace (minYValue, minYValue, resolutionOfPath));
//			double[] bottomYValues = Grid.linspace (minYValue, minYValue, resolutionOfPath);
//
//			double width = Mathf.Abs (oldVertices [0].z - oldVertices [1].z);
//
//			Vector3[] topVerts = makeStripVertices (topXValues, topYValues, width);
//			Vector3[] bottomVerts = makeStripVertices (bottomXValues, bottomYValues, width);
//			Vector3[] endVerts = MakeEndVertices (topXValues,topYValues,bottomYValues,width);
//			Vector3[] side1Verts = MakeSide1Vertices(topXValues,topYValues,bottomXValues,bottomYValues);
//			Vector3[] side2Verts = MakeSide2Vertices (topXValues,topYValues,bottomXValues,bottomYValues,width);
//
//			newVertices = new Vector3[topVerts.Length + bottomVerts.Length + endVerts.Length + side1Verts.Length + side2Verts.Length];
//			topVerts.CopyTo (newVertices, 0);
//			bottomVerts.CopyTo (newVertices, topVerts.Length);
//			endVerts.CopyTo (newVertices, topVerts.Length + bottomVerts.Length);
//			side1Verts.CopyTo (newVertices, topVerts.Length + bottomVerts.Length + endVerts.Length);
//			side2Verts.CopyTo (newVertices, topVerts.Length + bottomVerts.Length + endVerts.Length + side1Verts.Length);
//
//			return newVertices;
//		}

		static public void RecomputeLineMeshVertices (float[] topXValues, float[] topYValues, float[] bottomXValues, float[] bottomYValues, float width, Mesh mesh)
		{
			Vector3[] newVertices;
		
			Vector3[] topVerts = makeStripVertices (topXValues, topYValues, width);
			Vector3[] bottomVerts = makeStripVertices (bottomXValues, bottomYValues, width);
			Vector3[] endVerts = MakeEndVertices (topXValues,topYValues,bottomYValues,width);
			Vector3[] side1Verts = MakeSide1Vertices(topXValues,topYValues,bottomXValues,bottomYValues);
			Vector3[] side2Verts = MakeSide2Vertices (topXValues,topYValues,bottomXValues,bottomYValues,width);

			newVertices = new Vector3[topVerts.Length + bottomVerts.Length + endVerts.Length + side1Verts.Length + side2Verts.Length];
			topVerts.CopyTo (newVertices, 0);
			bottomVerts.CopyTo (newVertices, topVerts.Length);
			endVerts.CopyTo (newVertices, topVerts.Length + bottomVerts.Length);
			side1Verts.CopyTo (newVertices, topVerts.Length + bottomVerts.Length + endVerts.Length);
			side2Verts.CopyTo (newVertices, topVerts.Length + bottomVerts.Length + endVerts.Length + side1Verts.Length);

			mesh.vertices = newVertices;
			mesh.RecalculateNormals ();
		
		}

		/*****************************************************************************
		 * 
		 * 		Vertex generation functions
		 * 
		 * 
		 ******************************************************************************/
		private static Vector3[] makeStripVertices (float[] xPositions, float[] yPositions, float width)
		{
			
				if (xPositions.Length != yPositions.Length) {
						return new Vector3[0];
				}
				Vector3[] verts = new Vector3[xPositions.Length * 2];
				for (int i = 0; i < xPositions.Length; i++) {
			verts [2 * i] = new Vector3 ( xPositions [i],  yPositions [i], 0f);
			verts [2 * i + 1] = new Vector3 ( xPositions [i],  yPositions [i],  width);
						
				}
				return verts;
		}

		private static Vector3[] MakeEndVertices(float[] topXValues, float[] topYValues, float[] bottomYValues, float width){
			return new Vector3[] {
			new Vector3 ( topXValues [0],  topYValues [0],  0),
			new Vector3 ( topXValues [0],  topYValues [0],  width),
			new Vector3 ( topXValues [0],  bottomYValues [0],  0),
			new Vector3 ( topXValues [0],  bottomYValues [0],  width),
			new Vector3 ( topXValues [topXValues.Length - 1],  topYValues [0],  0),
			new Vector3 ( topXValues [topXValues.Length - 1],  topYValues [0],  width),
			new Vector3 ( topXValues [topXValues.Length - 1],  bottomYValues [0],  0),
			new Vector3 ( topXValues [topXValues.Length - 1],  bottomYValues [0],  width)
			};
		}

	private static Vector3[] MakeSide1Vertices(float[] topXValues, float[] topYValues, float[] bottomXValues, float[] bottomYValues){
			Vector3[] side1Verts = new Vector3[topYValues.Length * 2];
			for (int i = 0; i < topYValues.Length; i++) {
			side1Verts [2 * i] = new Vector3 ( topXValues [i],  topYValues [i],  0);
			side1Verts [2 * i + 1] = new Vector3 ( bottomXValues [i],  bottomYValues [i],  0);
			}
			return side1Verts;
		}

		private static Vector3[] MakeSide2Vertices(float[] topXValues, float[] topYValues, float[] bottomXValues, float[] bottomYValues, float width){
			Vector3[] side2Verts = new Vector3[topYValues.Length * 2];
			for (int i = 0; i < topYValues.Length; i++) {
			side2Verts [2 * i] = new Vector3 ( topXValues [i],  topYValues [i],  width);
			side2Verts [2 * i + 1] = new Vector3 ( bottomXValues [i],  bottomYValues [i],  width);
			}
			return side2Verts;
		}


		/*****************************************************************************
		 * 
		 * 		UV generation functions
		 * 		THESE ARE NOT COMPLETELY FUNCTIONAL AS THEY MAKE COORDINATES LARGER THAN 1
		 * 
		 ******************************************************************************/
		
		private static Vector2[] makeTopUVs (Vector3[] verts)
		{
			
				Vector2[] UVs = new Vector2[verts.Length];
				for (int i = 0; i < verts.Length/2; i++) {
						UVs [2 * i] = new Vector2 (i * 4.0f / verts.Length, 0);
						UVs [2 * i + 1] = new Vector2 (i * 4.0f / verts.Length, 0.15f);
				}
				return UVs;
		}

		private static Vector2[] MakeEndUVs (float[] topXValues, float width){
			return new Vector2[]{
			new Vector2 (0, 2.0f * 0.15f), new Vector2 ( (width / (topXValues [topXValues.Length - 1] - topXValues [0])), 2.0f * 0.15f),
			new Vector2 (0, 3.0f * 0.15f), new Vector2 ( (width / (topXValues [topXValues.Length - 1] - topXValues [0])), 3.0f * 0.15f),
			new Vector2 ( (width / (topXValues [topXValues.Length - 1] - topXValues [0])), 2.0f * 0.15f), new Vector2 ( (2 * width / (topXValues [topXValues.Length - 1] - topXValues [0])), 2.0f * 0.15f),
			new Vector2 ( (width / (topXValues [topXValues.Length - 1] - topXValues [0])), 3.0f * 0.15f), new Vector2 ( (2 * width / (topXValues [topXValues.Length - 1] - topXValues [0])), 3.0f * 0.15f)
			};
		}
		
		private static Vector2[] makeBottomUVs (Vector3[] verts)
		{
			
				Vector2[] UVs = new Vector2[verts.Length];
				for (int i = 0; i < verts.Length/2; i++) {
					UVs [2 * i] = new Vector2 ( (i * 3.0f / verts.Length),  0.15f);
					UVs [2 * i + 1] = new Vector2 ( (i * 3.0f / verts.Length),  (2.0f * 0.15f));
				}
				return UVs;
		}

		private static Vector2[] MakeSide1UVs (Vector3[] side1Verts, float[] topYValues, int resolutionOfPath){
			Vector2[] side1UVs = new Vector2[side1Verts.Length];
			for (int i = 0; i < topYValues.Length; i++) {
			side1UVs [2 * i] = new Vector2 ( (i * 1.0f / resolutionOfPath),  (3.0f * 0.15f + 0.50f - (topYValues [i] + 1.0f) / (Mathf.Max(topYValues) + 1.0f) * 0.50f));
			side1UVs [2 * i + 1] = new Vector2 ( (i * 1.0f / resolutionOfPath),  (3.0f * 0.15f + 0.50f));
			} 
			return side1UVs;
		}

		private static Vector2[] MakeSide2UVs(Vector3[] side2Verts, float[] topYValues, int resolutionOfPath){
			Vector2[] side2UVs = new Vector2[side2Verts.Length];
			for (int i = 0; i < topYValues.Length; i++) {
			side2UVs [2 * i] = new Vector2 ( (i * 1.0f / resolutionOfPath),  (3.0f * 0.15f + 2.0f * 0.50f - (topYValues [i] + 1.0f) / (Mathf.Max(topYValues) + 1.0f) * 0.50f));
			side2UVs [2 * i + 1] = new Vector2 ( (i * 1.0f / resolutionOfPath),  (3.0f * 0.15f + 2.0f * 0.50f));
			}
			return side2UVs;
		}

		/*****************************************************************************
		 * 
		 * 		Triangle generation functions
		 * 
		 * 
		 ******************************************************************************/
		
		private static int[] makeTopTris (Vector3[] verts)
		{
				if (verts.Length < 3) {
						return new int[0];		
				}
				int[] tris = new int[(verts.Length - 2) * 3];
				for (int i = 0; i < verts.Length-2; i++) {
						if (i % 2 == 0) {
								tris [3 * i] = i;
								tris [3 * i + 1] = i + 1;
								tris [3 * i + 2] = i + 2;
						} else {
								tris [3 * i] = i;
								tris [3 * i + 1] = i + 2;
								tris [3 * i + 2] = i + 1;
						}
				}
				return tris;
		}
		
		private static int[] makeBottomTris (Vector3[] verts)
		{
				if (verts.Length < 3) {
						return new int[0];		
				}
				int[] tris = new int[(verts.Length - 2) * 3];
				for (int i = 0; i < verts.Length-2; i++) {
						if (i % 2 == 0) {
								tris [3 * i] = i;
								tris [3 * i + 1] = i + 2;
								tris [3 * i + 2] = i + 1;
						} else {
								tris [3 * i] = i;
								tris [3 * i + 1] = i + 1;
								tris [3 * i + 2] = i + 2;
						}
				}
				return tris;
		}

		private static int[] MakeEndTris (int endsOffset){
			return new int[]{endsOffset,endsOffset + 2,endsOffset + 1, 
				endsOffset + 1,endsOffset + 2,endsOffset + 3, 
				endsOffset + 4,endsOffset + 5,endsOffset + 6, 
				endsOffset + 6,endsOffset + 5,endsOffset + 7};
		}

		private static int[] MakeSide1Tris (int side1Offset, Vector3[] topVerts){
			int[] side1 = new int[(topVerts.Length - 2) * 3];
			for (int i = 0; i < (topVerts.Length-2)/2; i++) {
				side1 [6 * i] = side1Offset + 2 * i;
				side1 [6 * i + 1] = side1Offset + 2 * i + 2;
				side1 [6 * i + 2] = side1Offset + 2 * i + 1;
				side1 [6 * i + 3] = side1Offset + 2 * i + 1;
				side1 [6 * i + 4] = side1Offset + 2 * i + 2;
				side1 [6 * i + 5] = side1Offset + 2 * i + 3;
			}
			return side1;
		}

		private static int[] MakeSide2Tris (int side2Offset, Vector3[] topVerts){
			int[] side2 = new int[(topVerts.Length - 2) * 3];
			for (int i = 0; i < (topVerts.Length-2)/2; i++) {
				side2 [6 * i] = side2Offset + 2 * i;
				side2 [6 * i + 1] = side2Offset + 2 * i + 1;
				side2 [6 * i + 2] = side2Offset + 2 * i + 2;
				side2 [6 * i + 3] = side2Offset + 2 * i + 2;
				side2 [6 * i + 4] = side2Offset + 2 * i + 1;
				side2 [6 * i + 5] = side2Offset + 2 * i + 3;
			}
			return side2;
		}
}
