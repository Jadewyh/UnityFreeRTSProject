using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public enum MoveDirection
{
    None = 0,
    Up = 1,
    Left = 2,
    Down = 3,
    Right = 4,
    UpLeft = 5,
    UpRight = 6,
    DownLeft = 7,
    DownRight = 8
}

public class MouseManager : MonoBehaviour
{
    [System.Serializable]
    public class assignableFields
    {
        public Texture2D defaultCursorNoSelect;
        public Texture2D targetSelectionCursor;
        public Texture2D moveCursorUp;
        public Texture2D moveCursorLeft;
        public Texture2D moveCursorDown;
        public Texture2D moveCursorRight;
        public Texture2D moveCursorDownLeft;
        public Texture2D moveCursorDownRight;

        public Texture2D moveCursorUpLeft;
        public Texture2D moveCursorUpRight;


        public float CursorMovementSpeed;
        public float windowBorderDistance;
        public CursorMode cursorMode;


    }

    [System.Serializable]
    public class values
    {
        public Vector2 cursorHotSpotMouse;
        public MoveDirection currentMouseMoveDirection = 0;
        public Vector2 mouseTargetPosition;
        public GameManager gameManager;
        public MapManager mapManager;

        public Vector2 selectionBoxStartPoint;
        public Vector2 selectionBoxEndPoint;
        public RectTransform selectionBoxRect;
        public Image selectionBoxImage;
        public bool selectionBoxActive;
        public GameObject _selectionBox;


    }
    public GameObject selectionBox
    {
        get
        {
            return Values._selectionBox;
        }
        set
        {
            if (!value) return;
            Values._selectionBox = value;
            Values.selectionBoxRect = value.GetComponent<RectTransform>();
            Values.selectionBoxImage = value.GetComponent<Image>();
        }
    }

    [ReadOnly] public values Values;
    [ReadOnly] public float screenWidthInUnits;
    [ReadOnly] public float screenHeightInUnits;
    public assignableFields AssignableFields;

    // Use this for initialization
    void Start()
    {

        Values.mapManager = transform.GetComponent<MapManager>();
        Values.gameManager = transform.GetComponent<GameManager>();
        if (!selectionBox)
        {
            selectionBox = GameObject.Find("MouseManager_SelectionBox");
        }

        screenHeightInUnits = Camera.main.orthographicSize * 2;
        screenWidthInUnits = screenHeightInUnits * Screen.width / Screen.height;

    }

    // Update is called once per frame
    void Update()
    {
        if (!Values.gameManager.hasStarted) return;
        if (!selectionBox) return;
        // If the user right-clicks after selecting a unit, selection is toggled off and the default cursor is back.
        if (Input.GetMouseButtonDown(1) && Values.gameManager.guiManagerInstance.hasSomethingSelected)
        {
            Cursor.SetCursor(AssignableFields.defaultCursorNoSelect, Values.mouseTargetPosition, AssignableFields.cursorMode);
            Values.gameManager.guiManagerInstance.selectedUnit = null;
        }

    }

    // fixed is enough for our load
    private void FixedUpdate()
    {
        if (!Values.gameManager.hasStarted) return;

        if (!selectionBox) return;
        UpdateRTSCamera();
    }

    /// <summary>
    /// Helper Function to set the cursor
    /// </summary>
    /// <param name="cursor">The Cursor</param>
    /// <param name="hotSpot">Hotspot value to "change the tip"</param>
    /// <param name="movement">The Direction</param>
    void setRTSCursor(Texture2D cursor, Vector2 hotSpot, MoveDirection movement)
    {
        Values.currentMouseMoveDirection = movement;
        Values.cursorHotSpotMouse = hotSpot;
        Cursor.SetCursor(cursor, Values.cursorHotSpotMouse, AssignableFields.cursorMode);
    }
    public Vector3 localPos = Vector3.zero;

