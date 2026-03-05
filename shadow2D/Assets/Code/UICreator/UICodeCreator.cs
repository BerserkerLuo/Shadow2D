using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using Client.UI.UICommon;
using System;
using UILibEditor.Export;
using UILib;
using System.Text.RegularExpressions;

class UICodeCreator
{
    //static public void CopyToUI()
    //{
    //    UnityEngine.Object selection = Selection.activeGameObject;
    //    if (null == selection)
    //    {
    //        return;
    //    }
    //    if (PrefabType.Prefab != PrefabUtility.GetPrefabType(selection))
    //    {
    //        Debug.Log("Not UI prefab");
    //        return;
    //    }

    //    string strPath = AssetDatabase.GetAssetPath(selection);

    //    if (strPath.IndexOf("Assets/Res/UI") == 0)
    //    {
    //        string strFileNew = strPath.Replace("Assets/Res/UI", "Assets/Resource_AssetBundle/UI");
    //        string strDirectory = strFileNew.Substring(0, strFileNew.LastIndexOf("/"));
    //        if (Directory.Exists(strDirectory) == false)
    //        {
    //            Directory.CreateDirectory(strDirectory);
    //        }
    //        File.Copy(strPath, strFileNew, true);
    //    }
    //}
	
    static public void CreateCode(GameObject go, bool isLua = false)
    {
        if (null == go)
        {
            return;
        }

        Debug.Log("CreateCode "+ go.name);

        if (go.name.IndexOf("Dlg") != 0)
        {
            Debug.LogError("go.name.IndexOf(\"Dlg\") != 0");
            return;
        }
        string rootDir = "Resources/UI";

        string strPath = AssetDatabase.GetAssetPath(go);

        Debug.Log("CreateCode2 strPath " + strPath);

        strPath = strPath.Replace(@"\", @"/");

        int nIndex = strPath.IndexOf(rootDir);
        if (nIndex == -1)
        {
            Debug.LogError($"not in {rootDir}");
            return;
        }
        strPath = strPath.Substring(nIndex+ rootDir.Length+1);
        nIndex = strPath.IndexOf("/");
        string strAbName = strPath.Substring(0, nIndex);

        string strFileName = strPath.Substring(nIndex + 1);
        strFileName = Path.Combine(Path.GetDirectoryName(strFileName), Path.GetFileNameWithoutExtension(strFileName));
        strFileName = strFileName.Replace(@"\", @"/");

        Debug.Log("CreateCode3 strAbName " + strAbName);

        string strFileDir = Application.dataPath + "/Code/Game/GameState/UICode/";
        if (strAbName.IndexOf("State") > 0)
        {
            nIndex = strPath.LastIndexOf("/");
            strFileDir = Application.dataPath + string.Format("/Code/Game/GameState/UICode/{0}/", strPath.Substring(0, nIndex));
            strFileDir = strFileDir.Replace(@"\", @"/");
            if (false == System.IO.Directory.Exists(strFileDir))
            {
                System.IO.Directory.CreateDirectory(strFileDir);
            }
        }


        PrefabType prefabType = PrefabUtility.GetPrefabType(go);
		bool bNeedDestory = false;
		if (prefabType == PrefabType.Prefab)
		{
			go = PrefabUtility.InstantiatePrefab(go) as GameObject;
			bNeedDestory = true;
		}

        //FindAllWidgets(go);
		s_dicPath2WidgetCached = new Dictionary<string, XUIObjectBase>();
		FindAllWidgets(go.transform, "");
        //if (isLua)
        //{
        //    CreateLuaCodeForDlg(go);
        //    CreateLuaCodeForDlgBehaviour(go);
        //}
        //else
        {
            CreateCodeForDlg(go, strAbName, strFileDir);
            CreateCodeForDlgBehaviour(go, strAbName, strFileName, strFileDir);
        }
		
		if (true == bNeedDestory)
		{
			GameObject.DestroyImmediate(go);
		}
    }

    static void CreateCodeForDlg(GameObject objPanel, string strAbName, string strFileDir)
    {
        string strDlgName = objPanel.name;
        string strFilePath = string.Format("{0}/{1}.cs", strFileDir, strDlgName);
        strFilePath = strFilePath.Replace(@"\", @"/");

        
        if (File.Exists(strFilePath) == true)
        {
            return;
        }

        Debug.Log(strFilePath);

        StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);

        //扩展
        List<string> listDotweenName = new List<string>();  //dotween
        bool bHasListMoney = false;
        bool bHasListMenu = false;                          //listMenu
        bool bHasListGroup = false;

        foreach (KeyValuePair<string, XUIObjectBase> pair in s_dicPath2WidgetCached)
        {
            XUIObjectBase widget = pair.Value;
            if (widget.name.Contains("Dotween"))
            {
                string[] resultString = Regex.Split(widget.name, "Dotween");
                string dotweenName = resultString[1];
                listDotweenName.Add(dotweenName);
            }

            if (widget.name.Contains("ListMoney"))
            {
                bHasListMoney = true;
            }

            if (widget.name.Contains("ListMenu"))
            {
                bHasListMenu = true;
            }

            if (widget.name.Contains("ListGroup"))
            {
                bHasListGroup = true;
            }
        }

        StringBuilder strBuilder = new StringBuilder();
        strBuilder.AppendLine("using System;")
		          .AppendLine("using UnityEngine;")
		          .AppendLine("using Client.UI.UICommon;")
                  .AppendLine("using DG.Tweening;")
                  .AppendLine("using System.Collections.Generic;")
                  .AppendLine("using UnityEngine.UI;")
                  .AppendLine("using UILib;")
                  .AppendLine()
                  .AppendLine("namespace Client.UI")
                  .AppendLine("{");

        if (bHasListMenu == true)
        {
            strBuilder.AppendFormat("\tpublic enum Enum{0}Menu\r\n", strDlgName.Substring(3));
            strBuilder.AppendLine("\t{");
            strBuilder.AppendLine("\t}");
            strBuilder.AppendLine();
        }

        strBuilder.AppendFormat("\tpublic class {0} : DlgBaseNew\r\n", strDlgName);
          strBuilder.AppendLine("\t{");

        strBuilder.AppendFormat("\t\t public static {0} singleton\r\n", strDlgName);
          strBuilder.AppendLine("\t\t{");
            strBuilder.AppendLine("\t\t\tget");
            strBuilder.AppendLine("\t\t\t{");
            strBuilder.AppendLine("\t\t\t\tif (s_singleton == null)");
            strBuilder.AppendLine("\t\t\t\t{");
            strBuilder.AppendFormat("\t\t\t\t\ts_singleton = new {0}();\r\n", strDlgName);
              strBuilder.AppendLine("\t\t\t\t\tUIManager.Singleton.AddDlg(s_singleton);");
            strBuilder.AppendLine("\t\t\t\t}");
            strBuilder.AppendLine("\t\t\t\treturn s_singleton;");
            strBuilder.AppendLine("\t\t\t}");
          strBuilder.AppendLine("\t\t}");

        strBuilder.AppendFormat("\t\t private static {0} s_singleton = null;\r\n", strDlgName);
        strBuilder.AppendLine();

        if (strAbName == "StateLogin")
        {
            strBuilder.AppendLine("\t\tpublic override uint Type => (int)EnumDlgType.eDlgType_Login;")
            .AppendLine();
        }
            
        if (strAbName == "StateRoom")
        {
            strBuilder.AppendLine("\t\tpublic override uint Type => (int)EnumDlgType.eDlgType_Room;");
        }

        if (strAbName == "StateBattle_AB")
        {
            strBuilder.AppendLine("\t\tpublic override uint Type => (int)EnumDlgType.eDlgType_Battle;");
        }

        if (strAbName == "StateAll")
        {
            strBuilder.AppendLine("\t\tpublic override uint Type => (int)EnumDlgType.eDlgType_AllState;");
        }

        strBuilder.AppendLine();
        strBuilder.AppendFormat("\t\tpublic {0}Behaviour uiBehaviour\r\n", strDlgName);
          strBuilder.AppendLine("\t\t{");
          strBuilder.AppendLine("\t\t\tget");
          strBuilder.AppendLine("\t\t\t{");
        strBuilder.AppendFormat("\t\t\t\treturn ({0}Behaviour)m_uiBehaviour;\r\n", strDlgName);
         strBuilder.AppendLine("\t\t\t}");
         strBuilder.AppendLine("\t\t}");
        strBuilder.AppendLine();


        strBuilder.AppendFormat("\t\tpublic {0}()\r\n",strDlgName);
          strBuilder.AppendLine("\t\t{");
        strBuilder.AppendFormat("\t\t\tm_uiBehaviour = new {0}Behaviour();\r\n", strDlgName);
          strBuilder.AppendLine("\t\t}");
        strBuilder.AppendLine();

        strBuilder.AppendLine("\t\tpublic override void Init()")
                  .AppendLine("\t\t{");

        if (bHasListMoney == true)
        {
            strBuilder.AppendLine("\t\t\tSetMoneyList();");
        }

        if (bHasListMenu == true)
        {
            strBuilder.AppendLine("\t\t\tSetMenuList();");
        }
        
        strBuilder.AppendLine("\t\t}")
                  .AppendLine();

        strBuilder.AppendLine("\t\tpublic override void Reset()")
                  .AppendLine("\t\t{")
                  .AppendLine("\t\t}")
                  .AppendLine();

        strBuilder.AppendLine("\t\tpublic override void RegisterEvent()")
                  .AppendLine("\t\t{");
        CreateUIEventRegCode(ref strBuilder);
        strBuilder.AppendLine("\t\t}\r\n");

        CreateUIEventFunCode(ref strBuilder);

        if (bHasListMoney == true)
        {
            CreateUIEventListMoneyCode(ref strBuilder);
        }

        if (bHasListMenu == true)
        {
            CreateUIEventListMenuCode(ref strBuilder, strDlgName, bHasListGroup);
        }

        if (bHasListGroup == true)
        {
            CreateUIEventListGroupCode(ref strBuilder);
        }

        strBuilder.AppendLine("\t\tprotected override void OnShow()")
                  .AppendLine("\t\t{")
                  .AppendLine("\t\t\tbase.OnShow();");
                  

        if (listDotweenName.Count > 0)
        {
            strBuilder.AppendLine("\t\t\tInitDotween();");
        }

        strBuilder.AppendLine("\t\t\tRefresh();")
                  .AppendLine("\t\t}")
                  .AppendLine();

        strBuilder.AppendLine("\t\tprotected override void OnRefresh()")
                  .AppendLine("\t\t{")
                  .AppendLine("\t\t\tbase.OnRefresh();")
                  .AppendLine("\t\t}")
                  .AppendLine();

        if (bHasListMoney == true)
        {
            strBuilder.AppendLine("\t\tprivate List<EnumMoneyType> m_listEnumMoney = new List<EnumMoneyType>();");
        }

        if (bHasListMenu == true)
        {
            strBuilder.AppendFormat("\t\tprivate List<Enum{0}Menu> m_listEnumMenu = new List<Enum{0}Menu>();", strDlgName.Substring(3))
                      .AppendLine();
            strBuilder.AppendFormat("\t\tprivate Enum{0}Menu m_selectMenu = 0;", strDlgName.Substring(3))
                      .AppendLine();
        }

        if (bHasListGroup == true)
        {
            strBuilder.AppendLine("\t\tprivate int m_selectGroup = 0;")
                      .AppendLine();
        }

        for (int i = 0; i < listDotweenName.Count; i++)
        {
            strBuilder.AppendLine("\t\tprivate DOTweenAnimation[] m_dot" + listDotweenName[i] + "; ");
        }
        strBuilder.AppendLine("\t}")
                  .AppendLine("}");

        sw.Write(strBuilder);
        sw.Flush();
        sw.Close();
    }

    static void CreateCodeForDlgBehaviour(GameObject objPanel, string strAbName, string strFileName, string strFileDir)
    {
        if (null == objPanel)
        {
            return;
        }
        string strDlgName = objPanel.name;
        string strFilePath = string.Format("{0}/{1}Behaviour.cs", strFileDir, strDlgName);

        StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);

        StringBuilder strBuilder = new StringBuilder();
        Debug.Log(strFilePath);
        strBuilder.AppendLine()
            .AppendLine("using UnityEngine;")
            .AppendLine("using Client.UI.UICommon;")
            .AppendLine("using Client;")
            .AppendLine("using UILib.Export;");
        strBuilder.AppendFormat("public class {0}Behaviour : DlgBehaviourBase\r\n", strDlgName)
            .AppendLine("{");

        strBuilder.AppendLine("\tpublic override string AbName")
            .AppendLine("\t{");
        strBuilder.AppendFormat("\t\tget {{  return \"{0}\"; }}\r\n", strAbName);
        strBuilder.AppendLine("\t}")
            .AppendLine();

        strBuilder.AppendLine("\tpublic override string FileName")
          .AppendLine("\t{");
        strBuilder.AppendFormat("\t\tget {{  return \"{0}\"; }}\r\n", strFileName);
        strBuilder.AppendLine("\t}")
          .AppendLine();

        strBuilder.AppendLine("\tpublic override void Init()")
            		.AppendLine("\t{")
					.AppendLine("\t\tbase.Init();");
        CreateWidgetBindCode(ref strBuilder, objPanel.transform);
        strBuilder.AppendLine("\t}");

        CreateWidgetsDeclareCode(ref strBuilder);
        strBuilder.AppendLine("}");
        sw.Write(strBuilder);
        sw.Flush();
        sw.Close();
    }


    static void CreateWidgetBindCode(ref StringBuilder strBuilder, Transform transRoot)
    { 
        foreach (KeyValuePair<string, XUIObjectBase> pair in s_dicPath2WidgetCached)
        {
            XUIObjectBase widget = pair.Value;
			string strPath = GetWidgetPath(widget.transform, transRoot);
			string strClassType = widget.GetType().ToString();
			string strInterfaceType = "";
			if (s_dicWidgetInterface.TryGetValue(strClassType, out strInterfaceType) == false)
			{
				Debug.LogError("s_dicWidgetInterface.TryGetValue(strClassType, out strInterfaceType) == false:" + strClassType);
			}
            strBuilder.AppendFormat("		m_{0} = GetUIObject(\"{1}\") as {2};\r\n", widget.name, strPath, strInterfaceType);

            strBuilder.AppendFormat("		if (null == m_{0})\r\n", widget.name);
              strBuilder.AppendLine("     	{");
            strBuilder.AppendFormat("           Debug.Log(\"{0} is null!\");\r\n", strPath);
            strBuilder.AppendFormat("		    m_{0} = WidgetFactory.CreateWidget<{1}>();\r\n", widget.name, strInterfaceType);
              strBuilder.AppendLine("     	}");
        }
    }
    
    static void CreateUIEventListMenuCode(ref StringBuilder strBuilder, string strDlgName, bool bHasGroup)
    {
        strBuilder.AppendLine("\t\tprivate void SetMenuList()")
                  .AppendLine("\t\t{")
                  .AppendLine("\t\t\tm_listEnumMenu.Clear();")
                  .AppendLine("\t\t\tShowMenu();")
                  .AppendLine("\t\t}")
                  .AppendLine();

        strBuilder.AppendLine("\t\tprivate void ShowMenu()")
              .AppendLine("\t\t{")
              .AppendLine("\t\t\tuiBehaviour.m_ListMenu.Clear();")
              .AppendLine()
              .AppendLine("\t\t\tfor (int i = 0; i < m_listEnumMenu.Count; i++)")
              .AppendLine("\t\t\t{")
              .AppendLine("\t\t\t\tIXUIListItem uiListItem = uiBehaviour.m_ListMenu.GetItemByIndexOrAdd(i);")
              .AppendLine("\t\t\t\tuiListItem.SetVisible(true);")
              .AppendLine("\t\t\t\tuiListItem.id = (uint)m_listEnumMenu[i];")
              .AppendLine("\t\t\t\tuiListItem.RegisterClickEventHandler(this.OnListMenuItemClick);")
              .AppendLine()
              .AppendLine("\t\t\t\tIXUISprite ImageIcon = uiListItem.GetUIObject(\"ImageIcon\") as IXUISprite;")
              .AppendLine("\t\t\t\tif (null != ImageIcon)")
              .AppendLine("\t\t\t\t{")
              .AppendLine("\t\t\t\t\tImageIcon.SetSprite(\"ui/common/UI\", string.Format(\"Menu/{0}_up\", uiListItem.id));")
              .AppendLine("\t\t\t\t}")
              .AppendLine()
              .AppendLine("\t\t\t\tstring strMenu = \"\";")
              .AppendLine("\t\t\t\tuiListItem.SetLabelText(\"TextName\", strMenu);")
              .AppendLine("\t\t\t}")
              .AppendLine("\t\t}")
              .AppendLine();

        strBuilder.AppendLine("\t\tpublic void Show()")
                  .AppendLine("\t\t{")
                  .AppendLine("\t\t\tSetVisible(true);")
                  .AppendLine("\t\t\tSelectMenu(m_listEnumMenu[0]);")
                  .AppendLine("\t\t}")
                  .AppendLine();
       
        strBuilder.AppendFormat("\t\tpublic void ShowByMenu(Enum{0}Menu eMenu)", strDlgName.Substring(3))
                  .AppendLine()
                  .AppendLine("\t\t{")
                  .AppendLine("\t\t\tSetVisible(true);")
                  .AppendLine("\t\t\tSelectMenu(eMenu);")
                  .AppendLine("\t\t}")
                  .AppendLine();

        strBuilder.AppendFormat("\t\tprivate void SelectMenu(Enum{0}Menu eMenu)", strDlgName.Substring(3))
                  .AppendLine()
                  .AppendLine("\t\t{")
                  .AppendLine("\t\t\tOnListMenuItemClick(uiBehaviour.m_ListMenu.GetItemById((uint)eMenu));")
                  .AppendLine()
                  .AppendLine("\t\t\tToggle toggle = uiBehaviour.m_ListMenu.GetItemById((uint)eMenu).CachedTransform.GetComponent<Toggle>();")
                  .AppendLine("\t\t\tif (toggle != null)")
                  .AppendLine("\t\t\t{")
                  .AppendLine("\t\t\t\ttoggle.isOn = true;")
                  .AppendLine("\t\t\t}")
                  .AppendLine("\t\t}")
                  .AppendLine();

        strBuilder.AppendLine("\t\tprivate bool OnListMenuItemClick(IXUIObject uiObject)")
                  .AppendLine("\t\t{")
                  .AppendLine("\t\t\tIXUIListItem item = uiObject as IXUIListItem;")
                  .AppendFormat("\t\t\tm_selectMenu = (Enum{0}Menu)item.id;", strDlgName.Substring(3))
                  .AppendLine()
                  .AppendLine("\t\t\tShowMenuPanel();")
                  .AppendLine();

        if (bHasGroup == true)
        {
            strBuilder.AppendLine("\t\t\tShowGroup();");
        }

        strBuilder.AppendLine()
                  .AppendLine("\t\t\treturn true;")
                  .AppendLine("\t\t}")
                  .AppendLine();

        strBuilder.AppendLine("\t\tprivate void ShowMenuPanel()")
                  .AppendLine("\t\t{")
                  .AppendLine("\t\t}")
                  .AppendLine();
    }

    static void CreateUIEventListGroupCode(ref StringBuilder strBuilder)
    {
        strBuilder.AppendLine("\t\tprivate void ShowGroup()")
                  .AppendLine("\t\t{")
                  .AppendLine("\t\t\tuiBehaviour.m_ListGroup.Clear();")
                  .AppendLine()
                  .AppendLine("\t\t\tList<int> listGroup = new List<int>();")
                  .AppendLine("\t\t\tfor (int i = 0; i < listGroup.Count; i++)")
                  .AppendLine("\t\t\t{")
                  .AppendLine("\t\t\t\tint groupId = listGroup[i];")
                  .AppendLine("\t\t\t\tstring groupName =\"\";")
                  .AppendLine("\t\t\t\tIXUIListItem uiListItem = uiBehaviour.m_ListGroup.GetItemByIndexOrAdd(i);")
                  .AppendLine("\t\t\t\tuiListItem.SetVisible(true);")
                  .AppendLine("\t\t\t\tuiListItem.id = (uint)listGroup[i];")
                  .AppendLine("\t\t\t\tuiListItem.SetLabelText(\"TextName\", groupName);")
                  .AppendLine("\t\t\t\tuiListItem.RegisterClickEventHandler(this.OnListGroupItemClick);")
                  .AppendLine("\t\t\t}")
                  .AppendLine("\t\t}")
                  .AppendLine();

        strBuilder.AppendLine("\t\tprivate bool OnListGroupItemClick(IXUIObject uiObject)")
                  .AppendLine("\t\t{")
                  .AppendLine("\t\t\tIXUIListItem item = uiObject as IXUIListItem;")
                  .AppendLine("\t\t\tm_selectGroup = (int)item.id;")
                  .AppendLine("\t\t\treturn true;")
                  .AppendLine("\t\t}")
                  .AppendLine();
    }

    static void CreateUIEventListMoneyCode(ref StringBuilder strBuilder)
    {
        strBuilder.AppendLine("\t\tprivate void SetMoneyList()")
                  .AppendLine("\t\t{")
                  .AppendLine("\t\t\tm_listEnumMoney.Clear();")
                  .AppendLine("\t\t\tShowMoney();")
                  .AppendLine("\t\t}")
                  .AppendLine();

        strBuilder.AppendLine("\t\tprivate void ShowMoney()")
              .AppendLine("\t\t{")
              .AppendLine("\t\t\tuiBehaviour.m_ListMoney.Clear();")
              .AppendLine("\t\t\tfor (int i = 0; i < m_listEnumMoney.Count; i++)")
              .AppendLine("\t\t\t{")
              .AppendLine("\t\t\t\tint value = AccountMgr.singleton.GetMoneyValue(m_listEnumMoney[i]);")
              .AppendLine()
              .AppendLine("\t\t\t\tIXUIListItem uiListItem = uiBehaviour.m_ListMoney.GetItemByIndexOrAdd(i);")
              .AppendLine("\t\t\t\tuiListItem.SetVisible(true);")
              .AppendLine("\t\t\t\tuiListItem.id = (uint)m_listEnumMoney[i];")
              .AppendLine()
              .AppendLine("\t\t\t\tIXUISprite ImageIcon = uiListItem.GetUIObject(\"ImageIcon\") as IXUISprite;")
              .AppendLine("\t\t\t\tif (null != ImageIcon)")
              .AppendLine("\t\t\t\t{")
              .AppendLine("\t\t\t\t\tImageIcon.SetSprite(\"ui/common/Icon\", string.Format(\"Money/money_{0}\", uiListItem.id));")
              .AppendLine("\t\t\t\t}")
              .AppendLine()
              .AppendLine("\t\t\t\tuiListItem.SetLabelText(\"TextNum\", value.ToString());")
              .AppendLine("\t\t\t}")
              .AppendLine("\t\t}")
              .AppendLine();
    }

    static void CreateUIEventRegCode(ref StringBuilder strBuilder)
    {
        foreach (KeyValuePair<string, XUIObjectBase> pair in s_dicPath2WidgetCached)
        {
            XUIObjectBase widget = pair.Value;
            string strClassType = widget.GetType().ToString();
            string strInterfaceType = "";
            if (s_dicWidgetInterface.TryGetValue(strClassType, out strInterfaceType) == false)
            {
                Debug.LogError("s_dicWidgetInterface.TryGetValue(strClassType, out strInterfaceType) == false:" + strClassType);
            }

            if (strInterfaceType == "IXUIButton")
            {
                strBuilder.AppendFormat("\t\t\tuiBehaviour.m_{0}.RegisterClickEventHandler(this.On{0}Click);\r\n", widget.name);
            }

            if (strInterfaceType == "IXUICheckBox")
            {
                strBuilder.AppendFormat("\t\t\tuiBehaviour.m_{0}.RegisterOnCheckEventHandler(this.On{0}Click);\r\n", widget.name);
            }

            if (strInterfaceType == "IXUIInput")
            {
                strBuilder.AppendFormat("\t\t\tuiBehaviour.m_{0}.RegisterOnValueChanged(this.On{0}ValueChanged);\r\n", widget.name);
            }

            if (strInterfaceType == "IXUIPopupList")
            {
                strBuilder.AppendFormat("\t\t\tuiBehaviour.m_{0}.RegisterPopupListSelectEventHandler(this.On{0}ValueChanged);\r\n", widget.name);
            }  
        }
    }

    static void CreateUIEventFunCode(ref StringBuilder strBuilder)
    {
        List<string> listDotweenName = new List<string>();

        foreach (KeyValuePair<string, XUIObjectBase> pair in s_dicPath2WidgetCached)
        {
            XUIObjectBase widget = pair.Value;
            string strClassType = widget.GetType().ToString();
            string strInterfaceType = "";
            if (s_dicWidgetInterface.TryGetValue(strClassType, out strInterfaceType) == false)
            {
                Debug.LogError("s_dicWidgetInterface.TryGetValue(strClassType, out strInterfaceType) == false:" + strClassType);
            }

            if (strInterfaceType == "IXUIButton")
            {
                strBuilder.AppendFormat("\t\tprivate bool On{0}Click(IXUIObject uiObject)", widget.name);
                strBuilder.AppendLine("\r\n\t\t{");
                if (widget.name.Contains("ButtonReturn") || widget.name.Contains("ButtonClose"))
                {
                    strBuilder.AppendLine("\t\t\tSetVisible(false);");
                }
                strBuilder.AppendLine("\t\t\treturn true;");
                strBuilder.AppendLine("\t\t}");
                strBuilder.AppendLine();
            }

            if (strInterfaceType == "IXUICheckBox")
            {
                strBuilder.AppendFormat("\t\tprivate bool On{0}Click(IXUIObject uiObject)", widget.name);
                strBuilder.AppendLine("\r\n\t\t{");
                strBuilder.AppendLine("\t\t\tIXUICheckBox uiCheckBox = uiObject as IXUICheckBox;");
                strBuilder.AppendLine("\t\t\tDebug.Log(uiCheckBox.bChecked);");
                strBuilder.AppendLine("\t\t\treturn true;");
                strBuilder.AppendLine("\t\t}");
                strBuilder.AppendLine();
            }

            if (strInterfaceType == "IXUIInput")
            {
                strBuilder.AppendFormat("\t\tprivate bool On{0}ValueChanged(IXUIObject uiObject)", widget.name);
                strBuilder.AppendLine("\r\n\t\t{");
                strBuilder.AppendLine("\t\t\treturn true;");
                strBuilder.AppendLine("\t\t}");
                strBuilder.AppendLine();
            }

            if (strInterfaceType == "IXUIPopupList")
            {
                strBuilder.AppendFormat("\t\tprivate bool On{0}ValueChanged(IXUIObject uiObject)", widget.name);
                strBuilder.AppendLine("\r\n\t\t{");
                strBuilder.AppendLine("\t\t\tIXUIPopupList uiPopupList = uiObject as IXUIPopupList;");
                strBuilder.AppendLine("\t\t\tDebug.Log(uiPopupList.SelectedIndex);");
                strBuilder.AppendLine("\t\t\treturn true;");
                strBuilder.AppendLine("\t\t}");
                strBuilder.AppendLine();
            }

            if (widget.name.Contains("Dotween"))
            {
                string[] resultString = Regex.Split(widget.name, "Dotween");
                string dotweenName = resultString[1];
                listDotweenName.Add(dotweenName);
            }
        }

        //dotween
        if (listDotweenName.Count > 0)
        {
            strBuilder.AppendLine("\t\tprivate void InitDotween()");
            strBuilder.AppendLine("\t\t{");

            for (int i = 0; i < listDotweenName.Count; i++)
            {
                string dotweenName = listDotweenName[i];
                strBuilder.AppendFormat("\t\t\tif (m_dot{0} == null)", dotweenName);
                strBuilder.AppendLine("\r\n\t\t\t{");
                strBuilder.AppendFormat("\t\t\t\tm_dot{0} = uiBehaviour.m_Dotween{0}.CachedGameObject.GetComponents<DOTweenAnimation>();", dotweenName);
                strBuilder.AppendLine("\r\n\t\t\t}");
                strBuilder.AppendLine();
            }

            if (listDotweenName.Contains("Start"))
            {
                strBuilder.AppendLine("\t\t\tif (m_dotStart != null) ")
                        .AppendLine("\t\t\t{")
                        .AppendLine("\t\t\t\tPlayAnimationStart();")
                        .AppendLine("\t\t\t}");
                strBuilder.AppendLine();
            }

            if (listDotweenName.Contains("Window"))
            {
                strBuilder.AppendLine("\t\t\tif (m_dotWindow != null) ")
                        .AppendLine("\t\t\t{")
                        .AppendLine("\t\t\t\tPlayAnimationWindow();")
                        .AppendLine("\t\t\t}");
            }

            strBuilder.AppendLine("\t\t}");
            strBuilder.AppendLine();
        }

        for (int i = 0; i < listDotweenName.Count; i++)
        {
            string dotweenName = listDotweenName[i];
            strBuilder.AppendFormat("\t\tprivate void PlayAnimation{0}()", dotweenName);
            strBuilder.AppendLine("\r\n\t\t{");
            strBuilder.AppendFormat("\t\t\tfor (int i = 0; i < m_dot{0}.Length; i++)", dotweenName);
            strBuilder.AppendLine("\r\n\t\t\t{");
            strBuilder.AppendFormat("\t\t\t\tm_dot{0}[i].DORestart();", dotweenName);
            strBuilder.AppendLine("\r\n\t\t\t}");
            strBuilder.AppendLine("\t\t}");
            strBuilder.AppendLine();
        }
    }

    static void CreateWidgetsDeclareCode(ref StringBuilder strBuilder)
    {
        foreach (KeyValuePair<string, XUIObjectBase> pair in s_dicPath2WidgetCached)
        {
            XUIObjectBase widget = pair.Value;
			string strClassType = widget.GetType().ToString();
        	strBuilder.AppendFormat("   public {0} m_{1} = null;\r\n", s_dicWidgetInterface[strClassType], widget.name);
        }
    }

#region Gen Lua Code
    static void CreateLuaCodeForDlg(GameObject objPanel)
    {
        string strDlgName = objPanel.name.Substring(0, objPanel.name.Length - 5);
        string strDlgCtrlName = strDlgName + "Ctrl";
        string strFilePath = Application.dataPath + LuaPath + "/Controller/" + strDlgCtrlName + ".lua";

        StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);
        StringBuilder strBuilder = new StringBuilder();
        strBuilder.AppendLine("require \"Common/define\"")
                  .AppendLine("require \"3rd/pbc/protobuf\"")
                  .AppendLine();
        strBuilder.AppendFormat("{0} = {1};\r\n", strDlgCtrlName,"{}");
        strBuilder.AppendFormat("local this = {0};\r\n", strDlgCtrlName);
        strBuilder.AppendLine()
                  .AppendLine("local panel;")
                  .AppendLine("local luaBehaviour;")
                  .AppendLine("local transform;")
                  .AppendLine("local gameObject;")
                  .AppendLine();

        strBuilder.AppendLine("--构建函数--");
        strBuilder.AppendFormat("function {0}.New()\r\n", strDlgCtrlName);
        strBuilder.AppendFormat("\tlogWarn(\"{0}.New--->>\");\n", strDlgCtrlName);
        strBuilder.AppendLine("\treturn this;");
        strBuilder.AppendLine("end")
                  .AppendLine();

        strBuilder.AppendFormat("function {0}.Awake()\r\n", strDlgCtrlName);
        strBuilder.AppendFormat("\tlogWarn(\"{0}.Awake--->>\");",strDlgCtrlName);
        strBuilder.AppendFormat("\tpanelMgr:CreatePanel('{0}', this.OnCreate);\n",strDlgName);
        strBuilder.AppendLine("end")
                  .AppendLine();

        strBuilder.AppendLine("--启动事件--");
        strBuilder.AppendFormat("function {0}.OnCreate(obj)\r\n", strDlgCtrlName);
        strBuilder.AppendLine("\tgameObject = obj;")
                  .AppendLine("\ttransform = obj.transform;")
                  .AppendLine("\tpanel = transform:GetComponent('UIPanel');")
                  .AppendLine("\tluaBehaviour = transform:GetComponent('LuaBehaviour');");
        //strBuilder.AppendFormat("//resMgr:LoadPrefab('{0}', { 'PromptItem' }, this.InitPanel);", strDlgCtrlName.ToLower())
        //          .AppendLine("//this.InitPanel(nil)");
        strBuilder.AppendLine("end")
                  .AppendLine();

        //strBuilder.AppendLine("--初始化面板--");
        //strBuilder.AppendFormat("function {0}.InitPanel(objs)\r\n", strDlgCtrlName);
        //strBuilder.AppendLine("end")
        //          .AppendLine();

        strBuilder.AppendLine("--关闭事件--");
        strBuilder.AppendFormat("function {0}.Close()\r\n", strDlgCtrlName);
        strBuilder.AppendFormat("\tpanelMgr:ClosePanel(CtrlNames.{0});\n", strDlgName);
        strBuilder.AppendLine("end")
                  .AppendLine();

        sw.Write(strBuilder);
        sw.Flush();
        sw.Close();
    }

    static void CreateLuaCodeForDlgBehaviour(GameObject objPanel)
    {
        if (null == objPanel)
        {
            return;
        }
        string strDlgName = objPanel.name;

        string strFilePath = Application.dataPath + LuaPath + "/View/" + strDlgName + ".lua";
        
        StreamWriter sw = new StreamWriter(strFilePath, false, Encoding.UTF8);
        StringBuilder strBuilder = new StringBuilder();
        strBuilder.AppendLine("local transform;")
                  .AppendLine("local gameObject;")
                  .AppendLine();
        strBuilder.AppendFormat("{0} = {1};\r\n", strDlgName,"{}");
        strBuilder.AppendFormat("local this = {0};\r\n", strDlgName);

        strBuilder.AppendLine()
                  .AppendLine("--启动事件--");
        strBuilder.AppendFormat("function {0}.Awake(obj)\r\n", strDlgName);
        strBuilder.AppendLine("\tgameObject = obj;")
                  .AppendLine("\ttransform = obj.transform;")
                  .AppendLine("\tthis.InitPanel();")
                  .AppendLine("end")
                  .AppendLine();

        strBuilder.AppendLine("--初始化面板--");
        strBuilder.AppendFormat("function {0}.InitPanel(obj)\r\n", strDlgName);
        //strBuilder.AppendLine("\tgameObject = obj;")
        //          .AppendLine("\ttransform = obj.transform;")
        //          .AppendLine("\tthis.InitPanel();");
        CreateWidgetBindLuaCode(ref strBuilder, objPanel.transform);
        strBuilder.AppendLine("end")
                  .AppendLine();

        strBuilder.AppendLine("--销毁--");
        strBuilder.AppendFormat("function {0}.OnDestroy(obj)\r\n", strDlgName);
        strBuilder.AppendLine("\tlogWarn(\"OnDestroy---->>>\");")
                  .AppendLine("end")
                  .AppendLine();

        
        sw.Write(strBuilder);
        sw.Flush();
        sw.Close();
    }

    static void CreateWidgetBindLuaCode(ref StringBuilder strBuilder, Transform transRoot)
    {
        foreach (KeyValuePair<string, XUIObjectBase> pair in s_dicPath2WidgetCached)
        {
            XUIObjectBase widget = pair.Value;
            string strPath = GetWidgetPath(widget.transform, transRoot);
            string strClassType = widget.GetType().ToString();
            string strInterfaceType = "";
            if (s_dicWidgetInterface.TryGetValue(strClassType, out strInterfaceType) == false)
            {
                Debug.LogError("s_dicWidgetInterface.TryGetValue(strClassType, out strInterfaceType) == false:" + strClassType);
            }
            //this.btnOpen = transform:FindChild("Open").gameObject;
            strBuilder.AppendFormat("\tthis.m_{0} = transform:FindChild(\"{1}\").gameObject;\r\n", widget.name, strPath, strInterfaceType);

        }
    }
    #endregion


    //    static void FindAllWidgets(GameObject objPanel)
    //    {
    //        s_dicAllWidgetCached.Clear();
    //        {
    //            Component[] listXUILabel = objPanel.GetComponentsInChildren<XUILabel>();
    //            s_dicAllWidgetCached.Add("XUILabel", listXUILabel);
    //
    //            Component[] listXUIButton = objPanel.GetComponentsInChildren<XUIButton>();
    //            s_dicAllWidgetCached.Add("XUIButton", listXUIButton);
    //
    //            Component[] listXUIInput = objPanel.GetComponentsInChildren<XUIInput>();
    //            s_dicAllWidgetCached.Add("XUIInput", listXUIInput);
    //
    //            Component[] listXUIList = objPanel.GetComponentsInChildren<XUIList>();
    //            s_dicAllWidgetCached.Add("XUIList", listXUIList);
    //
    //            Component[] listXUITextList = objPanel.GetComponentsInChildren<XUITextList>();
    //            s_dicAllWidgetCached.Add("XUITextList", listXUITextList);
    //
    //            Component[] listXUIPicture = objPanel.GetComponentsInChildren<XUIPicture>();
    //            s_dicAllWidgetCached.Add("XUIPicture", listXUIPicture);
    //
    //            Component[] listXUICheckBox = objPanel.GetComponentsInChildren<XUICheckBox>();
    //            s_dicAllWidgetCached.Add("XUICheckBox", listXUICheckBox);
    //
    //            Component[] listXUISprite = objPanel.GetComponentsInChildren<XUISprite>();
    //            s_dicAllWidgetCached.Add("XUISprite", listXUISprite);
    //
    //            Component[] listXUIProgress = objPanel.GetComponentsInChildren<XUIProgress>();
    //            s_dicAllWidgetCached.Add("XUIProgress", listXUIProgress);
    //        }
    //    }

    static void FindAllWidgets(Transform trans, string strPath)
    {
        if (null == trans)
        {
            return;
        }
        for (int nIndex = 0; nIndex < trans.childCount; ++nIndex)
        {
            Transform child = trans.GetChild(nIndex);
            XUIObjectBase uiObject = child.GetComponent<XUIObjectBase>();
            string strTemp = strPath + "/" + child.name;
            if (null != uiObject)
            {
                if (typeof(IXUIListItem).IsAssignableFrom(uiObject.GetType()) == true)
                {
                    continue;
                }

                if (s_dicPath2WidgetCached.ContainsKey(child.name) == true)
                {
                    Debug.LogError("ID Repeated:" + strTemp);
                }
                s_dicPath2WidgetCached[child.name] = uiObject;

                if (typeof(IXUIGroup).IsAssignableFrom(uiObject.GetType()) == true)
                {
                    continue;
                }
            }

            FindAllWidgets(child, strTemp);
        }
    }

    static string GetWidgetPath(Transform obj, Transform root)
    {
        string path = obj.name;

        while (obj.parent != null && obj.parent != root)
        {
            obj = obj.transform.parent;
            path = obj.name + "/" + path;
        }
        return path;
    }

    static UICodeCreator()
    {
        s_dicWidgetInterface = new Dictionary<string, string>();
        s_dicWidgetInterface.Add("UILib.XUIObject", "IXUIObject");
        s_dicWidgetInterface.Add("UILib.XUILabel", "IXUILabel");
        s_dicWidgetInterface.Add("UILib.XUITextPro", "IXUILabel");
        s_dicWidgetInterface.Add("UILib.XUIRichLabel", "IXUIRichLabel");
        s_dicWidgetInterface.Add("UILib.XUIButton", "IXUIButton");
        s_dicWidgetInterface.Add("UILib.XUIInput", "IXUIInput");
        s_dicWidgetInterface.Add("UILib.XUIList", "IXUIList");
        //s_dicWidgetInterface.Add("XUITextList", "IXUITextList");
        s_dicWidgetInterface.Add("UILib.XUIPicture", "IXUIPicture");
        s_dicWidgetInterface.Add("UILib.XUICheckBox", "IXUICheckBox");
        s_dicWidgetInterface.Add("UILib.XUISprite", "IXUISprite");
        s_dicWidgetInterface.Add("UILib.XUIProgress", "IXUIProgress");
		s_dicWidgetInterface.Add("XUIGroup", "IXUIGroup");
        s_dicWidgetInterface.Add("UILib.XUIPopupList", "IXUIPopupList"); 
        s_dicWidgetInterface.Add("UILib.XUISlider", "IXUISlider");
        s_dicWidgetInterface.Add("UILib.XUILoopList", "IXUILoopList");
        s_dicWidgetInterface.Add("UILib.XUIAdaptList", "IXUIAdaptList");
        s_dicWidgetInterface.Add("UILib.XUIScrollView", "IXUIObject");

        s_dicWidgetInterface.Add("UILib.LoopListView2", "IXUILoopListView2");
        s_dicWidgetInterface.Add("UILib.LoopListViewItem2", "IXUIListItem");
        s_dicWidgetInterface.Add("UILib.HorizontalLoop3DList", "IXUILoop3DListView");
        s_dicWidgetInterface.Add("UILib.VerticalLoop3DList", "IXUILoop3DListView");

    }

    const string LuaPath = "/LuaFramework/Lua";
	static Dictionary<string, XUIObjectBase> s_dicPath2WidgetCached = null;
    static Dictionary<string, string> s_dicWidgetInterface = null;
}

