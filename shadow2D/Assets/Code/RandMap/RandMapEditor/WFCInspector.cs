using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(WFCCreator))]
public class WFCInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        WFCCreator myScript = (WFCCreator)target;
        if (GUILayout.Button("CreateTilemap"))
        {
            myScript.CreateNewMap();

        }
        if (GUILayout.Button("SaveTilemap"))
        {
            myScript.SaveTilemap();
        }


        if (GUILayout.Button("DebugCreate"))
        {
            myScript.TestCreate();
        }
        if (GUILayout.Button("DebugTick"))
        {
            myScript.TestTick();
        }

    }
}
