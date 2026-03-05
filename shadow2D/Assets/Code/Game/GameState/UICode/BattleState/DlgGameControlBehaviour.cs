
using UnityEngine;
using Client.UI.UICommon;
using Client;
using UILib.Export;
public class DlgGameControlBehaviour : DlgBehaviourBase
{
	public override string AbName
	{
		get {  return "BattleState"; }
	}

	public override string FileName
	{
		get {  return "DlgGameControl"; }
	}

	public override void Init()
	{
		base.Init();
		m_SelectBox = GetUIObject("SelectBox") as IXUIObject;
		if (null == m_SelectBox)
     	{
           Debug.Log("SelectBox is null!");
		    m_SelectBox = WidgetFactory.CreateWidget<IXUIObject>();
     	}
	}
   public IXUIObject m_SelectBox = null;
}
