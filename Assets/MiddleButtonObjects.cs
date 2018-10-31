using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiddleButtonObjects : MonoBehaviour
{

    public string buttonName;
    [ReadOnly] public GameManager gameManager;
    protected Button myButton;

    // Use this for initialization
    protected void Start()
    {
        gameManager = GameManager.myInstance;
        myButton = this.GetComponent<Button>();
        if (!myButton)
            myButton = gameObject.AddComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
