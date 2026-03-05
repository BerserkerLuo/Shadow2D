using System;
using UnityEngine;
using Client.UI.UICommon;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;
using UILib;
using Table;
using ECS;
using PlayerSystemData;

namespace Client.UI
{
	public class DlgSelectWeapon : DlgBaseNew
	{
		 public static DlgSelectWeapon singleton
		{
			get
			{
				if (s_singleton == null)
				{
					s_singleton = new DlgSelectWeapon();
					UIManager.Singleton.AddDlg(s_singleton);
				}
				return s_singleton;
			}
		}
		 private static DlgSelectWeapon s_singleton = null;


		public DlgSelectWeaponBehaviour uiBehaviour
		{
			get
			{
				return (DlgSelectWeaponBehaviour)m_uiBehaviour;
			}
		}

		public DlgSelectWeapon()
		{
			m_uiBehaviour = new DlgSelectWeaponBehaviour();
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
			ShowWeaponList();
		}


		public void ShowWeaponList() {

			DlgMessage.singleton.ShowMessage($"ShowWeaponList");

			ClearAVObjs();

			int selectWeaponId = OperationSystem.Singleton.GetWeaponId();
			List<WeaponCfg> weaponCfgs = TableMgr.Singleton.tables.WeaponCfgMgr.DataList;
			for (int index = 0; index < weaponCfgs.Count; ++index) {
				var weaponCfg = weaponCfgs[index];
				XUIButton button = ShowWeapon(weaponCfg, index);
				if (selectWeaponId == 0 && index == 0)
					SetSelectButton(button);
				else if (selectWeaponId == weaponCfg.ID)
					SetSelectButton(button);
			}
		}

		List<ECSModelObject> avObjs = new List<ECSModelObject>();
		public void ClearAVObjs(){
			foreach (ECSModelObject obj in avObjs)
				obj.Destory();
			avObjs.Clear();
		}

		public XUIButton ShowWeapon(WeaponCfg weaponCfg, int index)
		{
			IXUIObject item = uiBehaviour.m_SelectWeaponList.GetItemByIndexOrAdd(index);

			XUIButton button = (XUIButton)item.GetUIObject("Button");

			ECSModelObject avObj = ECSModelObject.GetByModelName(weaponCfg.UIAvatar);
			avObj.OnActive();
			RectTransform rectTrans = avObj.gameObject.GetComponent<RectTransform>();

			rectTrans.parent = button.CachedTransform;
			rectTrans.anchorMin = new Vector2(0.5f, 0.5f);
			rectTrans.anchorMax = new Vector2(0.5f, 0.5f);
			rectTrans.sizeDelta = new Vector2(140, 140);
			rectTrans.anchoredPosition = new Vector2(0, 10);
			rectTrans.localScale = Vector3.one;

			button.Data = weaponCfg.ID;
			button.RegisterEnterEventHandler(OnPointEnter);
			button.RegisterClickEventHandler(OnPointClick);

			avObjs.Add(avObj);

			XUIButton unLockButton = (XUIButton)item.GetUIObject("UnLockButton");
			if (!WeaponSystem.Singleton.CheckWeaponIsLock(weaponCfg.ID)){
				XUITextPro priceText = (XUITextPro)unLockButton.GetUIObject("PriceText");
				priceText.SetText(weaponCfg.UnLock.ToString());

				unLockButton.Data = weaponCfg.ID;
				unLockButton.RegisterClickEventHandler(OnUnlockButtonClick);
			}
			else{
				unLockButton.SetVisible(false);
			}

			return button;
		}

		public bool OnPointEnter(IXUIObject obj)
		{
			SetSelectButton((XUIButton)obj);
			return true;
		}
		public bool OnPointClick(IXUIObject obj){
			OnConfirmWeapon((XUIButton)obj);
			return true;
		}

		public bool OnUnlockButtonClick(IXUIObject obj){
			XUIButton button = (XUIButton)obj;
			ShopUtils.BuyWeapon((int)button.Data);
			return true;
		}

		//================================================

		XUIButton selectButton;
		ECSModelObject selectAVObj;
		public void ClearSelectAVObj()
		{
			selectAVObj.Destory();
			selectAVObj = null;
		}
		public void SetSelectButton(XUIButton button)
		{
			int weaponId = (int)button.Data;
			//if (!WeaponSystem.Singleton.CheckWeaponIsLock(weaponId))
			//	return;

			SetButtonPush(selectButton, false);

			selectButton = button;
			OnSelectWeapon(weaponId);

			SetButtonPush(selectButton, true);
		}

		public void SetButtonPush(XUIButton button, bool flag)
		{
			if (button == null) return;
			IXUIObject pushObj = button.GetUIObject("Push");
			pushObj.SetVisible(flag);
		}

		public void OnSelectWeapon(int weaponId){
			WeaponCfg weaponCfg = TableMgr.Singleton.GetWeaponCfg(weaponId);
			if (weaponCfg == null)
				return;

			OperationSystem.Singleton.SetWeaponId(weaponId);

			uiBehaviour.m_WeaponName.SetText(weaponCfg.Name);
			uiBehaviour.m_AtkText.SetText(weaponCfg.Damage.ToString());

			if (selectAVObj != null)
				selectAVObj.Destory();
		}

		//================================================
		public void OnConfirmWeapon(XUIButton button){
			int weaponId = (int)button.Data;
			//if (!WeaponSystem.Singleton.CheckWeaponIsLock(weaponId))
			//	return;

			DlgStartControl.singleton.ChangeToSelectMap();
		}
	}
}

