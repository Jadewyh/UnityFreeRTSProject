using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Fraction
{
    Axis = -1,
    Germany = 1,
    Italy = 2,
    Japan = 3,
    Allies = -2,
    USA = 4,
    UK = 5,
    France = 6,
    Soviet = 7,
    Others = 98,
    Nature = 99
}
public enum DiplomacyStatus
{
    AttackAllowed = 0,
    AttackPossible = 1,
    AttackOnSight = 2,
    AttackImpossible = 99
}

public class PlayerHandler : MonoBehaviour
{
    public string playerName = "undefined";
    public Color playerColor = Color.white;
    public Fraction playerFraction = Fraction.Nature;
    public int currentResources = 1000;
    public int currentRadars;
    public bool isFrontendPlayer = false;
    public List<GameObject> playerOwnedObjects;
    public Dictionary<PlayerHandler, DiplomacyStatus> aggressionMatrix;
    public List<GameObject> playerInitialSpawnObjects;
    public Transform playerObjectSpawnParent;

    [ReadOnly] public GameManager gameManagerInstance;
    [ReadOnly] public string gameObjectSpawnParentName;
    [ReadOnly] public Material playerMaterial;


    // Use this for initialization
    void Start()
    {  

    }

    public void SpawnPlayer(){
        playerOwnedObjects = new List<GameObject>();
        aggressionMatrix = new Dictionary<PlayerHandler, DiplomacyStatus>();
        gameManagerInstance = GameManager.myInstance;
        if (!gameManagerInstance) gameManagerInstance = GetComponentInParent<GameManager>();
        if (!gameManagerInstance) gameManagerInstance = GetComponentInChildren<GameManager>();
        gameObjectSpawnParentName = "PlayerHandler_" + playerName + "_ObjectParent";
        PlayerHandler[] ph = gameManagerInstance.getAllPlayers();

        if (ph.Length > 0)
        {
            foreach (PlayerHandler p in ph)
            {
                aggressionMatrix.Add(p, DiplomacyStatus.AttackPossible);
            }
        }
        gameManagerInstance.OnPlayerJoin += joinEvent;

        if (!playerObjectSpawnParent)
        {
            GameObject go = GameObject.Find(gameObjectSpawnParentName);
            playerObjectSpawnParent = go ? go.transform : null;
            if (!playerObjectSpawnParent)
            {
                go = new GameObject(gameObjectSpawnParentName);
                go.transform.SetParent(gameManagerInstance.newUnitHierarchyParentObject.transform);
                playerObjectSpawnParent = go ? go.transform : null;
                if (!playerObjectSpawnParent)
                    Debug.Log("Could not generate Parent...");
            }
        }
    }

    void joinEvent(PlayerHandler p)
    {
        aggressionMatrix.Add(p, DiplomacyStatus.AttackPossible);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
