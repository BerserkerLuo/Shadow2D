using System;
using UnityEngine;
using Client.UI.UICommon;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;
using UILib;

namespace Client.UI
{
	public class DlgMessage : DlgBaseNew
	{
		 public static DlgMessage singleton
		{
			get
			{
				if (s_singleton == null)
				{
					s_singleton = new DlgMessage();
					UIManager.Singleton.AddDlg(s_singleton);
				}
				return s_singleton;
			}
		}
		 private static DlgMessage s_singleton = null;


		public DlgMessageBehaviour uiBehaviour
		{
			get
			{
				return (DlgMessageBehaviour)m_uiBehaviour;
			}
		}

		public DlgMessage()
		{
			m_uiBehaviour = new DlgMessageBehaviour();
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
		}

		Tween tween;
		public void ShowMessage(string message) {

			Debug.Log($"ShowMessage [{message}]");

			SetVisible(true);

			tween?.Kill();

			XUITextPro textPro = (XUITextPro)uiBehaviour.m_Text;
			textPro.TextMeshPro.alpha = 1;
			tween = textPro.TextMeshPro.DOFade(0,2);
			uiBehaviour.m_Text.SetText(message);
		}
	}
}
