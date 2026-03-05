
using Client.UI.UICommon;
using UILib;
using Battle;
using DataDefine;

namespace Client.UI
{
	public class DlgGameResult : DlgBaseNew
	{
		 public static DlgGameResult singleton
		{
			get
			{
				if (s_singleton == null)
				{
					s_singleton = new DlgGameResult();
					UIManager.Singleton.AddDlg(s_singleton);
				}
				return s_singleton;
			}
		}
		 private static DlgGameResult s_singleton = null;


		public DlgGameResultBehaviour uiBehaviour
		{
			get
			{
				return (DlgGameResultBehaviour)m_uiBehaviour;
			}
		}

		public DlgGameResult()
		{
			m_uiBehaviour = new DlgGameResultBehaviour();
		}

		public override void Init()
		{
		}

		public override void Reset()
		{
		}

		public override void RegisterEvent()
		{
			uiBehaviour.m_OkButton.RegisterClickEventHandler(this.OnOkButtonClick);
		}

		private bool OnOkButtonClick(IXUIObject uiObject)
		{
			SetVisible(false);
			BattleMgr.Singleton.EndGame();
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

		public void OnShowResult(GameResultInfo result) {
			SetVisible(true);

			uiBehaviour.m_LvText.SetText(result.level.ToString());
			uiBehaviour.m_LifeText.SetText(result.gameTime.ToString());
			uiBehaviour.m_KillText.SetText(result.killCount.ToString());
		}

	}
}
