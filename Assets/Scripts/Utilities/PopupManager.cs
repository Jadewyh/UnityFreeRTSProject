/*
Class(es): PopupManager
Short description: Handles various types of popup

Initial author: Christian Kessner <SpiegelEiXXL>
Initial creation date: 20th AUG 2018
Initial name: MenuAudioSettings.cs
Written for: Unity Free 2 Play RTS project
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// TODO: Finish implementing
public class PopupManager : MonoBehaviour
{
    [ReadOnly] public GameObject myPopupCanvas;
    [ReadOnly] public GameObject myPopupCanvasGrid;
    [ReadOnly] public GameObject currentQuestionObject;
    [ReadOnly] public Canvas canvas;
    [ReadOnly] public GridLayoutGroup grid;

    [ReadOnly] public int currentCounter = 0;
    // Use this for initialization
    void Start()
    {
        myPopupCanvas = new GameObject("CanvasPopupManager");
        myPopupCanvas.AddComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
        myPopupCanvas.GetComponent<RectTransform>().localPosition.Set(0, 0, 0);
        myPopupCanvasGrid = new GameObject("CanvasPopupManagerGrid");
        canvas = myPopupCanvas.AddComponent<UnityEngine.Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 1;
        CanvasScaler c = myPopupCanvas.AddComponent<CanvasScaler>();
        c.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        grid = myPopupCanvasGrid.AddComponent<GridLayoutGroup>();
        grid.constraint = GridLayoutGroup.Constraint.Flexible;
        grid.cellSize = new Vector2(200, 20);
        myPopupCanvas.AddComponent<GraphicRaycaster>();
        myPopupCanvasGrid.transform.SetParent(myPopupCanvas.transform);
        myPopupCanvasGrid.GetComponent<RectTransform>().anchoredPosition = new Vector3(0, 0, 0);
		//myPopupCanvasGrid.transform.position = new Vector3(0f,0f,0f);
    }

    public void CleanUp()
    {
        foreach (CanvasRenderer r in myPopupCanvas.GetComponentsInChildren<CanvasRenderer>())
        {
            if (r && r.gameObject)
                GameObject.DestroyImmediate(r.gameObject);
        }
        if (currentQuestionObject)
            GameObject.DestroyImmediate(currentQuestionObject);
        //GameObject.DestroyImmediate(myPopupCanvas);
        myPopupCanvas.SetActive(true);
    }

    public GameObject addButton(string text, Transform parent = null)
    {
        GameObject gb1 = new GameObject("ButtonPopupManager" + currentCounter);
        gb1.AddComponent<RectTransform>();
        gb1.AddComponent<Button>();
        GameObject gb1t = new GameObject("ButtonPopupManagerText" + currentCounter);
        Text t1 = gb1t.AddComponent<Text>();
        currentCounter++;
        if (parent != null)
            gb1.transform.SetParent(parent);
        gb1t.transform.SetParent(gb1.transform);
        t1.text = text;
        t1.alignment = TextAnchor.MiddleCenter;
        t1.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
        return gb1;
    }

    public GameObject addText(string text){
        GameObject o = new GameObject("TextPopupManager"+currentCounter);
        Text t = o.AddComponent<Text>();
        t.text = text;
        t.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
        t.alignment = TextAnchor.MiddleCenter;
        currentCounter++;
        return o;
    }

    public void displayMessageBox(string text){
        displayMessageBox(text, "OK");
    }
    public void displayMessageBox(string text, string OKButton = "OK"){
        CleanUp();
        currentQuestionObject = new GameObject("CanvasPopupManagerPopup");
        currentQuestionObject.transform.SetParent(myPopupCanvas.transform);
        RectTransform r = currentQuestionObject.AddComponent<RectTransform>();
        r.pivot = new Vector2(0.5f, 0.5f);
        r.anchoredPosition = new Vector3(0, 0, 0);

        GridLayoutGroup g = currentQuestionObject.AddComponent<GridLayoutGroup>();
        g.constraint = GridLayoutGroup.Constraint.Flexible;
        g.cellSize = new Vector2(400, 100);
        currentQuestionObject.AddComponent<CanvasRenderer>();
        addText(text).transform.SetParent(currentQuestionObject.transform);
        addButton(OKButton, currentQuestionObject.transform).GetComponent<Button>().onClick.AddListener(() => { answer1func(); });
    }

    public void MakeChoiceBetweenTwo(string question, string answer1, string answer2)
    {
        CleanUp();
        currentQuestionObject = new GameObject("CanvasPopupManagerTextQuestion");
        currentQuestionObject.transform.SetParent(grid.gameObject.transform);
        currentQuestionObject.AddComponent<GridLayoutGroup>();
        currentQuestionObject.AddComponent<CanvasRenderer>();
        Text t = currentQuestionObject.AddComponent<Text>();
        t.text = question;
        t.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
        t.alignment = TextAnchor.MiddleCenter;
        //t.gameObject.AddComponent<RectTransform>();
        GameObject gb1 = addButton(answer1, currentQuestionObject.transform);
        GameObject gb2 = addButton(answer2, currentQuestionObject.transform);
        gb1.GetComponent<Button>().onClick.AddListener(() => { answer1func(); });
        gb2.GetComponent<Button>().onClick.AddListener(() => { answer2func(); });
    }
    public void answer1func()
    {
        Debug.Log("successfully chosen answer 1!");
        if (answer1Chosen != null)
        {
            answer1Chosen();
        }
        myPopupCanvas.SetActive(false);
    }
    public void answer2func()
    {
        Debug.Log("successfully chosen answer 2!");
        if (answer2Chosen != null)
        {
            answer2Chosen();
        }
        myPopupCanvas.SetActive(false);
    }


    public delegate void choosenAnswer();
    public event choosenAnswer answer1Chosen;
    public event choosenAnswer answer2Chosen;

    // Update is called once per frame
    void Update()
    {

    }
}
