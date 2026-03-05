
using UnityEngine;
using Client.UI.UICommon;
using Client;
using UILib.Export;
public class DlgWeaponUpBehaviour : DlgBehaviourBase
{
	public override string AbName
	{
		get {  return "BattleState"; }
	}

	public override string FileName
	{
		get {  return "DlgWeaponUp"; }
	}

	public override void Init()
	{
		base.Init();
		m_RandSkillList = GetUIObject("RandSkillList") as IXUIList;
		if (null == m_RandSkillList)
     	{
           Debug.Log("RandSkillList is null!");
		    m_RandSkillList = WidgetFactory.CreateWidget<IXUIList>();
     	}
		m_RandButton = GetUIObject("RandButton") as IXUIButton;
		if (null == m_RandButton)
     	{
           Debug.Log("RandButton is null!");
		    m_RandButton = WidgetFactory.CreateWidget<IXUIButton>();
     	}
		m_CloseButton = GetUIObject("CloseButton") as IXUIButton;
		if (null == m_CloseButton)
     	{
           Debug.Log("CloseButton is null!");
		    m_CloseButton = WidgetFactory.CreateWidget<IXUIButton>();
     	}
		m_WeaponCfgRoot = GetUIObject("WeaponCfgRoot") as IXUIObject;
		if (null == m_WeaponCfgRoot)
     	{
           Debug.Log("WeaponCfgRoot is null!");
		    m_WeaponCfgRoot = WidgetFactory.CreateWidget<IXUIObject>();
     	}
	}
   public IXUIList m_RandSkillList = null;
   public IXUIButton m_RandButton = null;
   public IXUIButton m_CloseButton = null;
   public IXUIObject m_WeaponCfgRoot = null;
}
