using UnityEditor;
using UnityEngine;
namespace UILibEditor.Export
{
    class UIEditorController
    {
    	////[MenuItem("Assets/UI/CopyToResource_AssetBundle")]
     //   static public void CopyToUI()
     //   {
     //       UICodeCreator.CopyToUI();
     //   }


        [MenuItem("Assets/Create UICode")]
        static public void CreateCode()
        {
            GameObject go = Selection.activeObject as GameObject;
            UICodeCreator.CreateCode(go);
        }

        ////[MenuItem("Assets/UI/Create All UICode")]
        //static public void CreateAllCode()
        //{
        //    Debug.Log(Directory.GetCurrentDirectory());
        //    nIndex = 0;
        //    string searchFolder = "Assets/Res_AssetBundle/UI/";
        //    if (!Directory.Exists(searchFolder))
        //        return;

        //    string[] dirs = Directory.GetDirectories(searchFolder);
        //    foreach (string oneDir in dirs)
        //    {
        //        Debug.Log(oneDir);
        //        _CreateAllCode(oneDir);
        //    }
        //}
        //static int nIndex = 0;

        //static public void _CreateAllCode(string searchFolder)
        //{
        //    Debug.Log(string.Format("{0}", nIndex++));
        //    string srcDir = StandardlizePath(searchFolder);
        //    string srcFile = Path.Combine(srcDir, string.Format("{0}.prefab", Path.GetFileName(srcDir)));
        //    srcFile = StandardlizePath(srcFile);
        //    UnityEngine.Object obj = AssetDatabase.LoadMainAssetAtPath(srcFile);
        //    GameObject go = obj as GameObject;
        //    UICodeCreator.CreateCode(go);

        //    string[] dirs = Directory.GetDirectories(searchFolder);
        //    foreach (string oneDir in dirs)
        //    {
        //        _CreateAllCode(oneDir);
        //    }
        //}

        ////[MenuItem("UGUI/Lua/Create UICode")]
        //static public void CreateLuaCode()
        //{
        //    GameObject go = Selection.activeObject as GameObject;
        //    UICodeCreator.CreateCode(go, true);
        //}

        public static string StandardlizePath(string path)
        {
            string pathReplace = path.Replace(@"\", @"/");
            string pathLower = pathReplace.ToLower();
            return pathLower;
        }
    }
}
