
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.IO;
using System;
using System.Threading;

public class Movement : MonoBehaviour
{
    // Declaration Of used variables
    public float tankspeed;
    public float currentTankSpeed;
    public Texture2D defaultCursor;
    //private Rigidbody tankrb;
    public Texture2D Movementcursor;
    public Vector2 hotSpot;
    public Vector3 targetPosition;
    public bool selected = false;
    public bool moving = false;
    System.Random RND = new System.Random();
    private bool firsttimeclick;
    private AudioSource quote;
    public UnityEngine.AudioClip[] tankQuoteArray;
    public CursorMode cursorMode = CursorMode.Auto;
//    double rotateangle;

    private GameManager gameManager;

    // Use this for initialization
    void Start()
    {
        quote = GetComponent<AudioSource>();
        gameManager = transform.root.GetComponentInChildren<GameManager>();
        //tankrb = GetComponent<Rigidbody>();
    }
    //OnMouseDown is called when the user clicks on the unit, thus toggling selection and changing the cursor...
    void OnMouseDown()
    {
        Cursor.SetCursor(Movementcursor, hotSpot, cursorMode);
        selected = true;
        firsttimeclick = true;
        gameManager.guiManagerInstance.selectedUnit = this.gameObject;
    }

    // plays a random quote
    void playRandomQuote(){
        int randquote;
        randquote = RND.Next(0, tankQuoteArray.Length - 1);
        quote.clip = tankQuoteArray[randquote];
        quote.Play();

    }
    void calculateTargetPosition(){
          //This part is the calculation of the coordinates of the location where the user has clicked.
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 oldPosition = transform.position;
            // double movedistance = 0;

            /*Vector3 axis;
            double rotateangle;
            float rotateangleFloat;
            transform.rotation.ToAngleAxis(out rotateangleFloat, out axis);
            rotateangle = rotateangleFloat;
            transform.Rotate(xAngle: 0, yAngle: -(float)rotateangle, zAngle: 0);*/
            
            if (Physics.Raycast(ray, out hit))
            {
                targetPosition.Set(hit.point.x, transform.position.y, hit.point.z);
                /*Calculation of the distance between the position where the user clicked and the unit's initial position,
                 to use that distance later...
                 The used formula, in a 3-Dimensional worldspace, is (assuming the points are A and B) :
                 Distance = √((Xb - Xa)^2 + (Yb - Ya)^2 + (Zb - Za)^2) */
                // movedistance = Math.Sqrt((targetPosition.x - oldPosition.x) * (targetPosition.x - oldPosition.x) + (targetPosition.y - oldPosition.y) * (targetPosition.y - oldPosition.y) + (targetPosition.z - oldPosition.z) * (targetPosition.z - oldPosition.z));
                // If the user clicked in front of the unit 
                if (targetPosition.z > oldPosition.z)
                {
                    /*Assuming the distance was the hypotenuse of a right triangle and one of his angles was the rotation angle...
                      Using trigonometry...*/
                    //rotateangle = (Math.Asin((targetPosition.x - oldPosition.x) / movedistance) * 180) / Math.PI;
                    //transform.Rotate(xAngle: 0, yAngle: (float)rotateangle, zAngle: 0, relativeTo: Space.Self);
                    moving = true;
                }
                if (targetPosition.x < oldPosition.x && targetPosition.z < oldPosition.z)
                {
                    //rotateangle = (Math.Acos((targetPosition.x - oldPosition.x) / movedistance) * 180) / Math.PI;
                    //rotateangle = rotateangle + 90.0;
                    //transform.Rotate(xAngle: 0, yAngle: (float)rotateangle, zAngle: 0, relativeTo: Space.Self);
                    moving = true;
                }
                if (targetPosition.x > oldPosition.x && targetPosition.z < oldPosition.z)
                {
                    //rotateangle = (Math.Acos((targetPosition.x - oldPosition.x) / movedistance) * 180) / Math.PI;
                    //rotateangle = rotateangle + 90.0;
                    //transform.Rotate(xAngle: 0, yAngle: (float)rotateangle, zAngle: 0, relativeTo: Space.Self);
                    moving = true;
                }
            }

    }
    // Update is called once per frame
    void Update()
    {

        // If the user right-clicks after selecting a unit, selection is toggled off and the default cursor is back.
        if (Input.GetMouseButtonDown(1) && selected == true)
        {
            Cursor.SetCursor(defaultCursor, hotSpot, cursorMode);
            selected = false;
            gameManager.guiManagerInstance.selectedUnit = null;
        }

        // check Left Mouse Button
        if (Input.GetMouseButtonDown(0) && selected == true && firsttimeclick == false)
        {
            playRandomQuote();
            calculateTargetPosition();
            // reset current tank speed to max speed
            currentTankSpeed = tankspeed;
        }

        // moving transistion
        if (moving == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentTankSpeed * Time.deltaTime);
            
            Quaternion _lookRotation;
         Vector3 _direction;
             //find the vector pointing from our position to the target
         _direction = (targetPosition - transform.position);
         //_direction.z = _direction.z * -1;
 
         //create the rotation we need to be in to look at the target
         _lookRotation = Quaternion.LookRotation(_direction);
 
         //rotate us over time according to speed until we are in the required rotation
         transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, Time.deltaTime * currentTankSpeed);

            if (transform.position == targetPosition)
            {
                // halt tank
                currentTankSpeed = 0;
                moving = false;
            }
        }
    }
    private void LateUpdate()
    {
        firsttimeclick = false;
    }
}
















