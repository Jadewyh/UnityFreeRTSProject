using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NameParts
{
    public Dictionary<int, List<string>> parts;
}

public class PlayerSelectOptionsGenerator : MonoBehaviour
{
    [System.Serializable]
    public enum UIDropDownSlot
    {
        Computer = 1,
        Free = 0,
        Closed = 2
    }
    [System.Serializable]
    public enum UIDropDownColor
    {
        Red = 0,
        Blue = 1,
        Green = 2,
        White = 3,
        Yellow = 4,
        Cyan = 5,
        Black = 6
    }
    [System.Serializable]
    public enum UIDropDownFraction
    {
        Axis = 0,
        Allies = 1,
        Soviet = 2
    }
    public int numberOfPlayers = 2;
    public GameObject PrefabForPlayerConfig;
    public GameObject targetObjectForDisplay;
    public GameObject playerConfig;
    [ReadOnly] public List<GameObject> additionalPlayerConfig;
    public List<GameObject> listOfSpawnObjects;
    public Dictionary<Fraction, NameParts> namePossibilities;

    // Use this for initialization
    void Start()
    {
        namePossibilities = new Dictionary<Fraction, NameParts>();
        namePossibilities.Add(Fraction.Allies, new NameParts());
        namePossibilities.Add(Fraction.Axis, new NameParts());
        namePossibilities.Add(Fraction.Soviet, new NameParts());
        //Fraction.Allies
        namePossibilities[Fraction.Allies].parts = new Dictionary<int, List<string>>
        {
            { 0, new List<string>(new string[] { "Franz","Josef","Hugo","Christian" }) },
            { 1, new List<string>(new string[] { "Müller","Mustermann","Mayer","Meyer" }) }
        };
        namePossibilities[Fraction.Axis].parts = new Dictionary<int, List<string>>
        {
            { 0, new List<string>(new string[] { "Gary","Grey","Vicent","Sebastian" }) },
            { 1, new List<string>(new string[] { "King","Holy","Moyes","Trump" }) }
        };

        namePossibilities[Fraction.Soviet].parts = new Dictionary<int, List<string>>
        {
            { 0, new List<string>(new string[] { "Elisabeth","Yana","Yulia","June" }) },
            { 1, new List<string>(new string[] { "Matroyzka","Vodka","Da","Okunoya" }) }
        };
        playerConfig = this.GetComponentInChildren<UnityEngine.UI.InputField>().gameObject.transform.parent.gameObject;
        for (int i = 1; i < numberOfPlayers; i++)
        {
            GameObject pc = GameObject.Instantiate(PrefabForPlayerConfig, new Vector3(0, 0, 0), Quaternion.identity);
            pc.transform.SetParent(targetObjectForDisplay.transform);
            pc.GetComponentInChildren<UnityEngine.UI.Dropdown>().value = 2;
            additionalPlayerConfig.Add(pc);
        }
    }

    public static Fraction convertUIFractionToFraction(int uiFraction)
    {
        Fraction playerFraction = Fraction.Others;
        switch (uiFraction)
        {
            case (int)UIDropDownFraction.Allies:
                playerFraction = Fraction.Allies;
                break;
            case (int)UIDropDownFraction.Axis:
                playerFraction = Fraction.Axis;
                break;
            case (int)UIDropDownFraction.Soviet:
                playerFraction = Fraction.Soviet;
                break;
        }
        return playerFraction;
    }

    public static Color convertUIColorToColor(int uiColor)
    {
        Color playerColor = Color.cyan;
        switch (uiColor)
        {
            case (int)UIDropDownColor.Black:
                playerColor = Color.black;
                break;
            case (int)UIDropDownColor.Blue:
                playerColor = Color.blue;
                break;
            case (int)UIDropDownColor.Cyan:
                playerColor = Color.cyan;
                break;
            case (int)UIDropDownColor.Green:
                playerColor = Color.green;
                break;
            case (int)UIDropDownColor.Red:
                playerColor = Color.red;
                break;
            case (int)UIDropDownColor.White:
                playerColor = Color.white;
                break;
            case (int)UIDropDownColor.Yellow:
                playerColor = Color.yellow;
                break;
        }
        return playerColor;
    }

    PlayerHandler gameObject2Player(GameObject pc)
    {

        bool usePlayer = false;

        PlayerHandler p = null;

        // is Player?
        if (pc.GetComponentInChildren<UnityEngine.UI.InputField>() != null)
        {
            usePlayer = true;
            p = new GameObject("PlayerHandlerObject").AddComponent<PlayerHandler>();
            p.playerName = pc.GetComponentInChildren<UnityEngine.UI.InputField>().text;
            p.isFrontendPlayer = true;
        }

        // npc?
        foreach (UnityEngine.UI.Dropdown d in pc.GetComponentsInChildren<UnityEngine.UI.Dropdown>())
        {
            if (d.gameObject.name == "SlotDropDown" && d.value == (int)UIDropDownSlot.Computer)
            {
                usePlayer = true;
                p = new GameObject().AddComponent<PlayerHandler>();
            }
            if (d.gameObject.name == "ColorDropDown")
            {
                p.playerColor = convertUIColorToColor(d.value);
                p.playerMaterial = PlayerColorMaterialDispencer.GetMaterialFromColorIndex(d.value);
            }
            if (d.gameObject.name == "FractionDropDown")
            {
                p.playerFraction = convertUIFractionToFraction(d.value);
                p.playerInitialSpawnObjects = new List<GameObject>();
                p.playerInitialSpawnObjects.AddRange(listOfSpawnObjects.ToArray());
            }

        }
        if (usePlayer)
        {
            if (p.playerName == "" && p.isFrontendPlayer != true)
            {
                string fullName = "";

                foreach (int r in namePossibilities[p.playerFraction].parts.Keys)
                {
                    int rdIdx = namePossibilities[p.playerFraction].parts[r].Count;
                    rdIdx = UnityEngine.Random.Range(0, rdIdx);
                    fullName = fullName == ""
                        ? namePossibilities[p.playerFraction].parts[r][rdIdx]
                        : fullName + " " + namePossibilities[p.playerFraction].parts[r][rdIdx];
                }

                p.playerName = fullName;
            }
            p.SpawnPlayer();
            return p;
        }
        else
            return null;
    }

    public void convertPlayerSelectsToPlayers()
    {
        PlayerHandler p;
        foreach (GameObject pc in additionalPlayerConfig)
        {
            p = gameObject2Player(pc);
            if (p != null)
                GameManager.myInstance.addNewPlayer(p);

        }

        p = gameObject2Player(playerConfig);
        if (p != null)
            GameManager.myInstance.addNewPlayer(p);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
