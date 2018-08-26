using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
	[ReadOnly] public Vector3 TerrainRestriction;
	[ReadOnly] public MapGenerator mapGen;
	public void generateMap(){
		mapGen.generateTerrainObject();
	}

	public void generateAI(){
		mapGen.generateNavMesh();
	}

	// Use this for initialization
	void Start () {
		if (!mapGen){
			mapGen = GetComponent<MapGenerator>();
			if (!mapGen){
				mapGen = this.gameObject.AddComponent<MapGenerator>();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
