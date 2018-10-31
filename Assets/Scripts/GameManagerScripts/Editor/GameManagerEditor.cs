using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    PlayerSelectOptionsGenerator.UIDropDownColor colorSlot = PlayerSelectOptionsGenerator.UIDropDownColor.Blue;
    PlayerSelectOptionsGenerator.UIDropDownFraction fractionSlot = PlayerSelectOptionsGenerator.UIDropDownFraction.Axis;
    int selectedPlayer = 0;
    GameObject cloneableObject = null;
    string playerName = "xXx_CorpseRat_xXx";
    int teamNmbr = 1;
    bool isGui;

    public override void OnInspectorGUI()
    {

        //base.OnInspectorGUI();
        GameManager g = (GameManager)target;

        if (DrawDefaultInspector())
        {

        }

        if (GUILayout.Button("Start Game"))
        {
            g.mapManagerInstance.mapGen.gameStart();
        }
        GUILayout.Space(10f);

        playerName = EditorGUILayout.TextField("Player Name", playerName);
        //Color playerColor = EditorGUILayout.ColorField("Color", Color.red);
        colorSlot = (PlayerSelectOptionsGenerator.UIDropDownColor)EditorGUILayout.EnumPopup("Color", colorSlot);
        fractionSlot = (PlayerSelectOptionsGenerator.UIDropDownFraction)EditorGUILayout.EnumPopup("Fraction", fractionSlot);
        teamNmbr = EditorGUILayout.IntField("Team", teamNmbr);
        isGui = EditorGUILayout.Toggle("Is GUI player", isGui);

        if (GUILayout.Button("Add new player"))
        {
            PlayerHandler p = new GameObject("PlayerHandlerObject").AddComponent<PlayerHandler>();
            p.playerName = playerName;
            p.playerColor = PlayerSelectOptionsGenerator.convertUIColorToColor((int)colorSlot);
            p.playerFraction = PlayerSelectOptionsGenerator.convertUIFractionToFraction((int)fractionSlot);
            p.isFrontendPlayer = isGui;
            if (isGui)
                isGui = false;
            g.addNewPlayer(p);
            p.SpawnPlayer();
        }

        GUILayout.Space(10f);

        List<string> x = new List<string>();
        if (GameManager.myInstance)
            foreach (PlayerHandler p in GameManager.myInstance.playersInGame)
            {
                x.Add(p.playerName);
            }
        selectedPlayer = EditorGUILayout.Popup("Player", selectedPlayer, x.ToArray());
        cloneableObject = EditorGUILayout.ObjectField("Prefab", cloneableObject, typeof(GameObject), true) as GameObject;

        if (GUILayout.Button("Instance new object from prefab"))
        {
            if (GameManager.myInstance)
                GameManager.myInstance.playersInGame[selectedPlayer].spawnObject(cloneableObject, new Vector3(100, 100, 100));
            //g.mapManagerInstance.mapGen.generateTerrainObject();
        }

        GUILayout.Space(10f);
        if (GUILayout.Button("Generate new terrain"))
        {
            g.mapManagerInstance.mapGen.generateTerrainObject();
        }
        if (GUILayout.Button("Generate NavMesh agents"))
        {
            g.mapManagerInstance.mapGen.generateNavMesh();
        }
    }
}

