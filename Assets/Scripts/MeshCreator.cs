using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshCreator : MonoBehaviour
{
    public int vertexCount;
    public float bottomPoint;
    public float Xspacing;
    public float Yspacing;
    public int verticalVertexCount = 2;

    public float scale = .1f;
    public static float seed;
    public float height;

    private int currentVerticalVertexCount;
    public float xValue;
    [SerializeField] private List<Vector3> vertexList = new List<Vector3>();
    [SerializeField] private List<int> triangleList = new List<int>();
    EdgeCollider2D edgeCollider2D;

    private Mesh myMesh;

    // Start is called before the first frame update
    void Start()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.sharedMesh = CreateMesh();
        myMesh = meshFilter.sharedMesh;
        edgeCollider2D = gameObject.AddComponent<EdgeCollider2D>();

        //yield return new WaitForSecondsRealtime(2f);
        ManipulateMesh(myMesh);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();

        xValue = bottomPoint;
        int[] trianglesArray;

        for (int i = 0; i < vertexCount; i++)
        {
            if(currentVerticalVertexCount == verticalVertexCount)
            {
                currentVerticalVertexCount = 0;
                xValue += Xspacing;
            }

            Vector3 vertex;
            vertex = new Vector3(xValue, currentVerticalVertexCount * Yspacing, 0);
            vertexList.Add(vertex);

            currentVerticalVertexCount++;
        }

        trianglesArray = new int[vertexList.Count - 2];

        int triangleVertex = 3;
        int breakCount = 0;

        for (int i = 0; i < vertexList.Count; i++)
        {
            triangleList.Add(i);
            triangleVertex--;

            if (triangleVertex == 0)
            {
                triangleList.Add(i + 1);
                triangleList.Add(i);
                triangleList.Add(i - 1);

                if((i + 3) > vertexList.Count)
                {
                    break;
                }

                i--;

                triangleVertex = 3;
            }
        }

        trianglesArray = triangleList.ToArray();
        mesh.SetVertices(vertexList);
        mesh.SetTriangles(trianglesArray, 0);
        mesh.SetUVs(0, vertexList);

        mesh.RecalculateNormals();

        return mesh;
    }

    private void ManipulateMesh(Mesh mesh)
    {
        Vector3[] vertArray = mesh.vertices;
        List<Vector2> edgeColliderPoints = new List<Vector2>();

        for (int i = 0; i < vertArray.Length; i++)
        {
            if(vertArray[i].y != bottomPoint)
            {
                float x = ((vertArray[i].x + transform.position.x) / scale) + seed;
                float y = (vertArray[i].y / scale) + seed;

                float newYValue = Mathf.PerlinNoise(x, y) * height;
                Debug.Log(string.Format("New Y Value for Index {0},{1} with Perlin Index (X):{2} (Y):{3} is {4}", vertArray[i].x, vertArray[i].y, x, y, newYValue));
                vertArray[i].y = newYValue;
                edgeColliderPoints.Add(new Vector2(vertArray[i].x, newYValue));
            }
        }

        mesh.SetVertices(vertArray.ToList());
        mesh.RecalculateNormals();

        edgeCollider2D.points = edgeColliderPoints.ToArray();

        Debug.Log("=============================================================");
    }
}
