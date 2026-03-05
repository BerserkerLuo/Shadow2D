using System;
using UnityEngine;
using Client.UI.UICommon;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;
using UILib;
using ECS;
using Tools;

namespace Client.UI
{
	public class DlgMain : DlgBaseNew
	{
		public static DlgMain singleton
		{
			get
			{
				if (s_singleton == null)
				{
					s_singleton = new DlgMain();
					UIManager.Singleton.AddDlg(s_singleton);
				}
				return s_singleton;
			}
		}
		private static DlgMain s_singleton = null;


		public DlgMainBehaviour uiBehaviour
		{
			get
			{
				return (DlgMainBehaviour)m_uiBehaviour;
			}
		}

		public DlgMain()
		{
			m_uiBehaviour = new DlgMainBehaviour();
		}

		public override void Init()
		{
		}

		public override void Reset()
		{
		}

		public override void RegisterEvent() {
			uiBehaviour.m_WeaponButton.RegisterClickEventHandler(OnWeaponButtonClicked);
			uiBehaviour.m_AttrButton.RegisterClickEventHandler(OnLevelButtonClicked);
		}
		protected override void OnShow()
		{
			base.OnShow();
			Refresh();
		}

		protected override void OnRefresh()
		{
			base.OnRefresh();

			InitUI();
		}
		//============================================================================

		public bool OnWeaponButtonClicked(IXUIObject obj) {
			Util.TimePause();
			DlgWeaponUp.singleton.SetVisible(true);
			return true;
		}

		public bool OnLevelButtonClicked(IXUIObject obj){
			Util.TimePause();
			DlgLevelUp.singleton.OnLevelUp(1);
			return true;
		}

		//============================================================================

		public void InitUI() {
			uiBehaviour.m_Reload.SetVisible(false);
		}

		public void ShowAll() {
			ShowHP();
			ShowExp();
			ShowBullet();
		}

		public void ShowHP() {
			Entity player = UIUtils.GetPlayer();
			float curHp = AttrUtil.GetHP(player);
			float hpMax = AttrUtil.GetHPMax(player);
			if (hpMax == 0) hpMax = 1f;
			uiBehaviour.m_HPImg.SetFillAmount(curHp / hpMax);
			uiBehaviour.m_HPText.SetText($"{curHp}/{hpMax}");
		}

		public void ShowExp() {
			Entity player = UIUtils.GetPlayer();
			var comp = player.GetComponentData<ExpComponent>();
			uiBehaviour.m_ExpImg.SetFillAmount(comp.exp / comp.lvUpCost);
			uiBehaviour.m_LevelText.SetText($"Lv{comp.Level}");

			Debug.Log($"ShowExp {comp.exp} / {comp.lvUpCost}");
		}

		public void ShowBullet() {
			Entity player = UIUtils.GetPlayer();

			var comp = player.GetComponentData<WeaponSkillComponent>();
			int bulletTotal = (int)AttrUtil.GetBulletNum(player);
			uiBehaviour.m_BulletText.SetText($"{comp.BulletCount}/{bulletTotal}");
		}

		public void ShowReload(float time) {
			uiBehaviour.m_Reload.SetVisible(true);
			RectTransform rect = uiBehaviour.m_ReloadIndex.CachedRectTransform;
			rect.anchoredPosition = new Vector2(0, 0);

			Sequence seq1 = DOTween.Sequence();
			seq1.Append(rect.DOAnchorPosX(60, time).SetEase(Ease.OutExpo));
			seq1.AppendCallback(() => {uiBehaviour.m_Reload.SetVisible(false);});
		}

		public void OnCoinChange(int addCount) {
			Entity player = UIUtils.GetPlayer();
			int coinCount = BackpackUtil.GetCoinCount(player);
			uiBehaviour.m_CoinText.SetText(coinCount.ToString());
		}

		//血量
		//经验
		//子弹数量
		//加载子弹进度
	}
}
