

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class PlayerColorMaterialDispencer
{
    private static Dictionary<int, Material> matMap;
    private static Dictionary<string, Material> matMapName;
    private static List<Material> materials;


    static PlayerColorMaterialDispencer()
    {
        materials.AddRange(Resources.FindObjectsOfTypeAll<Material>());
        matMap = new Dictionary<int, Material>();
        matMapName = new Dictionary<string, Material>();
        foreach (Material m in materials)
        {
            int val = (int)System.Enum.Parse(typeof(PlayerSelectOptionsGenerator.UIDropDownColor), m.name, true);
            matMap.Add(val, m);
            matMapName.Add(m.name, m);
        }
    }

    static public Material GetMaterialFromColorIndex(string matName)
    {
        Material o = null;
        o = matMapName[matName];
        return o;
    }

    static public Material GetMaterialFromColorIndex(int idx)
    {
        return matMap[idx];
    }
}
