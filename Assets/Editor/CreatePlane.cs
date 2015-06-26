using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreatePlane
{
    [MenuItem("GameObject/Create Other/Custom Plane")]
    public static void CreatePlaneMesh()
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        Vector2[] uvs = new Vector2[4];
        int[] triangleIndices = new int[6] { 0, 1, 2, 2, 1, 3 };

        vertices[0] = new Vector3(-0.5f, 0.5f, 0f);
        vertices[1] = new Vector3(0.5f, 0.5f, 0f);
        vertices[2] = new Vector3(-0.5f, -0.5f, 0f);
        vertices[3] = new Vector3(0.5f, -0.5f, 0f);

        uvs[0] = new Vector2(0f, 1f);
        uvs[1] = new Vector2(1f, 1f);
        uvs[2] = new Vector2(0f, 0f);
        uvs[3] = new Vector2(1f, 0f);

        mesh.vertices = vertices;
        mesh.triangles = triangleIndices;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GameObject customPlaneObject = new GameObject();
        MeshFilter planeMeshFilter = customPlaneObject.AddComponent<MeshFilter>();
        planeMeshFilter.mesh = mesh;

        customPlaneObject.AddComponent<MeshRenderer>();

        AssetDatabase.CreateAsset(mesh, "Assets/CustomPlane.asset");
    }
}
