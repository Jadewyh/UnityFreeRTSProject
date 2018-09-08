using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMovementScript : MonoBehaviour {
    public Camera mainCamera;
    public GameObject terrain;
    public float screenHeightInUnits;
    public float screenWidthInUnits;
    public Vector3 p3;
    public Vector3 p4;
    private TerrainData d;

    // Use this for initialization
    void Start () {
        mainCamera = Camera.main;
        d = terrain.GetComponent<Terrain>().terrainData;

    }
	
	// Update is called once per frame
	void Update () {
        screenHeightInUnits = Camera.main.orthographicSize * 2;
        screenWidthInUnits = screenHeightInUnits * Screen.width / Screen.height;

        /*float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (h < -.5 || h > .5)
        {
            mainCamera.transform.Translate(h, 0f, 0f);
        }
        if (v < -.5 || v > .5)
        {
            mainCamera.transform.Translate(0f, v, 0f);
        }*/


        if (Input.GetKey(KeyCode.LeftArrow) && mainCamera.transform.position.x > (screenWidthInUnits/2 - 5))
        {
            mainCamera.transform.Translate(-1f, 0f, 0f, Space.World);
        }
        if (Input.GetKey(KeyCode.RightArrow) && mainCamera.transform.position.x < (d.detailWidth - (screenWidthInUnits/2 - 5)))
        {
            mainCamera.transform.Translate(1f, 0f, 0f, Space.World);
        }

        if (Input.GetKey(KeyCode.UpArrow) && mainCamera.transform.position.z < (d.detailHeight - mainCamera.transform.position.y - screenHeightInUnits/2 - 5))
        {
            mainCamera.transform.Translate(0f, 0f, 1f, Space.World);
        }
        if (Input.GetKey(KeyCode.DownArrow) && mainCamera.transform.position.z > ((-1)*mainCamera.transform.position.y + (screenHeightInUnits / 2 ) + 5))
        {
            mainCamera.transform.Translate(0f, -0f, -1f, Space.World);
        }


    }
    void OnGUI()
    {
        Vector3 p = new Vector3();
        Camera c = Camera.main;
        Event e = Event.current;
        Vector2 mousePos = new Vector2();

        // Get the mouse position from Event.
        // Note that the y position from Event is inverted.
        mousePos.x = e.mousePosition.x;
        mousePos.y = c.pixelHeight - e.mousePosition.y;

        p = c.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, c.nearClipPlane));

        GUI.contentColor = Color.black;
        GUI.backgroundColor = Color.white;
        GUILayout.BeginArea(new Rect(20, 20, 250, 120));
        GUILayout.Label("Screen pixels: " + c.pixelWidth + ":" + c.pixelHeight);
        GUILayout.Label("Mouse position: " + mousePos);
        GUILayout.Label("World position: " + p.ToString("F3"));
        GUILayout.EndArea();
    }
}
