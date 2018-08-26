using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectOptionsGenerator : MonoBehaviour {
	public GameObject PrefabForPlayerConfig;
	public GameObject targetObjectForDisplay;
	public int numberOfPlayers = 2;

	// Use this for initialization
	void Start () {
		for (int i = 1; i < numberOfPlayers; i++){
			GameObject.Instantiate(PrefabForPlayerConfig, new Vector3(0,0,0),Quaternion.identity).transform.SetParent(targetObjectForDisplay.transform);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
