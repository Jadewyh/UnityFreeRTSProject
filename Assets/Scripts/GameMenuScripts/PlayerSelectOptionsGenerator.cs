using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIDropDownSlot
{
    Computer = 1,
    Free = 0,
    Closed = 2
}
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
public enum UIDropDownFraction
{
    Axis = 0,
    Allies = 1,
    Soviet = 2
}

public class NameParts
{
    public Dictionary<int, List<string>> parts;
}

public class PlayerSelectOptionsGenerator : MonoBehaviour
{
    public int numberOfPlayers = 2;
    public GameObject PrefabForPlayerConfig;
    public GameObject targetObjectForDisplay;
    public GameObject playerConfig;
    [ReadOnly] public List<GameObject> additionalPlayerConfig;
    public List<GameObject> listOfSpawnObjects;
    public Dictionary<Fraction, NameParts> namePossibilities;
    public List<Material> materials;
    private Dictionary<int, Material> matMap;


    // Use this for initialization
    void Start()
    {
        matMap = new Dictionary<int, Material>();
        foreach (Material m in materials)
        {
            int val = (int)System.Enum.Parse(typeof(UIDropDownColor), m.name, true);
            matMap.Add(val, m);
        }
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
                switch (d.value)
                {
                    case (int)UIDropDownColor.Black:
                        p.playerColor = Color.black;
                        break;
                    case (int)UIDropDownColor.Blue:
                        p.playerColor = Color.blue;
                        break;
                    case (int)UIDropDownColor.Cyan:
                        p.playerColor = Color.cyan;
                        break;
                    case (int)UIDropDownColor.Green:
                        p.playerColor = Color.green;
                        break;
                    case (int)UIDropDownColor.Red:
                        p.playerColor = Color.red;
                        break;
                    case (int)UIDropDownColor.White:
                        p.playerColor = Color.white;
                        break;
                    case (int)UIDropDownColor.Yellow:
                        p.playerColor = Color.yellow;
                        break;

                }
                p.playerMaterial = matMap[d.value];
            }
            if (d.gameObject.name == "FractionDropDown")
            {
                switch (d.value)
                {
                    case (int)UIDropDownFraction.Allies:
                        p.playerFraction = Fraction.Allies;
                        break;
                    case (int)UIDropDownFraction.Axis:
                        p.playerFraction = Fraction.Axis;
                        break;
                    case (int)UIDropDownFraction.Soviet:
                        p.playerFraction = Fraction.Soviet;
                        break;
                }
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
