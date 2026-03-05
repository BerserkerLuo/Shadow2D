using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RandMap;

[CustomEditor(typeof(RandMapCreator))]
public class RandMapInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RandMapCreator myScript = (RandMapCreator)target;
        if (GUILayout.Button("Createmap"))
        {
            myScript.GenerateMap();

        }
        if (GUILayout.Button("Savemap"))
        {
            //myScript.SaveTilemap();
        }

    }
}
