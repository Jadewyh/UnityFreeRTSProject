using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public delegate void newPlayerJoined(PlayerHandler p);


public class GameManager : MonoBehaviour
{
    public static GameManager myInstance;

    public GameObject newUnitHierarchyParentObject;

    [ReadOnly] private List<PlayerHandler> playersInGame;
    [ReadOnly] public MapManager mapManagerInstance;
    [ReadOnly] public GUIManager guiManagerInstance;
    [ReadOnly] public PlayerHandler UIPlayer;
    public event newPlayerJoined OnPlayerJoin;
    public bool hasStarted = false;

    public void returnToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleMenu", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void startGame()
    {
        if (!mapManagerInstance.mapGen.TerrainObject) return;
        GameObject.Find("MapConfigScreen").GetComponentInChildren<PlayerSelectOptionsGenerator>().convertPlayerSelectsToPlayers();
        mapManagerInstance.generateAI();
        foreach (PlayerHandler p in playersInGame)
        {
            var spawningPoint = new Vector3(UnityEngine.Random.value * mapManagerInstance.mapGen.TerrainObject.GetComponent<Terrain>().terrainData.size.x,
                                           UnityEngine.Random.value * mapManagerInstance.mapGen.TerrainObject.GetComponent<Terrain>().terrainData.size.y,
                                            UnityEngine.Random.value * mapManagerInstance.mapGen.TerrainObject.GetComponent<Terrain>().terrainData.size.z);

            foreach (GameObject o in p.playerInitialSpawnObjects)
            {

                GameObject obj = GameObject.Instantiate(o, spawningPoint, Quaternion.identity);
                if (!obj)
                {
                    Debug.LogError("Instantiating of object " + o.name + " failed!");
                    return;
                }
                if (!p.playerObjectSpawnParent)
                {
                    Debug.LogError("SpawnParent of player " + p.playerName + " not set!");
                    return;
                }
                obj.transform.parent = p.playerObjectSpawnParent.transform;
                foreach (GenericUnit u in obj.GetComponentsInChildren<GenericUnit>())
                {
                    u.owner = p;
                }
                foreach (MeshRenderer m in obj.GetComponentsInChildren<MeshRenderer>())
                {
                    m.materials = new Material[] { p.playerMaterial };

                }
            }
        }
        GameObject.Find("MapConfigScreen").SetActive(false);
        mapManagerInstance.TerrainRestriction = mapManagerInstance.mapGen.terrainSize;
        hasStarted = true;

    }

    public void addNewPlayer(PlayerHandler p)
    {
        if (p.isFrontendPlayer && UIPlayer != null)
            Debug.LogError("There is already a front-end player defined! CONFLICT!");
        else if (p.isFrontendPlayer)
            UIPlayer = p;

        playersInGame.Add(p);
    }
    public PlayerHandler[] getAllPlayers()
    {
        if (playersInGame == null) return new PlayerHandler[0];
        return playersInGame.ToArray();
    }

    public int Resources
    {
        get
        {
            return UIPlayer.currentResources;
        }
        set
        {
            UIPlayer.currentResources = value;
            guiManagerInstance.UI_ResourceDisplayText.text = "$: " + UIPlayer.currentResources;
        }
    }

    public int PlayerRadarStations
    {
        get
        {
            if (!UIPlayer)
                return 0;
            return UIPlayer.currentRadars;
        }
        set
        {
            if (!UIPlayer) return;

            UIPlayer.currentRadars = value;
            if (UIPlayer.currentRadars <= 0)
            {
                guiManagerInstance.isMiniMapVisible = false;
            }
            else
            {
                guiManagerInstance.isMiniMapVisible = true;
            }
        }
    }

    /// <summary>
    /// Initializes GameManager
    /// </summary>
    // Use this for initialization
    void Start()
    {
        // Debug.Log("Following Bundles are loaded:");
        // foreach(AssetBundle b in AssetBundle.GetAllLoadedAssetBundles()){
        // Debug.Log(b.ToString());
        // }

        if (!myInstance)
            myInstance = this;

        mapManagerInstance = this.gameObject.GetComponentInChildren<MapManager>();
        guiManagerInstance = this.gameObject.GetComponentInChildren<GUIManager>();
        PlayerRadarStations = 0;
        guiManagerInstance.selectedUnit = null;
        playersInGame = new List<PlayerHandler>();
    }


