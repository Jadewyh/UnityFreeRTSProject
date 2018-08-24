/*
Class(es): MenuGameSettings
Short description: Menu Handler for game settings

Initial author: Christian Kessner <SpiegelEiXXL>
Initial creation date: 20th AUG 2018
Initial name: MenuGameSettings.cs
Written for: Unity Free 2 Play RTS project
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGameSettings : MonoBehaviour
{
    public GameObject CameraWidthSlider;
    public GameObject PanelLeft;
    public GameObject PanelRight;
    public GameObject PanelTop;
    public GameObject PanelBottom;
    public bool _setVisiblity = true;

    // Use this for initialization
    void Start()
    {
        string val = GlobalGameSettings.gameSettingsInstance.getValue("RTSCameraBorderWidth");
        if (val != "")
            CameraWidthSlider.GetComponent<UnityEngine.UI.Slider>().value = float.Parse(val);
        refreshPanels();
    }

    public void takeOverGameSettings()
    {
        GlobalGameSettings.gameSettingsInstance.setValue("RTSCameraBorderWidth", CameraWidthSlider.GetComponent<UnityEngine.UI.Slider>().value.ToString());
    }

    public void refreshPanels()
    {
        PanelLeft.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CameraWidthSlider.GetComponent<UnityEngine.UI.Slider>().value);
        PanelRight.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, CameraWidthSlider.GetComponent<UnityEngine.UI.Slider>().value);
        PanelTop.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, CameraWidthSlider.GetComponent<UnityEngine.UI.Slider>().value);
        PanelBottom.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, CameraWidthSlider.GetComponent<UnityEngine.UI.Slider>().value);
    }

    private void OnDisable()
    {
        _setVisiblity = false;
        PanelLeft.SetActive(_setVisiblity);
        PanelBottom.SetActive(_setVisiblity);
        PanelTop.SetActive(_setVisiblity);
        PanelRight.SetActive(_setVisiblity);

    }

    private void OnEnable()
    {
        _setVisiblity = true;
        PanelLeft.SetActive(_setVisiblity);
        PanelBottom.SetActive(_setVisiblity);
        PanelTop.SetActive(_setVisiblity);
        PanelRight.SetActive(_setVisiblity);

    }

    // Update is called once per frame
    void Update()
    {

    }
}
