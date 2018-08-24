using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour {

		public string UnitFollowCameraObjectName = "GameManager_MiniMapCamera";
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

		
	// Use this for initialization
	void Start () {
		UI_unitFollowerDisplayObject = GameObject.Find(UnitFollowDisplayObjectName);
		UI_UnitFollowerCameraObject = GameObject.Find(UnitFollowCameraObjectName);
		UI_ResourceDisplay = GameObject.Find(resourceDisplayObjectName);
		UI_bottomMiddleContainer = GameObject.Find(bottomMiddleContainerObjectName);
		UI_miniMapContainerObject = GameObject.Find(miniMapContainerObjectName);
        UI_ResourceDisplayText = UI_ResourceDisplay.GetComponent<Text>();
        UI_UnitFollowerCamera = UI_UnitFollowerCameraObject.GetComponent<Camera>();
        UI_UnitFollowerCameraObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
