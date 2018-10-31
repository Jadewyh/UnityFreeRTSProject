using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public delegate void newPlayerJoined(PlayerHandler p);


public class GameManager : MonoBehaviour
{
    // Public Instance Reference
    public static GameManager myInstance;

    [Header("Read-only Varibles")]
    #region Attributes (read only)
    [ReadOnly] public List<PlayerHandler> playersInGame;

    [ReadOnly] public MapManager _mapManagerInstance;
    [ReadOnly] public GUIManager _guiManagerInstance;
    [ReadOnly] public PlayerHandler UIPlayer;
    [ReadOnly] public Terrain playingTerrain;
    public event newPlayerJoined OnPlayerJoin;
    public bool hasStarted = false;
    #endregion

    #region Accessers
    public MapManager mapManagerInstance
    {
        get
        {
            _mapManagerInstance = this.gameObject.GetComponentInChildren<MapManager>();

            if (!_mapManagerInstance)
                _mapManagerInstance = gameObject.AddComponent<MapManager>();

            return _mapManagerInstance;
        }
    }

    public GUIManager guiManagerInstance
    {
        get
        {
            _guiManagerInstance = this.gameObject.GetComponentInChildren<GUIManager>();
            if (!_guiManagerInstance)
                _guiManagerInstance = gameObject.AddComponent<GUIManager>();
            return _guiManagerInstance;
        }
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
    #endregion

    #region Public / Callable functions
    public void returnToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleMenu", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void startGame()
    {

        if (!mapManagerInstance.mapGen.TerrainObject)
        {
            Debug.LogError("No TerrainObject available!");
            return;
        }

        PlayerRadarStations = 0;
        guiManagerInstance.selectedUnit = null;

        playingTerrain = mapManagerInstance.mapGen.TerrainObject.GetComponent<Terrain>();

        // work around for 'campaign' if we only want to show a certain area at a time
        //TODO: TerrainRestrictionPosition (for non x=0/y=0 maps/growth)
        if (playingTerrain && mapManagerInstance.TerrainRestriction == Vector3.zero)
            mapManagerInstance.TerrainRestriction = playingTerrain.terrainData.size;

        // If skirmish => generate players
        PlayerSelectOptionsGenerator psog = GameObject.Find("MapConfigScreen").GetComponentInChildren<PlayerSelectOptionsGenerator>();
        if (psog)
            psog.convertPlayerSelectsToPlayers();

        // Generate NavMesh
        mapManagerInstance.generateAI();

        // Spawn Player(s) if not already spawned.
        foreach (PlayerHandler p in playersInGame)
        {
            var spawningPoint = new Vector3(UnityEngine.Random.value * playingTerrain.terrainData.size.x,
                                           UnityEngine.Random.value * playingTerrain.terrainData.size.y,
                                            UnityEngine.Random.value * playingTerrain.terrainData.size.z);

            foreach (GameObject o in p.playerInitialSpawnObjects)
            {
                p.spawnObject(o, spawningPoint);
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
    #endregion

    #region Unity Functions (Start, Update, LateUpdate)
    /// <summary>
    /// Initializes GameManager
    /// </summary>
    // Use this for initialization
    void Start()
    {
        // TODO: Bundle logic
        // Debug.Log("Following Bundles are loaded:");
        // foreach(AssetBundle b in AssetBundle.GetAllLoadedAssetBundles()){
        // Debug.Log(b.ToString());
        // }

        if (!myInstance)
            myInstance = this;
        else
        {
            //TODO: Proper Error Handling of multiple instances of GameManager
            Debug.LogError("GameManager is already instanciated! You are doing something fucky.");
        }

        // Initialize gameManager
        playersInGame = new List<PlayerHandler>();
    }


    /// <summary>
    /// GameObject Update method (as often as possible)
    /// </summary>
    // Update is called once per frame
    void Update()
    {
        // Moved everything to FixedUpdate since not frame important!
    }
    private void FixedUpdate()
    {
        UpdatePositionCalculations();
    }
    #endregion

    #region Internal functions
    /// <summary>
    /// Calculate / Check position updates for selected unit(s)
    /// </summary>
    void UpdatePositionCalculations()
    {
        // if a unit is selected
        if (guiManagerInstance.selectedUnit)
        {
            // make camera follow!
            Vector3 x = guiManagerInstance.selectedUnit.transform.position;
            x.y = x.y + 5f;
            x.z = x.z - 20f;
            guiManagerInstance.UI_UnitFollowerCamera.transform.position = x;
        }

        // check Left Mouse Button
        if (Input.GetMouseButtonDown(0) && guiManagerInstance.selectedUnit)
        {
            //if (GUI._selectedUnitGeneric && GUI._selectedUnitGeneric.AudioSettings.moveQuotes.Length > 0)
            //playRandomQuote(GUI._selectedUnitGeneric.AudioSettings.moveQuotes, Audio.quoteAudioSource);

            if (guiManagerInstance.selectedUnitGenericUnit && guiManagerInstance.selectedUnitGenericUnit.canMove)
            {
                calculateTargetPosition();
                guiManagerInstance.selectedUnitGenericUnit.movementSpeed = guiManagerInstance.selectedUnitGenericUnit.maxMovementSpeed;
            }
        }
    }

    //TODO: [MINIMAP] Minimap Position
    void drawMainCameraPositionOnMiniMap()
    {
        //GUI._unitFollowerProjectionObject;
        DrawRectangle(Camera.main.GetComponent<Camera>().rect, Color.black);

    }

    //TODO: [MINIMAP] DrawRect
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

        GenericUnit unit = guiManagerInstance.selectedUnitGenericUnit;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            unit.moveTargetPoint.Set(hit.point.x, transform.position.y, hit.point.z);
            NavMeshDisplay d = GetComponentInChildren<NavMeshDisplay>();
            if (d != null)
            {
                d.target = unit.moveTargetPoint;
            }
            else
            {
                Debug.LogError("NavMeshDisplay is null!");
            }
            unit.isMoving = true;


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
        if (p.gameObjectSpawnParentName != "")
            o.transform.parent = p.playerObjectSpawnParent.transform;
        else
            o.transform.parent = this.gameObject.transform;

        o.GetComponent<Rigidbody>().velocity = Vector3.zero;
        guiManagerInstance.selectedUnit.GetComponent<Rigidbody>().velocity = Vector3.zero;

    }

    #endregion


}
