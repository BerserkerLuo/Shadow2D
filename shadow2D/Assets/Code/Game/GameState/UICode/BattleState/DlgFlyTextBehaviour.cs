
using UnityEngine;
using Client.UI.UICommon;
using Client;
using UILib.Export;
public class DlgFlyTextBehaviour : DlgBehaviourBase
{
	public override string AbName
	{
		get {  return "BattleState"; }
	}

	public override string FileName
	{
		get {  return "DlgFlyText"; }
	}

	public override void Init()
	{
		base.Init();
		m_TextItemList = GetUIObject("TextItemList") as IXUIList;
		if (null == m_TextItemList)
     	{
           Debug.Log("TextItemList is null!");
		    m_TextItemList = WidgetFactory.CreateWidget<IXUIList>();
     	}
	}
   public IXUIList m_TextItemList = null;
}
