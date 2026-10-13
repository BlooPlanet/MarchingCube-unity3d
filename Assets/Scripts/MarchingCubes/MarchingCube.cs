using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

namespace MarchingCubes {
    public class MarchingCube {
        public float surfaceLevel = 0;
        public int width, height, depth;
        public float[] points;

        public MarchingCube(int width, int height, int depth,float surfaceLevel) {
            this.width = width;
            this.height = height;
            this.depth = depth;
            this.surfaceLevel = surfaceLevel;
            int totallPoints = (width + 1) * (height + 1) * (depth + 1);
            points = new float[totallPoints];
        }

        int GetIndex(Vector3Int pointCoordinate) {
            int x = pointCoordinate.x;
            int y = pointCoordinate.y;
            int z = pointCoordinate.z;
            return x + y * (width + 1) + z * (width + 1) * (height + 1);
        }
        
        public void SetPoint(Vector3Int pointCoordinate, float density) {
            int index = GetIndex(pointCoordinate);
            points[index] = density;
        }

        public float GetPoint(Vector3Int pointCoordinate) {
            return points[GetIndex(pointCoordinate)];
        }

        public int GetCubeConfigIndex(Vector3Int cubeCoordinate) {
            int configIndex = 0;
            for (int i = 0; i < MarchingCubeConfigs.CornerTable.Length; i++) {
                Vector3Int corner = MarchingCubeConfigs.CornerTable[i] + cubeCoordinate;
                float pointDensity = GetPoint(corner);
                if (pointDensity > surfaceLevel) {
                    configIndex |= 1 << i;
                }
            }
            return configIndex;
        }

        public void March(Vector3Int cubeCoord,int configIndex,List<Vector3> vertices, List<int> triangles) {
            if (configIndex == 0 || configIndex == 255) return;

            for (int i = 0; i < 15; i++) {
                int edgeIndex = MarchingCubeConfigs.TriangleTable[configIndex,i];
                if (edgeIndex == -1) break;
                Vector3 cornaerA = MarchingCubeConfigs.EdgeTable[edgeIndex, 0];
                Vector3 cornaerB = MarchingCubeConfigs.EdgeTable[edgeIndex, 1];
                Vector3 midPoint = (cornaerA + cornaerB) * 0.5f;
                
                vertices.Add(midPoint + cubeCoord);
                triangles.Add(vertices.Count - 1);
            }
        }

        public Mesh ContructMesh() {
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            Mesh mesh = new Mesh();

            for (int x = 0; x < width; x++) {
                for (int z = 0; z < depth; z++) {
                    for (int y = 0; y < height; y++) {
                        Vector3Int cubeCoord = new Vector3Int(x, y, z);
                        int configIndex = GetCubeConfigIndex(cubeCoord);
                        if (configIndex == 0 || configIndex == 255) continue;
                        March(cubeCoord, configIndex, vertices, triangles);
                    }
                }
            }

            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.RecalculateTangents();
            
            return mesh;
        }

        public void RandomGeneratePoints() {
            for (int x = 0; x < width + 1; x++) {
                for (int z = 0; z < depth + 1; z++) {
                    int amplitude = (int)(Mathf.PerlinNoise(x / 30f, z / 30f) * height);
                    for (int y = 0; y < amplitude; y++) {
                        Vector3Int pointCoord = new Vector3Int(x, y, z);
                        SetPoint(pointCoord, -2);
                    }
                }
                
            }
        }
    }
}