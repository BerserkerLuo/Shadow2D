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
	public class DlgStartControl : DlgBaseNew
	{
		 public static DlgStartControl singleton
		{
			get
			{
				if (s_singleton == null)
				{
					s_singleton = new DlgStartControl();
					UIManager.Singleton.AddDlg(s_singleton);
				}
				return s_singleton;
			}
		}
		 private static DlgStartControl s_singleton = null;


		public DlgStartControlBehaviour uiBehaviour
		{
			get
			{
				return (DlgStartControlBehaviour)m_uiBehaviour;
			}
		}

		public DlgStartControl()
		{
			m_uiBehaviour = new DlgStartControlBehaviour();
		}

		public override void Init()
		{
		}

		public override void Reset()
		{
		}

		public override void RegisterEvent()
		{
			uiBehaviour.m_Return.RegisterClickEventHandler(this.OnButton_StartClick);
		}

		protected override void OnShow()
		{
			base.OnShow();
			Refresh();
			RefreshCoin();
		}

		protected override void OnRefresh()
		{
			base.OnRefresh();

			Step = 0;
			ChangeToSelectHero();
		}

		private bool OnButton_StartClick(IXUIObject uiObject){
			ChangeStep(Step - 1);
			return true;
		}

		public int Step = 0; //0 大厅 1选英雄 2选武器 3选地图 4进游戏

		public void ChangeStep(int newStep) {
			SetStepDlgVisible(Step, false);
			SetStepDlgVisible(newStep, true);
			Step = newStep;

			if (newStep == 0) DlgLobby.singleton.OnReturnLobby();
		}

		public void SetStepDlgVisible(int step,bool flag) {
			switch (step){
				case 1: DlgSelectHero.singleton.SetVisible(flag);break;
				case 2: DlgSelectWeapon.singleton.SetVisible(flag); break;
				case 3: DlgSelectMap.singleton.SetVisible(flag); break;
			}
		}

		public void ChangeToSelectHero() { ChangeStep(1); }
		public void ChangeToSelectWeapon(){ChangeStep(2);}
		public void ChangeToSelectMap(){ChangeStep(3);}
		public void ChangeToStartGame(){
			ChangeStep(4);
			SetVisible(false);
			GameStateMgr.Singleton.ChangeGameState(EnumGameState.eState_Battle);
		}
		//======================================================================
		public void RefreshCoin() {
			int gold = CurrencySystem.Singleton.GetGold();
			uiBehaviour.m_CoinText.SetText(gold.ToString());
		}
	}
}
