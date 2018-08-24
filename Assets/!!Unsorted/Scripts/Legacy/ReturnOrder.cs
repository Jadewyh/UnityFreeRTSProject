using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnOrder : MonoBehaviour
{
    private bool clicked;
    public static bool MouseOver;
    public Texture2D whileover;
    private Vector2 overhotspot;

    private void Start()
    {
        overhotspot.Set(whileover.width / 2, whileover.height);
        MouseOver = false;
    }

    private void OnMouseDown()
    {
        if (LegacyMovementScript.selected == true)
            clicked = true;
    }
    private void OnMouseOver()
    {
        if (LegacyMovementScript.selected == true)
        {
            MouseOver = true;
        }
    }
    // Update is called once per frame
    void Update () {
		if(clicked == true)
        {
            FlyingOld.orderedtoreturn = true;
        }
        if(MouseOver == true)
        {
            Cursor.SetCursor(whileover, overhotspot, CursorMode.Auto);
        }
	}
    private void LateUpdate()
    {
        clicked = false;
        MouseOver = false;
    }
}
