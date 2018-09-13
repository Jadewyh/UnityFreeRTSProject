using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor {
    public override void OnInspectorGUI() {
        //base.OnInspectorGUI();
        DrawDefaultInspector();
        /*MapGenerator myScript = (MapGenerator)target;
            
        if (DrawDefaultInspector()){
            if (myScript.autoUpdateInUnity)
                    myScript.generateTerrainObject();            
        }
        
        if(GUILayout.Button("Build Terrain"))
        {
            myScript.generateTerrainObject();
        }
        if(GUILayout.Button("Start Game"))
        {
            myScript.gameStart();
        }
        if(GUILayout.Button("NavMesh Generation"))
        {
            myScript.generateNavMesh();
        }*/
    }
}
