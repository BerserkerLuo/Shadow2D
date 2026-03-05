
using UnityEngine;
using Client.UI.UICommon;
using Client;
using UILib.Export;
public class DlgMainBehaviour : DlgBehaviourBase
{
	public override string AbName
	{
		get {  return "BattleState"; }
	}

	public override string FileName
	{
		get {  return "DlgMain"; }
	}

	public override void Init()
	{
		base.Init();
		m_Icon = GetUIObject("Header/Icon") as IXUISprite;
		if (null == m_Icon)
     	{
           Debug.Log("Header/Icon is null!");
		    m_Icon = WidgetFactory.CreateWidget<IXUISprite>();
     	}
		m_HPImg = GetUIObject("HP/HPImg") as IXUISprite;
		if (null == m_HPImg)
     	{
           Debug.Log("HP/HPImg is null!");
		    m_HPImg = WidgetFactory.CreateWidget<IXUISprite>();
     	}
		m_HPText = GetUIObject("HP/HPText") as IXUILabel;
		if (null == m_HPText)
     	{
           Debug.Log("HP/HPText is null!");
		    m_HPText = WidgetFactory.CreateWidget<IXUILabel>();
     	}
		m_ExpImg = GetUIObject("Exp/ExpImg") as IXUISprite;
		if (null == m_ExpImg)
     	{
           Debug.Log("Exp/ExpImg is null!");
		    m_ExpImg = WidgetFactory.CreateWidget<IXUISprite>();
     	}
		m_LevelText = GetUIObject("Exp/LevelText") as IXUILabel;
		if (null == m_LevelText)
     	{
           Debug.Log("Exp/LevelText is null!");
		    m_LevelText = WidgetFactory.CreateWidget<IXUILabel>();
     	}
		m_CoinText = GetUIObject("Coin/CoinText") as IXUILabel;
		if (null == m_CoinText)
     	{
           Debug.Log("Coin/CoinText is null!");
		    m_CoinText = WidgetFactory.CreateWidget<IXUILabel>();
     	}
		m_BulletText = GetUIObject("BuleetPanel/BulletText") as IXUILabel;
		if (null == m_BulletText)
     	{
           Debug.Log("BuleetPanel/BulletText is null!");
		    m_BulletText = WidgetFactory.CreateWidget<IXUILabel>();
     	}
		m_Reload = GetUIObject("BuleetPanel/Reload") as IXUIObject;
		if (null == m_Reload)
     	{
           Debug.Log("BuleetPanel/Reload is null!");
		    m_Reload = WidgetFactory.CreateWidget<IXUIObject>();
     	}
		m_ReloadIndex = GetUIObject("BuleetPanel/Reload/ReloadIndex") as IXUIObject;
		if (null == m_ReloadIndex)
     	{
           Debug.Log("BuleetPanel/Reload/ReloadIndex is null!");
		    m_ReloadIndex = WidgetFactory.CreateWidget<IXUIObject>();
     	}
		m_AttrButton = GetUIObject("AttrButton") as IXUIButton;
		if (null == m_AttrButton)
     	{
           Debug.Log("AttrButton is null!");
		    m_AttrButton = WidgetFactory.CreateWidget<IXUIButton>();
     	}
		m_WeaponButton = GetUIObject("WeaponButton") as IXUIButton;
		if (null == m_WeaponButton)
     	{
           Debug.Log("WeaponButton is null!");
		    m_WeaponButton = WidgetFactory.CreateWidget<IXUIButton>();
     	}
	}
   public IXUISprite m_Icon = null;
   public IXUISprite m_HPImg = null;
   public IXUILabel m_HPText = null;
   public IXUISprite m_ExpImg = null;
   public IXUILabel m_LevelText = null;
   public IXUILabel m_CoinText = null;
   public IXUILabel m_BulletText = null;
   public IXUIObject m_Reload = null;
   public IXUIObject m_ReloadIndex = null;
   public IXUIButton m_AttrButton = null;
   public IXUIButton m_WeaponButton = null;
}
