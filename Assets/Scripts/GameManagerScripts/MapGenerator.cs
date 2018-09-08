using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
    public Vector3 terrainSize;

    public Texture2D heightMap;

    public Texture2D[] terrainTextures;

    public float globScale;
    public GenerationParameters[] impacts;
    public GameObject TerrainObject;
    public bool autoUpdateInUnity;
    public bool showUnityHelpObjects;
    public bool useTestTerrain;
    public GameObject spawnBase;

    public GameObject CanvasDisplay;
    public GameObject CanvasRawImageObject;

    void Start()
    {

    }

    public void CleanUp()
    {
        if (!TerrainObject)
            TerrainObject = new GameObject("TerrainObj");
        if (!CanvasDisplay)
            CanvasDisplay = GameObject.Find("CanvasObject");

        if (CanvasDisplay)
            foreach (RawImage o in CanvasDisplay.GetComponentsInChildren<RawImage>())
                GameObject.DestroyImmediate(o.gameObject);

        GameObject.DestroyImmediate(TerrainObject.GetComponent<TerrainCollider>());
        GameObject.DestroyImmediate(TerrainObject.GetComponent<Terrain>());

    }

    public void generateTerrainObject(bool genMesh = false)
    {
        CleanUp();

        // Creation of objects
        TerrainData _TerrainData = new TerrainData();
        TerrainCollider _TerrainCollider = TerrainObject.AddComponent<TerrainCollider>();
        Terrain _Terrain2 = TerrainObject.AddComponent<Terrain>();

        _TerrainData.size = terrainSize;
        _TerrainData.heightmapResolution = (int)terrainSize.x + 1;
        _TerrainData.alphamapResolution = _TerrainData.heightmapHeight;
        _TerrainData.baseMapResolution = 1024;
        _TerrainData.SetDetailResolution(1024, 16);

        int _heightmapWidth = _TerrainData.heightmapWidth;
        int _heightmapHeight = _TerrainData.heightmapHeight;
        float[,] hmap = new float[_heightmapHeight, _heightmapWidth];

        // Materials
        SplatPrototype[] mats = new SplatPrototype[terrainTextures.Length];
        Texture2D[] maps = new Texture2D[terrainTextures.Length];
        int[] mapsAmount = new int[terrainTextures.Length];

        for (int i = 0; i < terrainTextures.Length; i++)
        {
            mats[i] = new SplatPrototype();
            mats[i].texture = terrainTextures[i];
            if (showUnityHelpObjects)
                maps[i] = new Texture2D(_TerrainData.alphamapWidth, _TerrainData.alphamapHeight);
        }
        _TerrainData.splatPrototypes = mats;

        // Randoms
        Vector2[] roundArray = new Vector2[impacts.Length];
        for (int i = 0; i < impacts.Length; i++)
        {
            roundArray[i] = new Vector3(Random.Range(impacts[i].lowBounds, impacts[i].highBounds), Random.Range(impacts[i].lowBounds, impacts[i].highBounds), (float)Random.Range(-1000, 1000) / 1000f);
        }

        float[,,] amap = new float[_TerrainData.alphamapHeight, _TerrainData.alphamapWidth, mats.Length];

        // Reset Alphamap / texturing
        for (int i = 0; i < _TerrainData.alphamapHeight; i++)
            for (int j = 0; j < _TerrainData.alphamapWidth; j++)
            {
                amap[i, j, 0] = 1;
                mapsAmount[0]++;
                for (int k = 1; k < mats.Length; k++)
                    amap[i, j, k] = 0;
            }

        //Debug.Log(string.Format("Alphamap: {0} {1} | Heightmap: {2} {3}", _TerrainData.alphamapHeight, _TerrainData.alphamapWidth, _heightmapWidth, _heightmapHeight));
        _TerrainData.SetAlphamaps(0, 0, amap);

        float lowestPoint = float.MaxValue;
        float highestPoint = float.MinValue;

        for (float i = 0; i < _heightmapHeight; i++)
        {
            for (float j = 0; j < _heightmapWidth; j++)
            {
                float height = 0f;
                if (useTestTerrain)
                {
                    height = Mathf.Lerp(0, 1, Mathf.Sin(i) + 1);
                }
                else
                {
                    for (var k = 0; k < impacts.Length; k++)
                    {
                        float x = (float)i / (globScale * impacts[k].scaleX) + roundArray[k].x;
                        float y = (float)j / (globScale * impacts[k].scaleY) + roundArray[k].y;
                        float perVal = Mathf.PerlinNoise(x, y);
                        height = height + (perVal / impacts[k].impact);
                    }
                }

                if (height < lowestPoint)
                    lowestPoint = height;
                if (height > highestPoint)
                    highestPoint = height;

                hmap[(int)i, (int)j] = height;

            }
        }

        if (showUnityHelpObjects)
            heightMap = new Texture2D(_heightmapWidth, _heightmapHeight);

        for (int i = 0; i < _heightmapHeight; i++)
            for (int j = 0; j < _heightmapWidth; j++)
            {
                //normalizing
                hmap[i, j] = Mathf.InverseLerp(lowestPoint, highestPoint, hmap[i, j]);
                if (showUnityHelpObjects)
                    heightMap.SetPixel(i, j, Color.Lerp(Color.black, Color.white, hmap[i, j]));
            }

        _TerrainData.SetHeights(0, 0, hmap);


        lowestPoint = float.MaxValue;
        highestPoint = float.MinValue;

        float[] realHeightMap = new float[_TerrainData.alphamapHeight * _TerrainData.alphamapHeight];
        for (var i = 0; i < _TerrainData.alphamapHeight; i++)
        {
            for (var j = 0; j < _TerrainData.alphamapWidth; j++)
            {
                realHeightMap[j * _TerrainData.alphamapHeight + i] = _TerrainData.GetHeight(j, i);
                if (realHeightMap[j * _TerrainData.alphamapHeight + i] < lowestPoint)
                    lowestPoint = realHeightMap[j * _TerrainData.alphamapHeight + i];
                if (realHeightMap[j * _TerrainData.alphamapHeight + i] > highestPoint)
                    highestPoint = realHeightMap[j * _TerrainData.alphamapHeight + i];
            }
        }

        for (var i = 0; i < _TerrainData.alphamapHeight; i++)
        {
            for (var j = 0; j < _TerrainData.alphamapWidth; j++)
            {
                for (var k = 0; k < impacts.Length; k++)
                {
                    float val = (realHeightMap[j * _TerrainData.alphamapHeight + i] / (highestPoint)) * 100;
                    if (val >= impacts[k].cutOffPointLower && val <= impacts[k].cutOffPointUpper)
                    {
                        amap[(int)i, (int)j, impacts[k].textureArrayIndex] = 1f;
                        amap[(int)i, (int)j, 0] = 0f;
                        if (showUnityHelpObjects)
                        {
                            mapsAmount[impacts[k].textureArrayIndex]++;
                            mapsAmount[0]--;
                            maps[impacts[k].textureArrayIndex].SetPixel(i, j, Color.Lerp(Color.black, Color.white, amap[i, j, impacts[k].textureArrayIndex]));
                        }

                    }
                }
            }
        }

        _TerrainData.SetAlphamaps(0, 0, amap);
        _TerrainCollider.terrainData = _TerrainData;
        _Terrain2.terrainData = _TerrainData;
        _Terrain2.gameObject.layer = 10;

        if (showUnityHelpObjects)
        {
            GameObject or;
            for (var k = 0; k < terrainTextures.Length; k++)
            {
                or = GameObject.Instantiate(CanvasRawImageObject);
                or.GetComponent<RawImage>().texture = maps[k];
                or.GetComponentInChildren<Text>().text = "Tex #" + k + " ( " + (((float)mapsAmount[k] / ((float)_TerrainData.alphamapHeight * (float)_TerrainData.alphamapWidth)) * 100f).ToString("F2") + "% ; " + mapsAmount[k] + ")";
                or.transform.SetParent(CanvasDisplay.transform);
                maps[impacts[k].textureArrayIndex].Apply();
            }

            or = GameObject.Instantiate(CanvasRawImageObject);
            or.GetComponent<RawImage>().texture = heightMap;
            or.GetComponentInChildren<Text>().text = "Height Map";
            or.transform.SetParent(CanvasDisplay.transform);
            heightMap.Apply();
        }

        if (genMesh)
            generateNavMesh();
    }

    public void gameStart()
    {
        generateNavMesh();

        GameManager myGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        // if (GameObject.Find("UnitFollower"))
        //     myGameManager.GUI.assignableFields.UnitFollowerCamera = GameObject.Find("UnitFollower").GetComponent<Camera>();

        // myGameManager.GUI.assignableFields._ResourceDisplay = GameObject.Find("CashStatus");
        // myGameManager.initManager();

        //myGameManager.GUI.assignableFields.TerrainRestriction = new Vector3(TerrainObject.GetComponent<Terrain>().terrainData.size.x, TerrainObject.GetComponent<Terrain>().terrainData.size.y, TerrainObject.GetComponent<Terrain>().terrainData.size.z);

        if (spawnBase)
        {
            GameObject allMyBase = GameObject.Instantiate(spawnBase, new Vector3(terrainSize.x / 2, terrainSize.y + 10f, terrainSize.z / 2), Quaternion.identity);
            allMyBase.transform.parent = myGameManager.transform;
        }

    }
    static Vector3 Quantize(Vector3 v, Vector3 quant)
    {
        float x = quant.x * Mathf.Floor(v.x / quant.x);
        float y = quant.y * Mathf.Floor(v.y / quant.y);
        float z = quant.z * Mathf.Floor(v.z / quant.z);
        return new Vector3(x, y, z);
    }

    public void generateNavMesh()
    {
        NavMeshSurface nms = TerrainObject.GetComponent<NavMeshSurface>();
        if (nms == null)
        {
            TerrainObject.AddComponent(typeof(NavMeshSurface));
            nms = TerrainObject.GetComponent<NavMeshSurface>();
        }
        nms.layerMask = LayerMask.GetMask(new string[]{"Terrain"});
        nms.BuildNavMesh();
    }

    [System.Serializable]
    public class GenerationParameters
    {
        public float impact;
        public int lowBounds;

        public int highBounds;

        public float cutOffPointLower;

        public float cutOffPointUpper;

        public float scaleX;
        public float scaleY;

        public int textureArrayIndex;
    }

}
