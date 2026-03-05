
using UnityEngine;
using Client.UI.UICommon;
using Client;
using UILib.Export;
public class DlgSelectWeaponBehaviour : DlgBehaviourBase
{
	public override string AbName
	{
		get {  return "LobbyState"; }
	}

	public override string FileName
	{
		get {  return "DlgSelectWeapon"; }
	}

	public override void Init()
	{
		base.Init();
		m_WeaponName = GetUIObject("AttrList/Name/WeaponName") as IXUILabel;
		if (null == m_WeaponName)
     	{
           Debug.Log("AttrList/Name/WeaponName is null!");
		    m_WeaponName = WidgetFactory.CreateWidget<IXUILabel>();
     	}
		m_AtkText = GetUIObject("AttrList/Attack/AtkText") as IXUILabel;
		if (null == m_AtkText)
     	{
           Debug.Log("AttrList/Attack/AtkText is null!");
		    m_AtkText = WidgetFactory.CreateWidget<IXUILabel>();
     	}
		m_WeaponDesc = GetUIObject("AttrList/Desc/WeaponDesc") as IXUILabel;
		if (null == m_WeaponDesc)
     	{
           Debug.Log("AttrList/Desc/WeaponDesc is null!");
		    m_WeaponDesc = WidgetFactory.CreateWidget<IXUILabel>();
     	}
		m_SelectWeaponList = GetUIObject("SelectWeaponList") as IXUIList;
		if (null == m_SelectWeaponList)
     	{
           Debug.Log("SelectWeaponList is null!");
		    m_SelectWeaponList = WidgetFactory.CreateWidget<IXUIList>();
     	}
		m_WeaponCfg = GetUIObject("WeaponCfg") as IXUIObject;
		if (null == m_WeaponCfg)
     	{
           Debug.Log("WeaponCfg is null!");
		    m_WeaponCfg = WidgetFactory.CreateWidget<IXUIObject>();
     	}
	}
   public IXUILabel m_WeaponName = null;
   public IXUILabel m_AtkText = null;
   public IXUILabel m_WeaponDesc = null;
   public IXUIList m_SelectWeaponList = null;
   public IXUIObject m_WeaponCfg = null;
}
