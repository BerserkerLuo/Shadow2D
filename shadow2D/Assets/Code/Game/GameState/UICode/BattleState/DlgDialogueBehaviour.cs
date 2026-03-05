
using UnityEngine;
using Client.UI.UICommon;
using Client;
using UILib.Export;
public class DlgDialogueBehaviour : DlgBehaviourBase
{
	public override string AbName
	{
		get {  return "BattleState"; }
	}

	public override string FileName
	{
		get {  return "DlgDialogue"; }
	}

	public override void Init()
	{
		base.Init();
		m_Panel = GetUIObject("Anchor_Bottom/Panel") as IXUIObject;
		if (null == m_Panel)
     	{
           Debug.Log("Anchor_Bottom/Panel is null!");
		    m_Panel = WidgetFactory.CreateWidget<IXUIObject>();
     	}
		m_Button_Fast = GetUIObject("Anchor_Bottom/Panel/Button_Fast") as IXUIButton;
		if (null == m_Button_Fast)
     	{
           Debug.Log("Anchor_Bottom/Panel/Button_Fast is null!");
		    m_Button_Fast = WidgetFactory.CreateWidget<IXUIButton>();
     	}
		m_Button_Skip = GetUIObject("Anchor_Bottom/Panel/Button_Skip") as IXUIButton;
		if (null == m_Button_Skip)
     	{
           Debug.Log("Anchor_Bottom/Panel/Button_Skip is null!");
		    m_Button_Skip = WidgetFactory.CreateWidget<IXUIButton>();
     	}
		m_Button_Next = GetUIObject("Anchor_Bottom/Panel/Button_Next") as IXUIButton;
		if (null == m_Button_Next)
     	{
           Debug.Log("Anchor_Bottom/Panel/Button_Next is null!");
		    m_Button_Next = WidgetFactory.CreateWidget<IXUIButton>();
     	}
		m_Text_Dialogue = GetUIObject("Anchor_Bottom/Panel/Text_Dialogue") as IXUILabel;
		if (null == m_Text_Dialogue)
     	{
           Debug.Log("Anchor_Bottom/Panel/Text_Dialogue is null!");
		    m_Text_Dialogue = WidgetFactory.CreateWidget<IXUILabel>();
     	}
	}
   public IXUIObject m_Panel = null;
   public IXUIButton m_Button_Fast = null;
   public IXUIButton m_Button_Skip = null;
   public IXUIButton m_Button_Next = null;
   public IXUILabel m_Text_Dialogue = null;
}
