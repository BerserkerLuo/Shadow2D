using System;
using UnityEngine;
using Client.UI.UICommon;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;
using UILib;
using Tool;
using UnityEngine.Events;
using TMPro;
using ECS;

namespace Client.UI
{

	public class FlyItem
	{
		public float endTime;
		public Vector3 follow3DPos;

		public RectTransform textRoot;
		public RectTransform textTrans;
		public TextMeshProUGUI text;
		public XUIListItem flyUIListItem;
	}
	public class FlyTextItemPool : ObjectPool<FlyItem>
	{
		public static FlyItem Get(Func<XUIListItem> createNewFun)
		{
			FlyItem ret = null;
			if (WaitList.Count < 1)
			{
				ret = CreateNew(createNewFun);
				++NewCount;
			}
			else
			{
				ret = WaitList.Dequeue();
			}
			ret.flyUIListItem.SetVisible(true);
			UseList.Add(ret);
			return ret;
		}

		private static FlyItem CreateNew(Func<XUIListItem> unityAction) {
			FlyItem flyItem = new FlyItem();
			flyItem.flyUIListItem = unityAction();
			flyItem.textRoot = flyItem.flyUIListItem.CachedRectTransform;
			XUIObject textObj = (XUIObject)flyItem.flyUIListItem.GetUIObject("Text");
			if (textObj != null)
			{
				flyItem.textTrans = textObj.CachedRectTransform;
				flyItem.text = textObj.GetComponent<TextMeshProUGUI>();
			}
			return flyItem;
		}

		public static new void Return(FlyItem ret)
		{
			ret.flyUIListItem.SetVisible(false);
			ObjectPool<FlyItem>.Return(ret);
		}

		public static new void ReturnAll()
		{
			foreach (var it in UseList)
			{
				it.flyUIListItem.SetVisible(false);
				WaitList.Enqueue(it);
			}
			UseList.Clear();
		}

		public static HashSet<FlyItem> GetUseList() {
			return UseList;
		}
	}

	public class DlgFlyText : DlgBaseNew
	{
		 public static DlgFlyText singleton
		{
			get
			{
				if (s_singleton == null)
				{
					s_singleton = new DlgFlyText();
					UIManager.Singleton.AddDlg(s_singleton);
				}
				return s_singleton;
			}
		}
		 private static DlgFlyText s_singleton = null;


		public DlgFlyTextBehaviour uiBehaviour
		{
			get
			{
				return (DlgFlyTextBehaviour)m_uiBehaviour;
			}
		}

		public DlgFlyText()
		{
			m_uiBehaviour = new DlgFlyTextBehaviour();
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

		public XUIListItem CreateNewItem() {
			return (XUIListItem)uiBehaviour.m_TextItemList.AddListItem();
		}

		const float moveTime = 0.5f;
		const float lifeTime = 1f;
		public void ShowFlyText(Vector3 pos, string text, int textType, Vector3 damageDire) {

			FlyItem item = FlyTextItemPool.Get(CreateNewItem);

			item.textTrans.anchoredPosition = Vector3.zero;
			item.textTrans.localScale = Vector3.one;

			item.follow3DPos = pos;
			item.text.text = text;
			item.text.color = Color.red;
			item.endTime = Time.time + lifeTime;

			OnDamageTextMove(item,damageDire);

			UpdatePos(item);
		}

		public void OnDamageTextMove(FlyItem item, Vector3 damageDire) {
			Sequence seq1 = DOTween.Sequence();
			seq1.Append(item.textTrans.DOAnchorPosY(50, 0.2f).SetEase(Ease.OutExpo));
			seq1.Append(item.textTrans.DOAnchorPosY(0, 0.2f).SetEase(Ease.OutExpo));
			seq1.Append(item.textTrans.DOScaleX(1.5f, 0.15f).SetEase(Ease.OutExpo));
			seq1.Append(item.textTrans.DOScaleX(1, 0.15f).SetEase(Ease.OutExpo));

			float randX = UnityEngine.Random.Range(50, 100);
			if (damageDire == Vector3.zero && UnityEngine.Random.Range(0, 1000) > 500)
				randX = 0 - randX;
			else if (damageDire.x < 0)
				randX = 0 - randX;
			item.textTrans.DOAnchorPosX(randX, 0.8f).SetEase(Ease.OutExpo);
		}

		List<FlyItem> expireItemList = new List<FlyItem>();
		public override void Update()
		{
			float now = Time.time;

			HashSet<FlyItem> UseList = FlyTextItemPool.GetUseList();
			foreach (var it in UseList)
			{
				UpdatePos(it);
				if (it.endTime > now)
					continue;
				expireItemList.Add(it);
			}

			if (expireItemList.Count == 0)
				return;

			foreach (var it in expireItemList)
				FlyTextItemPool.Return(it);

			expireItemList.Clear();
		}

		public void UpdatePos(FlyItem item) {
			item.textRoot.anchoredPosition = WorldToUILocalPos(uiBehaviour.m_TextItemList.CachedRectTransform,item.follow3DPos);
		}

		public static Vector3 WorldToUILocalPos(Transform targetTransform, Vector3 worldPos)
		{
			Camera uiCamera = UnityGameEntry.Instance.UICamera;
			Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
			RectTransformUtility.ScreenPointToLocalPointInRectangle(targetTransform.GetComponent<RectTransform>(),screenPos,uiCamera,out Vector2 localPoint);
			//DebugUtils.DebugLog("WorldToUILocalPos worldPos {} screenPos {} localPoint {}", worldPos, screenPos, localPoint);

			// 返回本地坐标
			return localPoint;
		}

	}
}
