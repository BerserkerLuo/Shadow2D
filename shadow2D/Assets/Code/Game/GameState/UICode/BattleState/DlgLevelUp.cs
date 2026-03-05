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

namespace Client.UI
{
	public class DlgLevelUp : DlgBaseNew
	{
		public static DlgLevelUp singleton
		{
			get
			{
				if (s_singleton == null)
				{
					s_singleton = new DlgLevelUp();
					UIManager.Singleton.AddDlg(s_singleton);
				}
				return s_singleton;
			}
		}
		private static DlgLevelUp s_singleton = null;


		public DlgLevelUpBehaviour uiBehaviour
		{
			get
			{
				return (DlgLevelUpBehaviour)m_uiBehaviour;
			}
		}

		public DlgLevelUp()
		{
			m_uiBehaviour = new DlgLevelUpBehaviour();
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

		}

		private bool OnRandButtonClick(IXUIObject uiObject)
		{
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
		}


		private int AddLevel = 0;
		public void OnLevelUp(int addLevel) {

			SetVisible(true);

			Util.TimePause();
			AddLevel = addLevel;

			RandOptions();
		}

		public void RandOptions()
		{
			Entity player = UIUtils.GetPlayer();
			List<ShopItemData> itemList = SkillRandUtils.RandStatusList(player);

			for (int index = 0; index < itemList.Count; ++index){
				ShopItemData item = itemList[index];

				ItemBattleCfg itemCfg = TableMgr.Singleton.GetItemBattleCfg(item.ItemId);
				StatusCfg statusCfg = TableMgr.Singleton.GetStatusCfg(itemCfg.Value1);
				if (statusCfg == null)
					continue;

				IXUIListItem uiItem = uiBehaviour.m_SelectSkillList.GetItemByIndexOrAdd(index);

				XUISprite icon = (XUISprite)uiItem.GetUIObject("Icon");
				icon.SetSprite(statusCfg.Icon);

				XUITextPro name = (XUITextPro)uiItem.GetUIObject("Name");
				name.SetText(statusCfg.Name);

				XUITextPro level = (XUITextPro)uiItem.GetUIObject("Level");
				level.SetText($"lv{item.Level}");

				XUITextPro desc = (XUITextPro)uiItem.GetUIObject("Desc");
				desc.SetText(statusCfg.Discribe);

				XUIButton button = (XUIButton)uiItem.GetUIObject("Button");
				button.RegisterClickEventHandler(OnSelectOpention);
				button.Data = item;
			}

			uiBehaviour.m_SelectSkillList.SetItemVisibleCount(itemList.Count);
		}  

		public bool OnSelectOpention(IXUIObject obj) {
			XUIButton button = (XUIButton)obj;

			ShopItemData item = (ShopItemData)button.Data;
			Entity player = UIUtils.GetPlayer();
			ItemRewardUitl.OnGiveItem(player, item.ItemId,1);

			SelectEnd();
			return true;
		}

		public void SelectEnd() {
			AddLevel -= 1;
			if (AddLevel <= 0){
				SetVisible(false);
				Util.TimeRun();
			}
			RandOptions();
		}
	}
}
