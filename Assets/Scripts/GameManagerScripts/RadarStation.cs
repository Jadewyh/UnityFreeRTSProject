using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarStation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		GameManager.myInstance.PlayerRadarStations++;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private void OnDestroy() {
		GameManager.myInstance.PlayerRadarStations--;
	}
}
