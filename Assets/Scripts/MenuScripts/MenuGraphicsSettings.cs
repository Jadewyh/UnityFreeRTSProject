/*
Class(es): MenuGraphicsSettings
Short description: Menu Handler for graphics settings

Initial author: Christian Kessner <SpiegelEiXXL>
Initial creation date: 20th AUG 2018
Initial name: MenuGraphicsSettings.cs
Written for: Unity Free 2 Play RTS project
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class MenuGraphicsSettings : MonoBehaviour
{
    public GameObject qualityDropDown;
    public GameObject resolutionMode;
    public GameObject fullscreenToggle;
	public Dictionary<int, Resolution> resMap = new Dictionary<int,Resolution>();

    // Use this for initialization
    void Start()
    {

        List<string> l = new List<string>();
		int i = 0;
        foreach (Resolution r in Screen.resolutions)
        {
            l.Add(r.ToString());
			resMap.Add(i,r);
			i++;
        }
        setDropdown(qualityDropDown.GetComponentInChildren<Dropdown>(), QualitySettings.names, QualitySettings.names[QualitySettings.GetQualityLevel()]);
        //setDropdown(resolutionMode.GetComponentInChildren<Dropdown>(), System.Enum.GetNames(typeof(resolutionMode)), l.ToArray());
        setDropdown(resolutionMode.GetComponentInChildren<Dropdown>(), l.ToArray(), Screen.currentResolution.ToString());
        fullscreenToggle.GetComponent<Toggle>().isOn = Screen.fullScreen;

    }

    public void setDropdown(Dropdown d, string[] strings, string def)
    {
        d.options.Clear();
        int i = 0;
        foreach (string str in strings)
        {
            d.options.Add(new Dropdown.OptionData(str));
            d.value = i;
            i++;
        }
    }

	public void takeOverGraphicSettings(){
		//Screen.fullScreen = fullscreenToggle.GetComponent<Toggle>().isOn;
		Resolution r = resMap[resolutionMode.GetComponentInChildren<Dropdown>().value];
		Screen.SetResolution(r.width, r.height, fullscreenToggle.GetComponent<Toggle>().isOn, r.refreshRate);
	}

    // Update is called once per frame
    void Update()
    {

    }
}
