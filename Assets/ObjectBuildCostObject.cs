using UnityEngine;
using UnityEngine.UI;

public class ObjectBuildCostObject : MonoBehaviour
{
    public float resourceCost;
    public GameObject objectToInstanciate;
    [ReadOnly] public GameManager gameManager;
    private Button myButton;

    // Use this for initialization
    void Start()
    {
        gameManager = GameManager.myInstance;
        myButton = this.GetComponent<Button>();
        if (!myButton)
            myButton = gameObject.AddComponent<Button>();
        myButton.onClick.AddListener(delegate { gameManager.spawnObject(this); });
    }
    //OnMouseDown is called when the user clicks on the unit, thus toggling selection and changing the cursor...
    void OnMouseDown()
    {
        gameManager.guiManagerInstance.selectedUnit = this.gameObject;
    }

}
