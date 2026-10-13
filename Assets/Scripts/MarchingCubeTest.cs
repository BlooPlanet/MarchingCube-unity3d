using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using MarchingCubes;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class MarchingCubeTest : MonoBehaviour {

    int width = 20, height = 20, depth = 20;
    
    MarchingCube marching;
    MeshFilter mFilter;
    
    // Start is called before the first frame update
    void Start() {
        marching = new MarchingCube(width,height,depth,-1);
        marching.RandomGeneratePoints();

        mFilter = GetComponent<MeshFilter>();
        Mesh mesh = marching.ContructMesh();
        mFilter.mesh = mesh;
        
    }

    void OnDrawGizmos() {
        if (Application.isPlaying) {
            // for (int x = 0; x < width; x++) {
            //     for (int z = 0; z < depth; z++) {
            //         for (int y = 0; y < height; y++) {
            //             Vector3Int cubeCoord = new Vector3Int(x, y, z);
            //             for (int i = 0; i < MarchingCubeConfigs.CornerTable.Length; i++) {
            //                 Vector3Int cubeCorner = MarchingCubeConfigs.CornerTable[i] + cubeCoord;
            //                 float density = (marching.GetPoint(cubeCorner) + 1) * 0.5f;
            //                 Gizmos.color = new Color(density, density, density, 1);
            //                 Gizmos.DrawSphere(cubeCorner,0.03f);
            //             }
            //         }
            //     }
            // }
            Vector3 size = new Vector3(width, height, depth);
            Gizmos.DrawWireCube(size / 2,size);
        }
        
    }
}
