
using UnityEngine;
using Client.UI.UICommon;
using Client;
using UILib.Export;
public class DlgLevelUpBehaviour : DlgBehaviourBase
{
	public override string AbName
	{
		get {  return "BattleState"; }
	}

	public override string FileName
	{
		get {  return "DlgLevelUp"; }
	}

	public override void Init()
	{
		base.Init();
		m_SelectSkillList = GetUIObject("SelectSkillList") as IXUIList;
		if (null == m_SelectSkillList)
     	{
           Debug.Log("SelectSkillList is null!");
		    m_SelectSkillList = WidgetFactory.CreateWidget<IXUIList>();
     	}
		m_RandButton = GetUIObject("RandButton") as IXUIButton;
		if (null == m_RandButton)
     	{
           Debug.Log("RandButton is null!");
		    m_RandButton = WidgetFactory.CreateWidget<IXUIButton>();
     	}
	}
   public IXUIList m_SelectSkillList = null;
   public IXUIButton m_RandButton = null;
}
