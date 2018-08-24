/*
Class(es): MenuManager
Short description: Master over all menus, controls transitions between menus and scenes.

Initial author: Christian Kessner <SpiegelEiXXL>
Initial creation date: 20th AUG 2018
Initial name: MenuManager.cs
Written for: Unity Free 2 Play RTS project
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public Dictionary<string, GameObject> menuObjects = new Dictionary<string, GameObject>();
    public GameObject currentMenu;
    // Use this for initialization
    void Start()
    {
        PrepareDict();
        GoToMenu("MenuMain");
    }
    void PrepareDict()
    {
        foreach (GameObject o in Resources.FindObjectsOfTypeAll<GameObject>())
        {

            if (!menuObjects.ContainsKey(o.name))
                menuObjects.Add(o.name, o);
            else
            {
                while (menuObjects.ContainsKey(o.name))
                {
                    Debug.Log("Double entry for gameObject " + o.name + " - consider changing it's name in the editor!");
                    o.name = o.name + "_dup";
                }
                menuObjects.Add(o.name, o);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void GoToMenu(string MenuName)
    {
        if (!menuObjects.ContainsKey(MenuName))
        {
            Debug.Log("The called menu '" + MenuName + "' does not exist!");
            return;
        }
        GameObject g = menuObjects[MenuName];
        if (g)
        {
            if (currentMenu)
                currentMenu.SetActive(false);
            g.SetActive(true);
            currentMenu = g;
        }
    }

    public void GoToScene(string sceneName){
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void quitGame()
    {
        PopupManager p = this.gameObject.GetComponent<PopupManager>();
        if (!p)
            p = this.gameObject.AddComponent<PopupManager>();

        p.answer1Chosen += quitGameReally;
        p.MakeChoiceBetweenTwo("Do you really want to quit the game?","Yes","No");

    }
    public void quitGameReally()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
