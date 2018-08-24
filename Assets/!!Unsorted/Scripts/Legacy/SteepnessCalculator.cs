using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SteepnessCalculator : MonoBehaviour {

    public Terrain terrain;
    public float steepness;
    public GameObject front;
    public double hyp;
    private float NormalizedX;
    private float NormalizedY;


	// Use this for initialization
	void Start () {
        front = GameObject.Find("Front");
	}
	
	// Update is called once per frame
	void Update () {
        NormalizedX = transform.position.x / terrain.terrainData.size.x;
        NormalizedY = transform.position.z / terrain.terrainData.size.z;
        steepness = terrain.terrainData.GetSteepness(NormalizedX, NormalizedY);
        Debug.Log(steepness);
       

	}
}