    /// <summary>
    /// Update / Move Real Time Stretegy Camera
    /// </summary>
    void UpdateRTSCamera()
    {
        float dist = AssignableFields.windowBorderDistance;
        if (Input.mousePosition.x < dist) // left side
        {
            if (Input.mousePosition.y < dist)
            { // top left
                setRTSCursor(AssignableFields.moveCursorDownLeft, new Vector2(0, 0), MoveDirection.DownLeft);
            }
            else if (Input.mousePosition.y > Screen.height - dist)
            { // bottom left
                setRTSCursor(AssignableFields.moveCursorUpLeft, new Vector2(0, 0), MoveDirection.UpLeft);
            }
            else
            { // left middle
                setRTSCursor(AssignableFields.moveCursorLeft, new Vector2(0, 0), MoveDirection.Left);
            }
        }
        else if (Input.mousePosition.x > Screen.width - dist)
        {
            if (Input.mousePosition.y < dist)
            { // top right
                setRTSCursor(AssignableFields.moveCursorDownRight, new Vector2(0, 0), MoveDirection.DownRight);
            }
            else if (Input.mousePosition.y > Screen.height - dist)
            { // bottom right
                setRTSCursor(AssignableFields.moveCursorUpRight, new Vector2(0, 0), MoveDirection.UpRight);
            }
            else
            { // right middle
                setRTSCursor(AssignableFields.moveCursorRight, new Vector2(0, 0), MoveDirection.Right);
            }
        }
        else if (Input.mousePosition.y < dist)
        { // top middle
            setRTSCursor(AssignableFields.moveCursorDown, new Vector2(0, 0), MoveDirection.Down);
        }
        else if (Input.mousePosition.y > Screen.height - dist)
        { // bottom middle
            setRTSCursor(AssignableFields.moveCursorUp, new Vector2(0, 0), MoveDirection.Up);
        }
        else
        {
            setRTSCursor(AssignableFields.defaultCursorNoSelect, new Vector2(0, 0), MoveDirection.None);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            Camera.main.GetComponent<Camera>().orthographicSize -= AssignableFields.CursorMovementSpeed * Time.deltaTime;
            //drawMainCameraPositionOnMiniMap();
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            Camera.main.GetComponent<Camera>().orthographicSize += AssignableFields.CursorMovementSpeed * Time.deltaTime;
            //drawMainCameraPositionOnMiniMap();
        }


        if (Values.currentMouseMoveDirection != MoveDirection.None)
        {
            List<MoveDirection> splitDir = new List<MoveDirection>();
            if (Values.currentMouseMoveDirection == MoveDirection.UpRight ||
                Values.currentMouseMoveDirection == MoveDirection.Right ||
                Values.currentMouseMoveDirection == MoveDirection.DownRight)
            {
                splitDir.Add(MoveDirection.Right);
            }
            if (Values.currentMouseMoveDirection == MoveDirection.UpLeft ||
                Values.currentMouseMoveDirection == MoveDirection.Up ||
                Values.currentMouseMoveDirection == MoveDirection.UpRight)
            {
                splitDir.Add(MoveDirection.Up);
            }
            if (Values.currentMouseMoveDirection == MoveDirection.DownRight ||
                Values.currentMouseMoveDirection == MoveDirection.Down ||
                Values.currentMouseMoveDirection == MoveDirection.DownLeft)
            {
                splitDir.Add(MoveDirection.Down);
            }
            if (Values.currentMouseMoveDirection == MoveDirection.DownLeft ||
                Values.currentMouseMoveDirection == MoveDirection.Left ||
                Values.currentMouseMoveDirection == MoveDirection.UpLeft)
            {
                splitDir.Add(MoveDirection.Left);
            }
            //Debug.Log("Ding: "+);

            foreach (MoveDirection m in splitDir)
            {
                //float zVal = Camera.main.transform.position.z;
                switch (m)
                {
                    case MoveDirection.Right:
                        if (Camera.main.transform.position.x < (Values.gameManager.mapManagerInstance.mapGen.TerrainObject.GetComponent<Terrain>().terrainData.size.x - (screenWidthInUnits / 2 - 5)))
                            //if (Camera.main.gameObject.transform.position.x <= Values.mapManager.TerrainRestriction.x)
                            Camera.main.transform.Translate(x: 2 * AssignableFields.CursorMovementSpeed * Time.deltaTime, y: 0, z: 0);
                        //drawMainCameraPositionOnMiniMap();
                        break;
                    case MoveDirection.Left:
                        //if (Camera.main.gameObject.transform.position.x >= 0)
                        if (Camera.main.transform.position.x > (screenWidthInUnits / 2 - 5))
                            Camera.main.transform.Translate(x: -2 * AssignableFields.CursorMovementSpeed * Time.deltaTime, y: 0, z: 0);
                        //drawMainCameraPositionOnMiniMap();
                        break;

                    case MoveDirection.Up:
                        if (Camera.main.transform.position.z < (Values.gameManager.mapManagerInstance.mapGen.TerrainObject.GetComponent<Terrain>().terrainData.size.z - Camera.main.transform.position.y - screenHeightInUnits / 2 - 5))
                        {
                            Camera.main.transform.Translate(0f, 0f, 2 * AssignableFields.CursorMovementSpeed * Time.deltaTime, Space.World);
                        }
                        break;
                    case MoveDirection.Down:
                        if (Camera.main.transform.position.z > ((-1) * Camera.main.transform.position.y + (screenHeightInUnits / 2) + 5))
                            Camera.main.transform.Translate(0f, -0f, -2 * AssignableFields.CursorMovementSpeed * Time.deltaTime, Space.World);
                        break;
                }
            }
        }
    }

