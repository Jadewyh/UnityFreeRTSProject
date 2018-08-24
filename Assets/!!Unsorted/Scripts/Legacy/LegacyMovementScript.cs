
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.IO;
using System;
using System.Threading;

public class LegacyMovementScript : MonoBehaviour
{
    // Declaration Of used variables
    public Texture2D defaultCursor;
    public Texture2D Movementcursor;
    public Vector2 hotSpot;
    public CursorMode cursorMode = CursorMode.Auto;
    public static bool selected = false;
    public static bool firsttimeclick;
    // Use this for initialization
    void Start()
    {
   
    }
    //OnMouseDown is called when the user clicks on the unit, thus toggling selection and changing the cursor...
    void OnMouseDown()
    {
        Cursor.SetCursor(Movementcursor, hotSpot, cursorMode);
        selected = true;
        firsttimeclick = true;
    }
    // Update is called once per frame
    void Update()
    {
       
        // If the user right-clicks after selecting a unit, selection is toggled off and the default cursor is back.
        if (Input.GetMouseButtonDown(1) && selected == true && ReturnOrder.MouseOver == false)
        {
            Cursor.SetCursor(defaultCursor, hotSpot, cursorMode);

            selected = false;
        }
    }
    private void LateUpdate()
    {
        firsttimeclick = false;
    }
}



    

         
       

        
   
    


        
    