    /// <summary>
    /// GameObject Update method (as often as possible)
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        UpdatePositionCalculations();
        //UpdateRTSCamera();
    }

    /// <summary>
    /// Calculate / Check position updates for selected unit(s)
    /// </summary>
    void UpdatePositionCalculations()
    {
        GenericUnit gu = null;
        // if a unit is selected
        if (guiManagerInstance.selectedUnit)
        {
            // make camera follow!
            Vector3 x = guiManagerInstance.selectedUnit.transform.position;
            x.y = x.y + 5f;
            x.z = x.z - 20f;
            guiManagerInstance.UI_UnitFollowerCamera.transform.position = x;
            gu = guiManagerInstance.selectedUnit.GetComponent<GenericUnit>();
        }

        // check Left Mouse Button
        if (Input.GetMouseButtonDown(0) && guiManagerInstance.selectedUnit)
        {
            //if (GUI._selectedUnitGeneric && GUI._selectedUnitGeneric.AudioSettings.moveQuotes.Length > 0)
            //playRandomQuote(GUI._selectedUnitGeneric.AudioSettings.moveQuotes, Audio.quoteAudioSource);

            if (gu && gu.UnitSettings.canMove)
            {
                calculateTargetPosition();
                gu.movementSpeed = gu.UnitSettings.maxMovementSpeed;
            }
        }
    }


    void drawMainCameraPositionOnMiniMap()
    {
        //GUI._unitFollowerProjectionObject;
        DrawRectangle(Camera.main.GetComponent<Camera>().rect, Color.black);

    }
    public static void DrawRectangle(Rect area, Color color)
    {
        // GameObject MiniMap = GameObject.Find("MiniMap");
        // RawImage MiniMapMap = MiniMap.GetComponent<RawImage>();
        // float screenHeightInUnits = Camera.main.orthographicSize * 2;
        // float screenWidthInUnits = screenHeightInUnits * Screen.width / Screen.height;
    }


    /// <summary>
    /// Calculate the target position of a selected unit
    /// </summary>
    void calculateTargetPosition()
    {
        if (!guiManagerInstance.selectedUnit) return;

        GenericUnit gu = guiManagerInstance.selectedUnit.GetComponent<GenericUnit>();

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            gu.moveTargetPoint.Set(hit.point.x, transform.position.y, hit.point.z);
            NavMeshDisplay d = GetComponentInChildren<NavMeshDisplay>();
            if (d != null)
            {
                d.target = gu.moveTargetPoint;
            }
            gu.isMoving = true;


        }
    }

    /// <summary>
    /// Spawns a Objects from a ConsturctionObject
    /// </summary>
    /// <param name="callee">Calling GenericConstructionObject</param>
    /// <param name="ignoreCosts">ignores the costs (for example via cinematic scene)</param>
    public void spawnObject(GenericConstructionObject callee, bool ignoreCosts = false, PlayerHandler p = null)
    {
        if (p == null) p = UIPlayer;
        if (p.currentResources - callee.resourceCost >= 0 || ignoreCosts)
        {
            GameObject o = GameObject.Instantiate(callee.CreationObject, guiManagerInstance.selectedUnit.transform.position, Quaternion.identity);
            o.transform.Translate(0, 0, -1 * (callee.transform.forward.z + 2f));
            configureGameObject(o, p);

            if (!ignoreCosts)
                this.Resources = this.Resources - (int)callee.resourceCost;
        }
        else
        {
            // not enough resources
            //Audio.infoAudioSource.PlayOneShot(Audio.AudioQueueStatus.notEnoughResources);
        }
    }

    /// <summary>
    /// Spawns a Objects from a ConsturctionObject
    /// </summary>
    /// <param name="callee">Calling GenericConstructionObject</param>
    /// <param name="ignoreCosts">ignores the costs (for example via cinematic scene)</param>
    public void spawnObject(ObjectBuildCostObject callee, bool ignoreCosts = false, PlayerHandler p = null)
    {
        if (p == null) p = UIPlayer;
        if (p.currentResources - callee.resourceCost >= 0 || ignoreCosts)
        {
            GameObject o = GameObject.Instantiate(callee.objectToInstanciate, guiManagerInstance.selectedUnit.transform.position, Quaternion.identity);
            o.transform.Translate(0, 0, -1 * (callee.transform.forward.z + 2f));
            configureGameObject(o, p);

            if (!ignoreCosts)
                this.Resources = this.Resources - (int)callee.resourceCost;
        }
        else
        {
            // not enough resources
            //Audio.infoAudioSource.PlayOneShot(Audio.AudioQueueStatus.notEnoughResources);
        }
    }


    /// <summary>
    /// Configures an object and sets the parent accordingly to GameManager
    /// </summary>
    /// <param name="o">Object to configure</param>
    public void configureGameObject(GameObject o, PlayerHandler p)
    {
        //if (newUnitHierarchyParentObject)
        //    o.transform.parent = newUnitHierarchyParentObject.transform;
        if (p.gameObjectSpawnParentName != "")
            o.transform.parent = p.playerObjectSpawnParent.transform;
        else
            o.transform.parent = this.gameObject.transform;

        o.GetComponent<Rigidbody>().velocity = Vector3.zero;
        guiManagerInstance.selectedUnit.GetComponent<Rigidbody>().velocity = Vector3.zero;

    }



}
