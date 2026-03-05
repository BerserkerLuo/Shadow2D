using System;
using UnityEngine;
using Client.UI.UICommon;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;
using UILib;
using Game;
using ECS;

namespace Client.UI
{
	public class DlgGameControl : DlgBaseNew
	{
		public static DlgGameControl singleton
		{
			get
			{
				if (s_singleton == null)
				{
					s_singleton = new DlgGameControl();
					UIManager.Singleton.AddDlg(s_singleton);
				}
				return s_singleton;
			}
		}
		private static DlgGameControl s_singleton = null;


		public DlgGameControlBehaviour uiBehaviour
		{
			get
			{
				return (DlgGameControlBehaviour)m_uiBehaviour;
			}
		}

		public DlgGameControl()
		{
			m_uiBehaviour = new DlgGameControlBehaviour();
		}

		public override int Layer
		{
			get { return 8; }
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

		public int controlState = 0;
		public int ControlState {
			get { return controlState; }
			set {
				controlState = value;
				//Debug.Log("Set ControlState " + controlState); 
			}
		}
		public  void Update_()
		{
			if (ControlState == 0){
				ControlPointSelect();
				MoveControl();
			}

			if (ControlState == 0 || ControlState == 1)
				ControlCamera();

			if (ControlState == 0 || ControlState == 2)
				ControlSelectBox();
		}


		//================================================================================================================
		//相机控制
		private Vector2 OldStepPos;
		private Vector2 CameraStartPos;
		private Vector3 CameraWorldStartPos;
		private Vector2 CameraMaxPos = new Vector2(10, 10);
		private Vector2 CameraMinPos = new Vector2(0, 0);

		private void ControlCamera() {

			if (Input.GetMouseButtonDown(1)) {
				CameraStartPos = Input.mousePosition;
				CameraWorldStartPos = CameraMgr.CameraRoot.position;
			}

			if (Input.GetMouseButton(1)) {

				Vector2 newPos = Input.mousePosition;
				if (ControlState != 1 && CameraStartPos.x == newPos.x && CameraStartPos.y == newPos.y)
					return;
				ControlState = 1;

				Vector2 stepPos = (newPos - CameraStartPos) * 0.1f;

				Vector3 newWorldPos = new Vector3(CameraWorldStartPos.x - stepPos.x, CameraWorldStartPos.y - stepPos.y, CameraWorldStartPos.z);

				CameraMgr.CameraRoot.position = Vector3.Lerp(CameraMgr.CameraRoot.position, newWorldPos, 0.3f);

				//ClimpCamera();
				//if (ClimpCamera())
				//	CameraStartPos = newPos + OldStepPos;
				//else
				//	OldStepPos = stepPos;
			}
			if (ControlState == 1 && Input.GetMouseButtonUp(1)) {
				ControlState = 0;
			}
		}

		private bool ClimpCamera() {
			bool ret = false;
			Vector2 pos = CameraMgr.CameraRoot.position;
			if (pos.x < CameraMinPos.x) { pos.x = CameraMinPos.x; ret = true; }
			else if (pos.x > CameraMaxPos.x) { pos.x = CameraMaxPos.x; ret = true; }
			if (pos.y < CameraMinPos.y) { pos.y = CameraMinPos.y; ret = true; }
			else if (pos.y > CameraMaxPos.y) { pos.y = CameraMaxPos.y; ret = true; }
			CameraMgr.CameraRoot.position = pos;
			return ret;
		}

		public void SetClimpCameraPos(Vector2 minPos,Vector2 maxPos) {
			CameraMinPos = minPos;
			CameraMaxPos = maxPos;
			ClimpCamera();
		}

		//================================================================================================================
		//选择框

		private Vector2 selectStartPos;
		private Vector2 SelectEndPos;
		private void ControlSelectBox() {
			if (Input.GetMouseButtonDown(0))
			{
				selectStartPos = Input.mousePosition;
			}

			if (Input.GetMouseButton(0))
			{
				SelectEndPos = Input.mousePosition;
				if (ControlState != 2 && selectStartPos.x == SelectEndPos.x && selectStartPos.y == SelectEndPos.y)
					return;

				ControlState = 2;
				uiBehaviour.m_SelectBox.SetVisible(true);
				UpdateBox();
			}

			if (ControlState == 2 && Input.GetMouseButtonUp(0))
			{
				ControlState = 0;
				uiBehaviour.m_SelectBox.SetVisible(false);
				BoxSelect();
			}
		}

		private void UpdateBox()
		{
			Vector2 boxStart = selectStartPos;
			Vector2 size = SelectEndPos - selectStartPos;

			if (size.x < 0) {
				boxStart.x = SelectEndPos.x;
				size.x = -size.x;
			}

			if (size.y < 0) {
				boxStart.y = SelectEndPos.y;
				size.y = -size.y;
			}

			RectTransform selectionBox = uiBehaviour.m_SelectBox.CachedRectTransform;
			selectionBox.anchoredPosition = boxStart;
			selectionBox.sizeDelta = size;
		}

		private void BoxSelect() {

			Vector2 posA = Camera.main.ScreenToWorldPoint(selectStartPos);
			Vector2 posB = Camera.main.ScreenToWorldPoint(SelectEndPos);
			LayerMask layerMask = 1 << LayerMask.NameToLayer("Role") ;
			Collider2D[] hits = Physics2D.OverlapAreaAll(posA, posB, layerMask);

			List<Collider2D> list = new List<Collider2D>();
			list.AddRange(hits);
			OnSelectObject(list);
		}

		//================================================================================================================
		//点选

		private void ControlPointSelect() {
			if (Input.GetMouseButtonUp(0)) {
				Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

				if (hit.collider != null)
					OnSelectObject(new List<Collider2D>() { hit.collider });
				else
					OnSelectObject(new List<Collider2D>());
			}
		}
		//================================================================================================================
		//移动控制
		private void MoveControl() {
			if (Input.GetMouseButtonUp(1)) {
				Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

				EffectUtils.ShowEffect("Flag", worldPos, 0.5f);
				LogicUtils.OnControlMove(worldPos);
			}
		}

		//================================================================================================================
		List<Collider2D> OldSelectList = new List<Collider2D>();
		private void OnSelectObject(List<Collider2D> objList) {
			SetMat(OldSelectList, "Shader/Normal");
			SetMat(objList, "Shader/OutLineYellow");
			OldSelectList = objList;

			objList.Sort((a,b)=> {
				Vector2 step = a.transform.position - b.transform.position;
				if (Mathf.Abs(step.y) > 0.3f)
					return step.y > 0 ? 1 : step.y < 0 ? -1 : 0;
				return step.x > 0 ? 1 : step.x < 0 ? -1 : 0;
			});


			List<Entity> selectList = new List<Entity>();
			foreach (Collider2D obj in objList) {
				OwnerScript script = obj.gameObject.GetComponent<OwnerScript>();
				if (script == null)
					continue;
				selectList.Add(script.Owner);
			}

			LogicUtils.SetSelectEntity(selectList);
		}

		//public void ShowList(List<Collider2D> list)
		//{
		//	string str = "\n";
		//	for (int it = 0; it < list.Count; ++it){
		//		Vector2 pos = list[it].transform.position;
		//		str += " " + pos.ToString();
		//		if ((it + 1) % 3 == 0)
		//			str += "\n";
		//	}
		//	Debug.Log(str);
		//}

		private void SetMat(List<Collider2D> objList, string matPath) {
			foreach (var obj in objList)
			{
				SpriteRenderer renderer = obj.gameObject.GetComponent<SpriteRenderer>();
				if (renderer == null)
					continue;

				renderer.material = (Material)Resources.Load(matPath);
			}
		}
	}
}
