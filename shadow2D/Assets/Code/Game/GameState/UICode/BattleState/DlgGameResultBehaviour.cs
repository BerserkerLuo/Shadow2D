
using UnityEngine;
using Client.UI.UICommon;
using Client;
using UILib.Export;
public class DlgGameResultBehaviour : DlgBehaviourBase
{
	public override string AbName
	{
		get {  return "BattleState"; }
	}

	public override string FileName
	{
		get {  return "DlgGameResult"; }
	}

	public override void Init()
	{
		base.Init();
		m_LvText = GetUIObject("ResultList/Item (2)/LvText") as IXUILabel;
		if (null == m_LvText)
     	{
           Debug.Log("ResultList/Item (2)/LvText is null!");
		    m_LvText = WidgetFactory.CreateWidget<IXUILabel>();
     	}
		m_LifeText = GetUIObject("ResultList/Item/LifeText") as IXUILabel;
		if (null == m_LifeText)
     	{
           Debug.Log("ResultList/Item/LifeText is null!");
		    m_LifeText = WidgetFactory.CreateWidget<IXUILabel>();
     	}
		m_KillText = GetUIObject("ResultList/Item (1)/KillText") as IXUILabel;
		if (null == m_KillText)
     	{
           Debug.Log("ResultList/Item (1)/KillText is null!");
		    m_KillText = WidgetFactory.CreateWidget<IXUILabel>();
     	}
		m_OkButton = GetUIObject("OkButton") as IXUIButton;
		if (null == m_OkButton)
     	{
           Debug.Log("OkButton is null!");
		    m_OkButton = WidgetFactory.CreateWidget<IXUIButton>();
     	}
	}
   public IXUILabel m_LvText = null;
   public IXUILabel m_LifeText = null;
   public IXUILabel m_KillText = null;
   public IXUIButton m_OkButton = null;
}
