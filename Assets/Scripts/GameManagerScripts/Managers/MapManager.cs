using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [ReadOnly] public Vector3 TerrainRestriction;

    private MapGenerator _mapGen;

    public MapGenerator mapGen
    {
        get
        {
            if (!_mapGen)
            {
                _mapGen = GetComponent<MapGenerator>();
                if (!mapGen)
                {
                    _mapGen = this.gameObject.AddComponent<MapGenerator>();
                }
            }
            return _mapGen;
        }
        /*set {
            _mapGen = value;
        }*/
    }
    public void generateMap()
    {
        mapGen.generateTerrainObject();
        this.TerrainRestriction = mapGen.terrainSize;
    }

    public void generateAI()
    {
        mapGen.generateNavMesh();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
