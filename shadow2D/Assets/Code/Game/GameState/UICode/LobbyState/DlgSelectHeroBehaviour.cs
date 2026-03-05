
using UnityEngine;
using Client.UI.UICommon;
using Client;
using UILib.Export;
public class DlgSelectHeroBehaviour : DlgBehaviourBase
{
	public override string AbName
	{
		get {  return "LobbyState"; }
	}

	public override string FileName
	{
		get {  return "DlgSelectHero"; }
	}

	public override void Init()
	{
		base.Init();
		m_Name = GetUIObject("AttrList/Name/Name") as IXUILabel;
		if (null == m_Name)
     	{
           Debug.Log("AttrList/Name/Name is null!");
		    m_Name = WidgetFactory.CreateWidget<IXUILabel>();
     	}
		m_HPText = GetUIObject("AttrList/HP/HPText") as IXUILabel;
		if (null == m_HPText)
     	{
           Debug.Log("AttrList/HP/HPText is null!");
		    m_HPText = WidgetFactory.CreateWidget<IXUILabel>();
     	}
		m_SkillIcon = GetUIObject("AttrList/Skill/SkillIcon") as IXUISprite;
		if (null == m_SkillIcon)
     	{
           Debug.Log("AttrList/Skill/SkillIcon is null!");
		    m_SkillIcon = WidgetFactory.CreateWidget<IXUISprite>();
     	}
		m_SKillDesc = GetUIObject("AttrList/Skill/SKillDesc") as IXUILabel;
		if (null == m_SKillDesc)
     	{
           Debug.Log("AttrList/Skill/SKillDesc is null!");
		    m_SKillDesc = WidgetFactory.CreateWidget<IXUILabel>();
     	}
		m_SelectHeroList = GetUIObject("SelectHeroList") as IXUIList;
		if (null == m_SelectHeroList)
     	{
           Debug.Log("SelectHeroList is null!");
		    m_SelectHeroList = WidgetFactory.CreateWidget<IXUIList>();
     	}
		m_SelectRoleAv = GetUIObject("SelectRoleAv") as IXUIObject;
		if (null == m_SelectRoleAv)
     	{
           Debug.Log("SelectRoleAv is null!");
		    m_SelectRoleAv = WidgetFactory.CreateWidget<IXUIObject>();
     	}
		m_SelectWeaponAv = GetUIObject("SelectWeaponAv") as IXUIObject;
		if (null == m_SelectWeaponAv)
     	{
           Debug.Log("SelectWeaponAv is null!");
		    m_SelectWeaponAv = WidgetFactory.CreateWidget<IXUIObject>();
     	}
	}
   public IXUILabel m_Name = null;
   public IXUILabel m_HPText = null;
   public IXUISprite m_SkillIcon = null;
   public IXUILabel m_SKillDesc = null;
   public IXUIList m_SelectHeroList = null;
   public IXUIObject m_SelectRoleAv = null;
   public IXUIObject m_SelectWeaponAv = null;
}