    private void LateUpdate()
    {
        if (!Values.gameManager.hasStarted) return;
        if (!selectionBox) return;
        if (!Values.gameManager.guiManagerInstance.hasSomethingSelected && Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))
                Values.selectionBoxStartPoint = Input.mousePosition;
            else if (Input.GetMouseButtonUp(0))
            {
                return;
            }
            else
            {

                if (!selectionBox.activeSelf)
                {
                    selectionBox.SetActive(true);
                }
                Values.selectionBoxEndPoint = Input.mousePosition;
                Vector2 boxdrawstart = Values.selectionBoxStartPoint;
                float width = Mathf.Abs(boxdrawstart.x - Values.selectionBoxEndPoint.x);
                float height = Mathf.Abs(boxdrawstart.y - Values.selectionBoxEndPoint.y);
                Vector2 boxcenter = (boxdrawstart + Values.selectionBoxEndPoint) / 2f;
                Values.selectionBoxRect.sizeDelta = new Vector2(width, height);
                Values.selectionBoxRect.position = boxcenter;
                Values.selectionBoxActive = true;
            }
        }
        else if (this.Values.selectionBoxActive)
        {
            foreach (GenericUnit g in GetComponentsInChildren<GenericUnit>())
            {
                if (g.owner != Values.gameManager.UIPlayer) continue;
                Vector2 unitscreenpos = Camera.main.WorldToScreenPoint(g.transform.position);
                if ((unitscreenpos.x >= Values.selectionBoxStartPoint.x && unitscreenpos.y >= Values.selectionBoxStartPoint.y && unitscreenpos.x <= Values.selectionBoxEndPoint.x
                    && unitscreenpos.y <= Values.selectionBoxEndPoint.y) || (unitscreenpos.x >= Values.selectionBoxStartPoint.x && unitscreenpos.y <= Values.selectionBoxStartPoint.y && unitscreenpos.x <= Values.selectionBoxEndPoint.x
                    && unitscreenpos.y >= Values.selectionBoxEndPoint.y) || (unitscreenpos.x <= Values.selectionBoxStartPoint.x && unitscreenpos.y >= Values.selectionBoxStartPoint.y && unitscreenpos.x >= Values.selectionBoxEndPoint.x
                    && unitscreenpos.y <= Values.selectionBoxEndPoint.y) || (unitscreenpos.x <= Values.selectionBoxStartPoint.x && unitscreenpos.y <= Values.selectionBoxStartPoint.y && unitscreenpos.x >= Values.selectionBoxEndPoint.x
                    && unitscreenpos.y >= Values.selectionBoxEndPoint.y))
                {
                    g.isSelected = true;
                    Values.gameManager.guiManagerInstance.selectedUnits.Add(g.gameObject);
                }
            }
            selectionBox.SetActive(false);
            Values.selectionBoxActive = false;
        }
    }

}
