using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshFilter))]
public class PlaneBender : MonoBehaviour {

    public Transform m_SphereCenter;
    public float m_Radius = 5f;

    [SerializeField]
    float length = 1f;
    [SerializeField]
    float width = 1f;
    [SerializeField]
    int resX = 2; // 2 minimum
    [SerializeField]
    int resZ = 2;

    private MeshFilter m_MeshFilter;
    private MeshRenderer m_MeshRenderer;
    private Mesh m_Mesh;
    private Vector3[] m_Vertices;

   // Use this for initialization
   void Start () {
        GeneratePlaneMesh();
        ProjectSpherical();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    private void GeneratePlaneMesh()
    {
        m_MeshFilter = GetComponent<MeshFilter>();
        if(m_MeshFilter == null)
            m_MeshFilter = gameObject.AddComponent<MeshFilter>();

        m_Mesh = m_MeshFilter.mesh;
        m_Mesh.Clear();

        #region Vertices		
        m_Vertices = new Vector3[resX * resZ];
        for (int z = 0; z < resZ; z++)
        {
            // [ -length / 2, length / 2 ]
            float zPos = ((float)z / (resZ - 1) - .5f) * length;
            for (int x = 0; x < resX; x++)
            {
                // [ -width / 2, width / 2 ]
                float xPos = ((float)x / (resX - 1) - .5f) * width;
                m_Vertices[x + z * resX] = new Vector3(xPos, zPos, 0f);
            }
        }
        #endregion

        #region Normales
        Vector3[] normales = new Vector3[m_Vertices.Length];
        for (int n = 0; n < normales.Length; n++)
            normales[n] = Vector3.up;
        #endregion

        #region UVs		
        Vector2[] uvs = new Vector2[m_Vertices.Length];
        for (int v = 0; v < resZ; v++)
        {
            for (int u = 0; u < resX; u++)
            {
                uvs[u + v * resX] = new Vector2((float)u / (resX - 1), (float)v / (resZ - 1));
            }
        }
        #endregion

        #region Triangles
        int nbFaces = (resX - 1) * (resZ - 1);
        int[] triangles = new int[nbFaces * 6];
        int t = 0;
        for (int face = 0; face < nbFaces; face++)
        {
            // Retrieve lower left corner from face ind
            int i = face % (resX - 1) + (face / (resZ - 1) * resX);

            triangles[t++] = i + resX;
            triangles[t++] = i + 1;
            triangles[t++] = i;

            triangles[t++] = i + resX;
            triangles[t++] = i + resX + 1;
            triangles[t++] = i + 1;
        }
        #endregion

        m_Mesh.vertices = m_Vertices;
        m_Mesh.normals = normales;
        m_Mesh.uv = uvs;
        m_Mesh.triangles = triangles;

        m_Mesh.RecalculateBounds();
        m_Mesh.Optimize();

    }

    private void ProjectSpherical()
    {
        for(int i=0;i<m_Vertices.Length;++i)
        {
            // get the radian angle of the vertex
            float rad = Mathf.Atan2(m_Vertices[i].z, m_Vertices[i].x);
            float cosdist = (Mathf.Cos(rad));
            float sindist = (Mathf.Sin(rad));

            //PROJECTED TO ALL THE POINTS RELATIVE TO THE OUTER RADIUS AND OFFSET TO THE CENTRE OF THE MESH 
            Vector3 projectedPos = new Vector3();
            projectedPos.x = cosdist * 10f;
            projectedPos.y = m_Vertices[i].y;
            projectedPos.z = sindist * 10f;

            m_Vertices[i] = projectedPos;

            //uvs[u] = new Vector2(cosdist * 0.5f, sindist * 0.5f) * vertices[u].magnitude / OuterRadius + Vector2.one * 0.5f; u++;

        }

        m_Mesh.vertices = m_Vertices;
    }
}
