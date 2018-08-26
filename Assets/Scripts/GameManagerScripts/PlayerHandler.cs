using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Fraction {
	Axis = 0,
	Allies = 1,
	Germany = 2,
	Others = 3,
	Nature = 99
}
public enum DiplomacyStatus {
	AttackAllowed = 0,
	AttackPossible = 1,
	AttackOnSight = 2,
	AttackImpossible = 99
}

public class PlayerHandler : MonoBehaviour {
	public string playerName = "undefined";
	public Color playerColor = Color.white;
	public Fraction playerFraction = Fraction.Nature;
	public int currentResources = 1000;
	public int currentRadars;
	public bool isFrontendPlayer = false;
	public List<GameObject> playerOwnedObjects;
	public Dictionary<PlayerHandler,DiplomacyStatus> aggressionMatrix;
	public List<GameObject> playerInitialSpawnObjects;
	public Transform playerObjectSpawnParent;

	[ReadOnly] public GameManager gameManagerInstance;
	[ReadOnly] public string gameObjectSpawnParentName;
	

	// Use this for initialization
	void Start () {
		playerOwnedObjects = new List<GameObject>();
		aggressionMatrix = new Dictionary<PlayerHandler, DiplomacyStatus>();
		gameManagerInstance = GameManager.myInstance;
		gameObjectSpawnParentName = "PlayerHandler_"+playerName+"_ObjectParent";

		foreach(PlayerHandler p in gameManagerInstance.getAllPlayers()){
			aggressionMatrix.Add(p,DiplomacyStatus.AttackPossible);
		}
		gameManagerInstance.onPlayerJoin += joinEvent;

		if (!playerObjectSpawnParent) {
			GameObject go = GameObject.Find(gameObjectSpawnParentName);
			playerObjectSpawnParent = go?go.transform:null;
			if (!playerObjectSpawnParent){
				go = new GameObject(gameObjectSpawnParentName);
				go.transform.SetParent(gameManagerInstance.newUnitHierarchyParentObject.transform);
				playerObjectSpawnParent = go?go.transform:null;
			}
		}
	}
	
	void joinEvent(PlayerHandler p){
		aggressionMatrix.Add(p,DiplomacyStatus.AttackPossible);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
