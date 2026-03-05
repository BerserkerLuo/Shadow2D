using System;
using UnityEngine;
using Client.UI.UICommon;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;
using UILib;
using PlayerSystemData;
using Table;
using ECS;

namespace Client.UI
{
	public class DlgSelectHero : DlgBaseNew
	{
		 public static DlgSelectHero singleton
		{
			get
			{
				if (s_singleton == null)
				{
					s_singleton = new DlgSelectHero();
					UIManager.Singleton.AddDlg(s_singleton);
				}
				return s_singleton;
			}
		}
		 private static DlgSelectHero s_singleton = null;


		public DlgSelectHeroBehaviour uiBehaviour
		{
			get
			{
				return (DlgSelectHeroBehaviour)m_uiBehaviour;
			}
		}

		public DlgSelectHero()
		{
			m_uiBehaviour = new DlgSelectHeroBehaviour();
		}

		public override void Init()
		{
		}

		public override void Reset()
		{
		}

		public override void RegisterEvent()
		{
		}

		protected override void OnShow()
		{
			base.OnShow();
			Refresh();
		}

		protected override void OnRefresh()
		{
			base.OnRefresh();
			OnShowHeroList();
		}

		protected override void OnUnLoad() {

			Debug.Log("DlgSelectHero OnUnLoad");

			ClearAVObjs();
			ClearSelectAVObj();
		}

		public void OnShowHeroList() {

			ClearAVObjs();

			DlgMessage.singleton.ShowMessage($"OnShowHeroList");

			int selectHeroId = OperationSystem.Singleton.GetHeroId();

			List<HeroCfg> heroCfgs = TableMgr.Singleton.tables.HeroCfgMgr.DataList;
			int ignoreCount = 0;
			for (int index = 0; index < heroCfgs.Count; ++index) {
				HeroCfg heroCfg = heroCfgs[index];
				if (heroCfg.Active == false) {
					ignoreCount++;
					continue;
				}
				XUIButton button = ShowHero(heroCfg,index - ignoreCount);
				if (selectHeroId == 0 && index == 0)
					SetSelectButton(button);
				else if(selectHeroId == heroCfg.ID)
					SetSelectButton(button); 
			}
		}

		List<ECSModelObject> avObjs = new List<ECSModelObject>();
		public void ClearAVObjs() {
			foreach (ECSModelObject obj in avObjs)
				obj.Destory();
			avObjs.Clear();
		}

		public XUIButton ShowHero(HeroCfg heroCfg,int index) {
			IXUIObject heroItem = uiBehaviour.m_SelectHeroList.GetItemByIndexOrAdd(index);

			XUIButton button = (XUIButton)heroItem.GetUIObject("Button");

			ECSModelObject avObj = ECSModelObject.GetByModelName(heroCfg.UIAvatar);
            avObj.OnActive();
			RectTransform rectTrans = avObj.gameObject.GetComponent<RectTransform>();

			rectTrans.parent = button.CachedTransform;
			rectTrans.anchorMin = new Vector2(0.5f, 0.5f);
			rectTrans.anchorMax = new Vector2(0.5f, 0.5f);
			rectTrans.sizeDelta = new Vector2(140, 140);
			rectTrans.anchoredPosition = new Vector2(0, 10);
			rectTrans.localScale = Vector3.one;

			button.Data = heroCfg.ID;
			button.RegisterEnterEventHandler(OnPointEnter);
			button.RegisterClickEventHandler(OnPointClick);

			avObjs.Add(avObj);

			XUIButton unLockButton = (XUIButton)heroItem.GetUIObject("UnLockButton");
			if (!HeroSystem.Singleton.CheckHeroIsLock(heroCfg.ID)){
				XUITextPro priceText = (XUITextPro)unLockButton.GetUIObject("PriceText");
				priceText.SetText(heroCfg.UnLock.ToString());

				unLockButton.Data = heroCfg.ID;
				unLockButton.RegisterClickEventHandler(OnUnlockButtonClick);
			}
			else {
				unLockButton.SetVisible(false);
			}
			

			return button;
		}

		public bool OnPointEnter(IXUIObject obj) {
			SetSelectButton((XUIButton)obj);
			return true;
		}
		public bool OnPointClick(IXUIObject obj){
			OnConfirmHero((XUIButton)obj);
			return true;
		}

		public bool OnUnlockButtonClick(IXUIObject obj) {
			XUIButton button = (XUIButton)obj;
			ShopUtils.BuyHero((int)button.Data);
			return true;
		}

		//================================================

		XUIButton selectButton;
		ECSModelObject selectAVObj;
		public void ClearSelectAVObj() {
			selectAVObj.Destory();
			selectAVObj = null;
		}
		public void SetSelectButton(XUIButton button) {
			int heroId = (int)button.Data;
			if (!HeroSystem.Singleton.CheckHeroIsLock(heroId))
				return;

			SetButtonPush(selectButton, false);

			selectButton = button;
			OnSelectHero(heroId);

			SetButtonPush(selectButton, true);
		}

		public void SetButtonPush(XUIButton button,bool flag) {
			if (button == null) return;
			IXUIObject pushObj = button.GetUIObject("Push");
			pushObj.SetVisible(flag);
		}

		public void OnSelectHero(int heroId) {
			HeroCfg heroCfg = TableMgr.Singleton.GetHeroCfg(heroId);
			if (heroCfg == null)
				return;

			DlgMessage.singleton.ShowMessage($"OnSelectHero [{heroId}]");

			OperationSystem.Singleton.SetHeroId(heroId);

			uiBehaviour.m_Name.SetText(heroCfg.Name);
			uiBehaviour.m_HPText.SetText(heroCfg.HP.ToString());

			if (selectAVObj != null)
				selectAVObj.Destory();

			selectAVObj = ECSModelObject.GetByModelName(heroCfg.UIAvatar);
			selectAVObj.OnActive();
            RectTransform rectTrans = selectAVObj.gameObject.GetComponent<RectTransform>();

            rectTrans.parent = uiBehaviour.m_SelectRoleAv.CachedTransform;
            rectTrans.anchorMin = new Vector2(0.5f, 0.5f);
            rectTrans.anchorMax = new Vector2(0.5f, 0.5f);
            rectTrans.sizeDelta = new Vector2(512, 512);
            rectTrans.anchoredPosition = new Vector2(0, 0);
            rectTrans.localScale = Vector3.one;
        }

		//================================================
		public void OnConfirmHero(XUIButton button) {

			DlgMessage.singleton.ShowMessage($"OnConfirmHero");

			int heroId = (int)button.Data;
			//if (!HeroSystem.Singleton.CheckHeroIsLock(heroId))
			//	return;

			DlgMessage.singleton.ShowMessage($"OnConfirmHero2");

			DlgStartControl.singleton.ChangeToSelectWeapon();
		}
	}
}
