using UnityEngine;
using UnityEngine.UI;

public class ObjectBuildCostObject : MiddleButtonObjects
{
    public float resourceCost;
    public GameObject objectToInstanciate;

    // Use this for initialization
    new void Start()
    {
        base.Start();
        myButton.GetComponentInChildren<Text>().text = buttonName;
        myButton.onClick.AddListener(delegate { gameManager.spawnObject(this); });
    }
    //OnMouseDown is called when the user clicks on the unit, thus toggling selection and changing the cursor...
    void OnMouseDown()
    {
        gameManager.guiManagerInstance.selectedUnit = this.gameObject;
    }

}
