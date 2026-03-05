
using UnityEngine;
using Client.UI.UICommon;
using Client;
using UILib.Export;
public class DlgStartControlBehaviour : DlgBehaviourBase
{
	public override string AbName
	{
		get {  return "LobbyState"; }
	}

	public override string FileName
	{
		get {  return "DlgStartControl"; }
	}

	public override void Init()
	{
		base.Init();
		m_CoinText = GetUIObject("Coin/CoinText") as IXUILabel;
		if (null == m_CoinText)
     	{
           Debug.Log("Coin/CoinText is null!");
		    m_CoinText = WidgetFactory.CreateWidget<IXUILabel>();
     	}
		m_Return = GetUIObject("Return") as IXUIButton;
		if (null == m_Return)
     	{
           Debug.Log("Return is null!");
		    m_Return = WidgetFactory.CreateWidget<IXUIButton>();
     	}
		m_PushText = GetUIObject("Return/PushText") as IXUILabel;
		if (null == m_PushText)
     	{
           Debug.Log("Return/PushText is null!");
		    m_PushText = WidgetFactory.CreateWidget<IXUILabel>();
     	}
		m_NomalText = GetUIObject("Return/Nomal/NomalText") as IXUILabel;
		if (null == m_NomalText)
     	{
           Debug.Log("Return/Nomal/NomalText is null!");
		    m_NomalText = WidgetFactory.CreateWidget<IXUILabel>();
     	}
	}
   public IXUILabel m_CoinText = null;
   public IXUIButton m_Return = null;
   public IXUILabel m_PushText = null;
   public IXUILabel m_NomalText = null;
}
