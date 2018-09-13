using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

		public string UnitFollowCameraObjectName = "GameManager_SelectedUnitFollowerCamera";
		public string UnitFollowDisplayObjectName = "GameManager_SelectedUnitDisplay";
		public string resourceDisplayObjectName = "GameManager_CashResourceDisplayText";
		public string bottomMiddleContainerObjectName = "GameManager_MiddlePanelForButtons";
		public string miniMapContainerObjectName = "GameManager_MiniMapDisplay";

		[ReadOnly] private bool _isMiniMapVisible;
    public bool isMiniMapVisible
    {
        get
        {
            return _isMiniMapVisible;
        }
        set
        {
            _isMiniMapVisible = value;
            UI_miniMapContainerObject.SetActive(_isMiniMapVisible);
        }
    }

        [ReadOnly] public GameObject UI_ResourceDisplay;
		[ReadOnly] public GameObject UI_UnitFollowerCameraObject;
    	[ReadOnly] public GameObject UI_unitFollowerDisplayObject;
		[ReadOnly] public GameObject UI_miniMapContainerObject;
        [ReadOnly] public Camera UI_UnitFollowerCamera;
        [ReadOnly] public Text UI_ResourceDisplayText;
    	[ReadOnly] public GameObject UI_bottomMiddleContainer;		
		
	    public List<GameObject> selectedUnits = new List<GameObject>();

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
                UI_unitFollowerDisplayObject.SetActive(true);
                selectedUnit.GetComponent<GenericUnit>().isSelected = true;
            }
            else
            {
                if (selectedUnit)
                    selectedUnit.GetComponent<GenericUnit>().isSelected = false;
                selectedUnits.Remove(value);
            }
            UI_unitFollowerDisplayObject.SetActive(value?true:false);
            setUnitActionButtons();
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

		
	// Use this for initialization
	void Start () {
		UI_unitFollowerDisplayObject = GameObject.Find(UnitFollowDisplayObjectName);
		UI_UnitFollowerCameraObject = GameObject.Find(UnitFollowCameraObjectName);
		UI_ResourceDisplay = GameObject.Find(resourceDisplayObjectName);
		UI_bottomMiddleContainer = GameObject.Find(bottomMiddleContainerObjectName);
		UI_miniMapContainerObject = GameObject.Find(miniMapContainerObjectName);
        UI_ResourceDisplayText = UI_ResourceDisplay.GetComponent<Text>();
        UI_UnitFollowerCamera = UI_UnitFollowerCameraObject.GetComponent<Camera>();
        //UI_UnitFollowerCameraObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setUnitActionButtons()
    {

        if (!UI_bottomMiddleContainer) return;
        foreach (Button b in UI_bottomMiddleContainer.GetComponentsInChildren<Button>())
        {
            Destroy(b.gameObject);
        }
        if (hasSomethingSelected)
        {
            foreach (GenericConstructionObject gco in selectedUnit.GetComponentInChildren<GenericUnit>().availableConstructionObjects)
            {
                GameObject go = Instantiate(gco.gameObject);
                go.transform.SetParent(UI_bottomMiddleContainer.transform);
            }
        }
    }

}
