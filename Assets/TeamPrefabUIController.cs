using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamPrefabUIController : MonoBehaviour
{
    [ReadOnly] public GameObject mySlotDropDown;
    [ReadOnly] public GameObject myColorDropDown;
    [ReadOnly] public GameObject myFractionDropDown;
    [ReadOnly] public GameObject myTeamDropDown;

    // Use this for initialization
    void Start()
    {
        foreach (Dropdown d in this.GetComponentsInChildren<Dropdown>())
        {
            switch (d.gameObject.name)
            {
                case "SlotDropDown":
                    mySlotDropDown = d.gameObject;
                    break;
                case "ColorDropDown":
                    myColorDropDown = d.gameObject;
                    break;
                case "FractionDropDown":
                    myFractionDropDown = d.gameObject;
                    break;
                case "TeamDropDown":
                    myTeamDropDown = d.gameObject;
                    break;
            }
        }

    }

	public void controlActiveFields(){
		if (mySlotDropDown.GetComponent<Dropdown>().value == 2){
			myColorDropDown.SetActive(false);
			myFractionDropDown.SetActive(false);
			myTeamDropDown.SetActive(false);
		} else {
			myColorDropDown.SetActive(true);
			myFractionDropDown.SetActive(true);
			myTeamDropDown.SetActive(true);

		}
	}

    // Update is called once per frame
    void Update()
    {

    }
}
