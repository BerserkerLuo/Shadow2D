
using UnityEngine;
using Client.UI.UICommon;
using Client;
using UILib.Export;
public class DlgSelectMapBehaviour : DlgBehaviourBase
{
	public override string AbName
	{
		get {  return "LobbyState"; }
	}

	public override string FileName
	{
		get {  return "DlgSelectMap"; }
	}

	public override void Init()
	{
		base.Init();
		m_SelectMapList = GetUIObject("BG/SelectMapList") as IXUIList;
		if (null == m_SelectMapList)
     	{
           Debug.Log("BG/SelectMapList is null!");
		    m_SelectMapList = WidgetFactory.CreateWidget<IXUIList>();
     	}
		m_RankList = GetUIObject("BG/RankList") as IXUIList;
		if (null == m_RankList)
     	{
           Debug.Log("BG/RankList is null!");
		    m_RankList = WidgetFactory.CreateWidget<IXUIList>();
     	}
		m_AttrList = GetUIObject("BG/AttrList") as IXUIList;
		if (null == m_AttrList)
     	{
           Debug.Log("BG/AttrList is null!");
		    m_AttrList = WidgetFactory.CreateWidget<IXUIList>();
     	}
		m_Start = GetUIObject("BG/Start") as IXUIButton;
		if (null == m_Start)
     	{
           Debug.Log("BG/Start is null!");
		    m_Start = WidgetFactory.CreateWidget<IXUIButton>();
     	}
		m_Return = GetUIObject("BG/Return") as IXUIButton;
		if (null == m_Return)
     	{
           Debug.Log("BG/Return is null!");
		    m_Return = WidgetFactory.CreateWidget<IXUIButton>();
     	}
	}
   public IXUIList m_SelectMapList = null;
   public IXUIList m_RankList = null;
   public IXUIList m_AttrList = null;
   public IXUIButton m_Start = null;
   public IXUIButton m_Return = null;
}
