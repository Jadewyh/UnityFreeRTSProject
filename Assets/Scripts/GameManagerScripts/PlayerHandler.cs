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
                go.transform.SetParent(gameManagerInstance.gameObject.transform);
                playerObjectSpawnParent = go ? go.transform : null;
                if (!playerObjectSpawnParent)
                    Debug.Log("Could not generate Parent...");
            }
        }
    }

    public void spawnObject(GameObject prefab, Vector3 spawningPoint)
    {
        GameObject obj = GameObject.Instantiate(prefab, spawningPoint, Quaternion.identity);
        if (!obj)
        {
            Debug.LogError("Instantiating of object " + prefab.name + " failed!");
            return;
        }
        if (!playerObjectSpawnParent)
        {
            Debug.LogError("SpawnParent of player " + playerName + " not set!");
            return;
        }
        obj.transform.parent = playerObjectSpawnParent.transform;
        foreach (GenericUnit u in obj.GetComponentsInChildren<GenericUnit>())
        {
            u.owner = this;
        }
        foreach (MeshRenderer m in obj.GetComponentsInChildren<MeshRenderer>())
        {
            m.materials = new Material[] { playerMaterial };

        }
    }

    void joinEvent(PlayerHandler p)
    {
        aggressionMatrix.Add(p, DiplomacyStatus.AttackPossible);
        Debug.Log("Player " + playerName + " added a new Matrix Element for new player " + p.playerName);
    }

}
