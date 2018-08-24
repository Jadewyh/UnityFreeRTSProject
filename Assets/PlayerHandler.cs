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
	public Fraction playerFraction = Fraction.Nature;
	public int currentResources = 1000;
	public bool isFrontendPlayer = false;
	public Color playerColor = Color.white;
	public string playerName = "undefined";
	public List<GameObject> playerOwnedObjects;
	public Dictionary<PlayerHandler,DiplomacyStatus> aggressionMatrix;
	
	[ReadOnly] public GameManager gameManagerInstance;
	

	// Use this for initialization
	void Start () {
		playerOwnedObjects = new List<GameObject>();
		aggressionMatrix = new Dictionary<PlayerHandler, DiplomacyStatus>();
		gameManagerInstance = GameManager.myInstance;

		foreach(PlayerHandler p in gameManagerInstance.playersInGame){
			aggressionMatrix.Add(p,DiplomacyStatus.AttackPossible);
		}
		gameManagerInstance.onPlayerJoin += joinEvent;
	}
	
	void joinEvent(PlayerHandler p){
		aggressionMatrix.Add(p,DiplomacyStatus.AttackPossible);
	}

	// Update is called once per frame
	void Update () {
		
	}
}
