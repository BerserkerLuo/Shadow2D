using System;
using UnityEngine;
using Client.UI.UICommon;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;
using UILib;
using Table;
using UnityEngine.Events;

namespace Client.UI
{
	public class DlgDialogue : DlgBaseNew
	{
		 public static DlgDialogue singleton
		{
			get
			{
				if (s_singleton == null)
				{
					s_singleton = new DlgDialogue();
					UIManager.Singleton.AddDlg(s_singleton);
				}
				return s_singleton;
			}
		}
		 private static DlgDialogue s_singleton = null;


		public DlgDialogueBehaviour uiBehaviour
		{
			get
			{
				return (DlgDialogueBehaviour)m_uiBehaviour;
			}
		}

		public DlgDialogue()
		{
			m_uiBehaviour = new DlgDialogueBehaviour();
		}

		public override void Init()
		{
		}

		public override void Reset()
		{
		}

		public override void RegisterEvent()
		{
			uiBehaviour.m_Button_Fast.RegisterClickEventHandler(this.OnButton_FastClick);
			uiBehaviour.m_Button_Skip.RegisterClickEventHandler(this.OnButton_SkipClick);
			uiBehaviour.m_Button_Next.RegisterClickEventHandler(this.OnButton_NextClick);
		}


		private bool OnButton_FastClick(IXUIObject uiObject)
		{
            TextSpeed = 0.025f - TextSpeed;
			IsFast = !IsFast;

			uiBehaviour.m_Text_Dialogue.SetTypeTextSpeed(TextSpeed);

            return true;
		}

		private bool OnButton_SkipClick(IXUIObject uiObject)
		{
			OnEnd();
			return true;
		}

		private bool OnButton_NextClick(IXUIObject uiObject) {
			Next();
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

		public List<string> m_dialougueList = new List<string>();
		public int m_index = 0;
		public UnityAction callBackFun = null;
		public float TextSpeed = 0.02f;
		public bool IsFast = false;

        public void ShowDialogue(string dialougue, UnityAction finish = null) {
			m_dialougueList.Clear();
			m_dialougueList.AddRange(dialougue.Split("\n"));
			m_index = -1;
			callBackFun = finish;

			SetVisible(true);

            OnRefreshDialogue();
        }

		public void OnRefreshDialogue() {
			m_index += 1;
			if (m_index >= m_dialougueList.Count)
				return;

            uiBehaviour.m_Text_Dialogue.TypeText(m_dialougueList[m_index], TextSpeed, AutoNext);
        }

		public void AutoNext() {
			if (!IsFast) return;
			OnRefreshDialogue();
		}
		public void Next() {
			if (m_index >= m_dialougueList.Count-1)
				OnEnd(); 
			else
				OnRefreshDialogue();
		}

		public void OnEnd() {
			SetVisible(false);
			callBackFun?.Invoke();
		}

	}
}
