using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RTSCameraBeta : MonoBehaviour
{
    public float camspeed = 120f;
    public bool rightclickmoving;
    public Vector3 rightclickloc;
    public Texture2D defaultcursor;
    public Texture2D upcursor;
    public Texture2D downcursor;
    public Texture2D rightcursor;
    public Texture2D leftcursor;
    public Texture2D uprightcursor;
    public Texture2D upleftcursor;
    public Texture2D downrightcursor;
    public Texture2D downleftcursor;
    public Texture2D MovCursor;
    public CursorMode cursormode;
    public Vector2 hotspot;

    public Vector3 TerrainRestriction;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mousePosition.x > 1 && Input.mousePosition.x < Screen.width - 1 && Input.mousePosition.y > 1 &&
        Input.mousePosition.y < Screen.height - 1 && LegacyMovementScript.selected == false && ReturnOrder.MouseOver == false)
        {
            hotspot.Set(0, 0);
            Cursor.SetCursor(texture: defaultcursor, hotspot: hotspot, cursorMode: cursormode);
        }
        if (Input.mousePosition.x > 1 && Input.mousePosition.x < Screen.width - 1 && Input.mousePosition.y > 1 &&
        Input.mousePosition.y < Screen.height - 1 && LegacyMovementScript.selected == true && ReturnOrder.MouseOver == false)
        {
            hotspot.Set(MovCursor.width / 2, MovCursor.height / 2);
            Cursor.SetCursor(texture: MovCursor, hotspot: hotspot, cursorMode: cursormode);
        }
        if (Input.mousePosition.x <= 1 && transform.position.x > 0 && ReturnOrder.MouseOver == false)
        {
            hotspot.Set(0, leftcursor.height / 2);
            Cursor.SetCursor(texture: leftcursor, hotspot: hotspot, cursorMode: cursormode);
            transform.Translate(x: -camspeed * Time.deltaTime, y: 0, z: 0);
        }
        if (Input.mousePosition.x >= Screen.width - 1 && transform.position.x < TerrainRestriction.x && ReturnOrder.MouseOver == false)
        {
            hotspot.Set(rightcursor.width, rightcursor.height);
            Cursor.SetCursor(texture: rightcursor, hotspot: hotspot, cursorMode: cursormode);
            transform.Translate(x: camspeed * Time.deltaTime, y: 0, z: 0);
        }
        if (Input.mousePosition.y <= 1 && transform.position.z > 4 && ReturnOrder.MouseOver == false)
        {
            hotspot.Set(downcursor.width / 2, downcursor.height);
            Cursor.SetCursor(texture: downcursor, hotspot: hotspot, cursorMode: cursormode);
            transform.Translate(x: 0, y: 0, z: -(camspeed) * Time.deltaTime, relativeTo: Space.World);
        }
        if (Input.mousePosition.y >= Screen.height - 1 && transform.position.z < TerrainRestriction.z && ReturnOrder.MouseOver == false)
        {
            hotspot.Set(upcursor.width / 2, 0);
            Cursor.SetCursor(texture: upcursor, hotspot: hotspot, cursorMode: cursormode);
            transform.Translate(x: 0, y: 0, z: (camspeed) * Time.deltaTime, relativeTo: Space.World);
        }
        if (Input.mousePosition.x <= 1 && Input.mousePosition.y >= Screen.height - 1 && ReturnOrder.MouseOver == false)
        {
            hotspot.Set(0, 0);
            Cursor.SetCursor(texture: upleftcursor, hotspot: hotspot, cursorMode: cursormode);
        }
        if (Input.mousePosition.x >= Screen.width - 1 && Input.mousePosition.y >= Screen.height - 1 && ReturnOrder.MouseOver == false)
        {
            hotspot.Set(0, uprightcursor.width);
            Cursor.SetCursor(texture: uprightcursor, hotspot: hotspot, cursorMode: cursormode);
        }
        if (Input.mousePosition.x <= 1 && Input.mousePosition.y <= 1 && ReturnOrder.MouseOver == false)
        {
            hotspot.Set(0, downleftcursor.height);
            Cursor.SetCursor(texture: downleftcursor, hotspot: hotspot, cursorMode: cursormode);
        }
        if (Input.mousePosition.x >= Screen.width - 1 && Input.mousePosition.y <= 1 && ReturnOrder.MouseOver == false)
        {
            hotspot.Set(downrightcursor.width, downrightcursor.height);
            Cursor.SetCursor(texture: downrightcursor, hotspot: hotspot, cursorMode: cursormode);

        }
        if (Input.GetMouseButtonDown(1))
        {
            rightclickmoving = true;
            rightclickloc = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(1))
        {
            rightclickmoving = false;
        }
        if (rightclickmoving == true)
        {
            if (Input.mousePosition.x > rightclickloc.x + (Screen.width / 10) && transform.position.x < 802)
            {
                transform.Translate(x: 2 * camspeed * Time.deltaTime, y: 0, z: 0);
            }
            if (Input.mousePosition.x < rightclickloc.x - (Screen.width / 10) && transform.position.x > 166)
            {
                transform.Translate(x: 2 * -camspeed * Time.deltaTime, y: 0, z: 0);
            }
            if (Input.mousePosition.y > rightclickloc.y && transform.position.z < 862)
            {
                transform.Translate(x: 0, y: 0, z: 2 * camspeed * Time.deltaTime, relativeTo: Space.World);
            }
            if (Input.mousePosition.y < rightclickloc.y && transform.position.z > 4)
            {
                transform.Translate(x: 0, y: 0, z: 2 * -camspeed * Time.deltaTime, relativeTo: Space.World);
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && transform.position.y > 10)
        {
            transform.Translate(x: 0, y: 0, z: 160f * Time.deltaTime, relativeTo: Space.Self);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f && transform.position.y < 50)
        {
            transform.Translate(x: 0, y: 0, z: -160f * Time.deltaTime, relativeTo: Space.Self);
        }
    }
}
