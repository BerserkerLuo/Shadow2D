using System;
using UnityEngine;
using Client.UI.UICommon;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;
using UILib;
using Game;
using PlayerSystemData;

namespace Client.UI
{
	public class DlgLobby : DlgBaseNew 
	{
		public static DlgLobby singleton
		{
			get
			{
				if (s_singleton == null)
				{
					Debug.Log("Load DlgLobby !");
					s_singleton = new DlgLobby();
					UIManager.Singleton.AddDlg(s_singleton);
				}
				return s_singleton;
			}
		}
		private static DlgLobby s_singleton = null;


		public DlgLobbyBehaviour uiBehaviour
		{
			get
			{
				return (DlgLobbyBehaviour)m_uiBehaviour;
			}
		}

		public DlgLobby()
		{
			m_uiBehaviour = new DlgLobbyBehaviour();
		}

		public override void Init()
		{
			//ShowHeroList();
		}

		public override void Reset()
		{ 
		}

		public override void RegisterEvent(){
			uiBehaviour.m_Button_Start.RegisterClickEventHandler(this.OnButton_StartClick);
			uiBehaviour.m_Button_RandHero.RegisterClickEventHandler(this.OnButton_RandHeroClick);
		}

		private bool OnButton_StartClick(IXUIObject uiObject)
		{
			//GameStateMgr.Singleton.ChangeGameState(EnumGameState.eState_Battle);
			DlgStartControl.singleton.SetVisible(true);

			uiBehaviour.m_ButtonList.SetVisible(false);

			return true;
		}

		private bool OnButton_RandHeroClick(IXUIObject uiObject){

			Debug.Log("RandHeroClick");

			//DlgBook.singleton.OnLobbyOpen();
			return true;
		}

		public void OnReturnLobby() {
			uiBehaviour.m_ButtonList.SetVisible(true);
			DlgStartControl.singleton.SetVisible(false);
		}

		protected override void OnShow()
		{
			base.OnShow();
			Refresh();
		}

		protected override void OnRefresh()
		{
			base.OnRefresh();
		}

		//=========================================================================
		//private Dictionary<int, IXUIListItem> heroUIList = new Dictionary<int, IXUIListItem>();
		//private void ShowHeroList() {
		//	heroUIList.Clear();

		//	List<HeroInfo> heros = HeroSystemUtils.GetHeroList();
		//	foreach (HeroInfo info in heros) 
		//		AddShowHero(info);
		//}

		//private void AddShowHero(HeroInfo hero) {
		//	IXUIListItem uiItem = uiBehaviour.m_UIList_Role.AddListItem();
		//	heroUIList.Add(hero.heroId,uiItem);

		//	uiItem.SetVisible(true);
		//}
	}
}
