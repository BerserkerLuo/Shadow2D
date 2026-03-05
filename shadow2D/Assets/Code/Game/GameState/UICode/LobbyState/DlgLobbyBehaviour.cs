
using UnityEngine;
using Client.UI.UICommon;
using Client;
using UILib.Export;
public class DlgLobbyBehaviour : DlgBehaviourBase
{
	public override string AbName
	{
		get {  return "LobbyState"; }
	}

	public override string FileName
	{
		get {  return "DlgLobby"; }
	}

	public override void Init()
	{
		base.Init();
		m_ButtonList = GetUIObject("ButtonList") as IXUIObject;
		if (null == m_ButtonList)
     	{
           Debug.Log("ButtonList is null!");
		    m_ButtonList = WidgetFactory.CreateWidget<IXUIObject>();
     	}
		m_Button_Start = GetUIObject("ButtonList/Button_Start") as IXUIButton;
		if (null == m_Button_Start)
     	{
           Debug.Log("ButtonList/Button_Start is null!");
		    m_Button_Start = WidgetFactory.CreateWidget<IXUIButton>();
     	}
		m_Button_RandHero = GetUIObject("ButtonList/Button_RandHero") as IXUIButton;
		if (null == m_Button_RandHero)
     	{
           Debug.Log("ButtonList/Button_RandHero is null!");
		    m_Button_RandHero = WidgetFactory.CreateWidget<IXUIButton>();
     	}
		m_Button_Store = GetUIObject("ButtonList/Button_Store") as IXUIButton;
		if (null == m_Button_Store)
     	{
           Debug.Log("ButtonList/Button_Store is null!");
		    m_Button_Store = WidgetFactory.CreateWidget<IXUIButton>();
     	}
	}
   public IXUIObject m_ButtonList = null;
   public IXUIButton m_Button_Start = null;
   public IXUIButton m_Button_RandHero = null;
   public IXUIButton m_Button_Store = null;
}
