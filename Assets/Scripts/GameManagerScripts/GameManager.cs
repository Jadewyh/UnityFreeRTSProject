using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public delegate void newPlayerJoined(PlayerHandler p);


public class GameManager : MonoBehaviour
{
    public void returnToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    public void startGame()
    {
        mapManagerInstance.generateAI();
        foreach (PlayerHandler p in playersInGame)
        {
            var spawningPoint = new Vector3(UnityEngine.Random.value * mapManagerInstance.TerrainRestriction.x,
                                           UnityEngine.Random.value * mapManagerInstance.TerrainRestriction.y,
                                            UnityEngine.Random.value * mapManagerInstance.TerrainRestriction.z);

            foreach (GameObject o in p.playerInitialSpawnObjects)
            {

                GameObject obj = GameObject.Instantiate(o, spawningPoint, Quaternion.identity);
                obj.transform.parent = p.playerObjectSpawnParent.transform;
            }
        }
    }
    public static GameManager myInstance;

    public GameObject newUnitHierarchyParentObject;

    [ReadOnly] private List<PlayerHandler> playersInGame;

    [ReadOnly] public MapManager mapManagerInstance;
    [ReadOnly] public GUIManager guiManagerInstance;

    [ReadOnly] public PlayerHandler UIPlayer;
    public event newPlayerJoined onPlayerJoin;

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
        if (playersInGame == null) return null;
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
    /// Helper Method to create the Action Buttons
    /// </summary>
    // void setConstructionObjects()
    // {
    //     if (!GUI.constructionObjectContainer) return;
    //     foreach (Button b in GUI.constructionObjectContainer.GetComponentsInChildren<Button>())
    //     {
    //         Destroy(b.gameObject);
    //     }
    //     if (GUI._selectedUnit)
    //     {
    //         foreach (GenericConstructionObject gco in GUI._selectedUnit.GetComponentInChildren<GenericUnit>().availableConstructionObjects)
    //         {
    //             GameObject go = Instantiate(gco.gameObject);
    //             go.transform.SetParent(GUI.constructionObjectContainer.transform);
    //         }
    //     }
    // }

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
            gu.isMoving = true;


        }
    }

    /// <summary>
    /// Spawns a Objects from a ConsturctionObject
    /// </summary>
    /// <param name="callee">Calling GenericConstructionObject</param>
    /// <param name="ignoreCosts">ignores the costs (for example via cinematic scene)</param>
    public void spawnObject(GenericConstructionObject callee, bool ignoreCosts = false)
    {
        if (UIPlayer.currentResources - callee.resourceCost >= 0 || ignoreCosts)
        {
            GameObject o = GameObject.Instantiate(callee.CreationObject, guiManagerInstance.selectedUnit.transform.position, Quaternion.identity);
            o.transform.Translate(0, 0, -1 * (callee.transform.forward.z + 2f));
            configureGameObject(o);

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
    public void configureGameObject(GameObject o)
    {
        if (newUnitHierarchyParentObject)
            o.transform.parent = newUnitHierarchyParentObject.transform;
        else
            o.transform.parent = this.gameObject.transform;

        o.GetComponent<Rigidbody>().velocity = Vector3.zero;
        guiManagerInstance.selectedUnit.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }



}
