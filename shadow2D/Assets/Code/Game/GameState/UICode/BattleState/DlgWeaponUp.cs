using System;
using UnityEngine;
using Client.UI.UICommon;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;
using UILib;
using Tools;
using ECS;
using Table;
using Game;

namespace Client.UI
{
	public class DlgWeaponUp : DlgBaseNew
	{
		public static DlgWeaponUp singleton
		{
			get
			{
				if (s_singleton == null)
				{
					s_singleton = new DlgWeaponUp();
					UIManager.Singleton.AddDlg(s_singleton);
				}
				return s_singleton;
			}
		}
		 private static DlgWeaponUp s_singleton = null;


		public DlgWeaponUpBehaviour uiBehaviour
		{
			get
			{
				return (DlgWeaponUpBehaviour)m_uiBehaviour;
			}
		}

		public DlgWeaponUp()
		{
			m_uiBehaviour = new DlgWeaponUpBehaviour();
		}

		public override void Init()
		{
		}

		public override void Reset()
		{
		}

		public override void RegisterEvent()
		{
			uiBehaviour.m_RandButton.RegisterClickEventHandler(this.OnRandButtonClick);
			uiBehaviour.m_CloseButton.RegisterClickEventHandler(this.OnCloseButtonClick);
		}

		private bool OnRandButtonClick(IXUIObject uiObject)
		{
			return true;
		}

		private bool OnCloseButtonClick(IXUIObject uiObject)
		{
			SetVisible(false);
			Util.TimeRun();
			return true;
		}

		protected override void OnShow()
		{
			base.OnShow();
			Refresh();
		}

		protected override void OnRefresh()
		{
			base.OnRefresh();

			if (WeaponSetObj == null)
				LoadWeraponSetting();

			ShowRandList();
		}

        protected override void OnUnLoad()
        {
            base.OnUnLoad();

			if (WeaponSetObj != null) {
				WeaponSetObj.Destory();
				WeaponSetObj = null;
			}
        }


        //======================================================================================

        ECSGameObject WeaponSetObj;
		WeaponSetting weaponSetting;

		private void LoadWeraponSetting() {
			Entity player = UIUtils.GetPlayer();
			WeaponSkillComponent comp = player.GetComponentData<WeaponSkillComponent>();
			if (comp == null)
				return;

			WeaponCfg weaponCfg = TableMgr.Singleton.GetWeaponCfg(comp.WeaponId);
			if (weaponCfg == null)
				return;

			WeaponSetObj = ECSGameObject.Get(weaponCfg.WeaponSet);

			RectTransform weaponRectTrans = WeaponSetObj.transform as RectTransform;

			weaponRectTrans.parent = uiBehaviour.m_WeaponCfgRoot.CachedRectTransform;
			weaponRectTrans.anchoredPosition = Vector2.zero;
			weaponRectTrans.localScale = Vector3.one;

			WeaponSetObj.OnActive();

			weaponSetting = weaponRectTrans.GetComponent<WeaponSetting>();
		}

		//======================================================================================

		public void ShowRandList() {

			Entity player = UIUtils.GetPlayer();
			if (player == null)
				return;

			List<ShopItemData> randSkillList = SkillRandUtils.RandSkillList(player);

			for (int index = 0; index < randSkillList.Count; ++index) {
				ShopItemData data = randSkillList[index];

				ItemBattleCfg itemCfg = TableMgr.Singleton.GetItemBattleCfg(data.ItemId);
				SkillCfg skillCfg = TableMgr.Singleton.GetSkillCfg(itemCfg.Value1);
				SkillLvCfg skillLKvCfg = TableMgr.Singleton.GetSkillLvCfg(itemCfg.Value1, data.Level);

				IXUIObject obj = uiBehaviour.m_RandSkillList.GetItemByIndexOrAdd(index);

				XUISprite Icon = (XUISprite)obj.GetUIObject("Icon");
				Icon.SetSprite(skillCfg.Icon);

				XUITextPro Name = (XUITextPro)obj.GetUIObject("Name");
				Name.SetText(skillCfg.ShowName);

				XUITextPro Desc = (XUITextPro)obj.GetUIObject("Desc");
				Desc.SetText(skillLKvCfg.Describe);

				XUIButton uIButton = (XUIButton)obj.GetUIObject("Button");
				uIButton.RegisterDownEventHandler(OnButtonDown);
				uIButton.RegisterUpEventHandler(OnButtonUp);
				uIButton.Data = data;
			}

			uiBehaviour.m_RandSkillList.SetItemVisibleCount(randSkillList.Count);
		}

		XUIButton SelectButton;
		public bool OnButtonDown(IXUIObject obj)
		{
			SelectButton = (XUIButton)obj;

			XUISprite icon = (XUISprite)SelectButton.GetUIObject("Icon");
			weaponSetting.StartDragIcon(icon);
			return true;
		}

		public bool OnButtonUp(IXUIObject obj)
		{
			weaponSetting.EndDragIcon();

			if (!weaponSetting.HaveSelectButton())
				return true;

			Entity player = UIUtils.GetPlayer();

			TriggerData triggerData = weaponSetting.GetSelectTriggerData();
			ShopItemData shopSkillData = (ShopItemData)SelectButton.Data;

			ItemBattleCfg itemCfg = TableMgr.Singleton.GetItemBattleCfg(shopSkillData.ItemId);

			WeaponUtil.OnSetTriggerSkill(player, triggerData.triggerType, triggerData.index, itemCfg.Value1);

			XUISprite icon = (XUISprite)SelectButton.GetUIObject("Icon");
			weaponSetting.SetSelectButtonIcon(icon.sprite);

			IXUIObject selecgBg = SelectButton.GetUIObject("SelectBG");
			selecgBg.SetVisible(true);

			return true;
		}

		//======================================================================================
	}
}
