using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor   : Editor {
    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        GameManager g = (GameManager)target;
            
        if (DrawDefaultInspector()){

        }
        
        if(GUILayout.Button("Build Terrain"))
        {
            g.mapManagerInstance.mapGen.generateTerrainObject();
        }
        if(GUILayout.Button("Start Game"))
        {
            g.mapManagerInstance.mapGen.gameStart();
        }
        if(GUILayout.Button("NavMesh Generation"))
        {
            g.mapManagerInstance.mapGen.generateNavMesh();
        }
    }
}

