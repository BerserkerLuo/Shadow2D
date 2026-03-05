
using UnityEngine;
using Client.UI.UICommon;
using Client;
using UILib.Export;
public class DlgMessageBehaviour : DlgBehaviourBase
{
	public override string AbName
	{
		get {  return "LobbyState"; }
	}

	public override string FileName
	{
		get {  return "DlgMessage"; }
	}

	public override void Init()
	{
		base.Init();
		m_TextBG = GetUIObject("TextBG") as IXUISprite;
		if (null == m_TextBG)
     	{
           Debug.Log("TextBG is null!");
		    m_TextBG = WidgetFactory.CreateWidget<IXUISprite>();
     	}
		m_Text = GetUIObject("TextBG/Text") as IXUILabel;
		if (null == m_Text)
     	{
           Debug.Log("TextBG/Text is null!");
		    m_Text = WidgetFactory.CreateWidget<IXUILabel>();
     	}
	}
   public IXUISprite m_TextBG = null;
   public IXUILabel m_Text = null;
}
