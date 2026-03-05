using System;
using System.Collections;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(TileEditerMgr))]
public class TileEditerInspector : Editor
{

    static TileEditerInspector()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TileEditerMgr myScript = (TileEditerMgr)target;
        if (GUILayout.Button("Sort"))
        {
            myScript.InitTileList();

        }
    }

    private static void OnSceneGUI(SceneView sceneView)
    {
        Event e = Event.current;

        if (e.type == EventType.MouseDown &&e.button == 0) // 左键点击
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null)
            {
                TileEditerCell script = hit.collider.gameObject.GetComponent<TileEditerCell>();
                script.tileEditerMgr.OnClick(hit.collider.gameObject,0);

                //Debug.Log("编辑器模式下点击到了物体: " + hit.collider.gameObject.name);
            }
            else
            {
                //Debug.Log("编辑器模式下点击到了空地！");
            }
        }
    }

}
