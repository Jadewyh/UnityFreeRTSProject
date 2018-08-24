using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public delegate void newPlayerJoined(PlayerHandler p);


public class GameManager : MonoBehaviour
{   
    public static GameManager myInstance;
     
    [ReadOnly] public float _resources;
    [ReadOnly] public int _PlayerRaderStations;

    public GameObject newUnitHierarchyParentObject;

    public List<PlayerHandler> playersInGame;

    [ReadOnly] public MapManager mapManagerInstance;
    [ReadOnly] public GUIManager guiManagerInstance;
    public event newPlayerJoined onPlayerJoin;

    public float Resources
    {
        get
        {
            return this._resources;
        }
        set
        {
            _resources = value;
            guiManagerInstance.UI_ResourceDisplayText.text = "$: " + _resources;
        }
    }

    public int PlayerRadarStations
    {
        get
        {
            return _PlayerRaderStations;
        }
        set
        {
            _PlayerRaderStations = value;
            if (_PlayerRaderStations <= 0)
            {
                guiManagerInstance.isMiniMapVisible = false;
            }
            else
            {
                guiManagerInstance.isMiniMapVisible = true;
            }
        }
    }
    public GameObject selectedUnit
    {
        get
        {
            return selectedUnits.Count > 0?selectedUnits[0]:null;
        }
        set
        {
            selectedUnits.Clear();
            if (value)
            {
                selectedUnits.Add(value);
                guiManagerInstance.UI_unitFollowerDisplayObject.SetActive(true);
                selectedUnit.GetComponent<GenericUnit>().isSelected = true;
            }
            else
            {
                if (selectedUnit)
                    selectedUnit.GetComponent<GenericUnit>().isSelected = false;
                selectedUnits.Remove(value);
            }
            guiManagerInstance.UI_unitFollowerDisplayObject.SetActive(value?true:false);
            // setConstructionObjects();
        }
    }

    public bool hasSomethingSelected {
        get {
            if (selectedUnit)
                return true;
            if (selectedUnits.Count > 0)
                return true;

            return false;
        }
    }

    public List<GameObject> selectedUnits = new List<GameObject>();

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
        selectedUnit = null;
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
        if (selectedUnit)
        {
            // make camera follow!
            Vector3 x = selectedUnit.transform.position;
            x.y = x.y + 5f;
            x.z = x.z - 20f;
            guiManagerInstance.UI_UnitFollowerCamera.transform.position = x;
            gu = selectedUnit.GetComponent<GenericUnit>();
        }

        // check Left Mouse Button
        if (Input.GetMouseButtonDown(0) && selectedUnit)
        {
            //if (GUI._selectedUnitGeneric && GUI._selectedUnitGeneric.AudioSettings.moveQuotes.Length > 0)
                //playRandomQuote(GUI._selectedUnitGeneric.AudioSettings.moveQuotes, Audio.quoteAudioSource);

            if (gu && gu.canMove)
            {
                calculateTargetPosition();
                gu.movementSpeed = gu.maxMovementSpeed;
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
        if (!selectedUnit) return;

        GenericUnit gu = selectedUnit.GetComponent<GenericUnit>();

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
        if (this._resources - callee.resourceCost >= 0 || ignoreCosts)
        {
            GameObject o = GameObject.Instantiate(callee.CreationObject, selectedUnit.transform.position, Quaternion.identity);
            o.transform.Translate(0, 0, -1 * (callee.transform.forward.z + 2f));
            configureGameObject(o);

            if (!ignoreCosts)
                this.Resources = this.Resources - callee.resourceCost;
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
        selectedUnit.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

 

}
