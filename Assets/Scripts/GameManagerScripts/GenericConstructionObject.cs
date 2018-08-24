using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenericConstructionObject: MonoBehaviour
{
    public Mesh ButtonDisplay;
    public float resourceCost;
    public GameObject CreationObject;
    public GameManager gameManager;
    private Button myButton;

    // Use this for initialization
    void Start()
    {
        gameManager = GameManager.myInstance;
        myButton = this.GetComponent<Button>();
        myButton.onClick.AddListener(delegate {gameManager.spawnObject(this); });
    }
    //OnMouseDown is called when the user clicks on the unit, thus toggling selection and changing the cursor...
    void OnMouseDown()
    {
        gameManager.guiManagerInstance.selectedUnit = this.gameObject;
    }

}