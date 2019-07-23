using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public GameObject terrainPiecePrefab;

    public List<MeshCreator> meshList = new List<MeshCreator>();
    public MeshCreator lastMesh = null;
    public bool spawnMesh = false;

    public float xPos = 0;

    public MeshProperty meshProperty;

    [Header("Number Of Mesh Pieces")]
    public int meshCount = 20;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        System.Random random = new System.Random();
        MeshCreator.seed = random.Next(-100000, 100000);

        for (int i = 0; i < meshCount; i++)
        {
            SpawnTerrainPiece();
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(spawnMesh)
        {
            spawnMesh = false;
            SpawnTerrainPiece();
        }
    }

    public void SpawnTerrainPiece()
    {
        GameObject terrainObject;
        MeshCreator meshCreator;
        if (meshList.Count == 0)
        {
            terrainObject = Instantiate(terrainPiecePrefab, transform.position, Quaternion.identity);
            meshCreator = terrainObject.GetComponent<MeshCreator>();
        }

        else
        {
            xPos += lastMesh.xValue;
            terrainObject = Instantiate(terrainPiecePrefab, new Vector3(xPos, transform.position.y, transform.position.z), Quaternion.identity);
            meshCreator = terrainObject.GetComponent<MeshCreator>();
        }

        //Set Mesh Properties
        meshCreator.vertexCount = meshProperty.vertexCount;
        meshCreator.bottomPoint = meshProperty.bottomPoint;
        meshCreator.Xspacing = meshProperty.Xspacing;
        meshCreator.Yspacing = meshProperty.Yspacing;
        meshCreator.verticalVertexCount = meshProperty.verticalVertexCount;
        meshCreator.scale = meshProperty.scale;
        meshCreator.height = meshProperty.height;
        meshCreator.xValue = meshProperty.xValue;
        meshCreator.GetComponent<MeshRenderer>().material = meshProperty.material;

        meshList.Add(meshCreator);
        lastMesh = meshCreator;
    }
}

[System.Serializable]
public class MeshProperty
{
    public int vertexCount;
    public int bottomPoint;
    public float Xspacing;
    public float Yspacing;
    public int verticalVertexCount = 2;
    public float scale;
    public float height;
    public float xValue;
    public Material material;
}
