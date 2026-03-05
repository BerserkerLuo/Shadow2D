using System;
using UnityEngine;
using Client.UI.UICommon;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.UI;
using UILib;
using ECS;
using PlayerSystemData;
using Table;
using Game;

namespace Client.UI
{
	public class DlgSelectMap : DlgBaseNew
	{
		 public static DlgSelectMap singleton
		{
			get
			{
				if (s_singleton == null)
				{
					s_singleton = new DlgSelectMap();
					UIManager.Singleton.AddDlg(s_singleton);
				}
				return s_singleton;
			}
		}
		 private static DlgSelectMap s_singleton = null;


		public DlgSelectMapBehaviour uiBehaviour
		{
			get
			{
				return (DlgSelectMapBehaviour)m_uiBehaviour;
			}
		}

		public DlgSelectMap()
		{
			m_uiBehaviour = new DlgSelectMapBehaviour();
		}

		public override void Init()
		{
		}

		public override void Reset()
		{
		}

		public override void RegisterEvent()
		{
			uiBehaviour.m_Start.RegisterClickEventHandler(OnStartClicked);
			uiBehaviour.m_Return.RegisterClickEventHandler(OnReturnClicked);
			
		}

		protected override void OnShow()
		{
			base.OnShow();
			Refresh();
		}

		protected override void OnRefresh()
		{
			base.OnRefresh();

			ShowMapList();
		}


		public bool OnStartClicked(IXUIObject obj) {
			DlgStartControl.singleton.ChangeToStartGame();
			return true;
		}

		public bool OnReturnClicked(IXUIObject obj){
			DlgStartControl.singleton.ChangeToSelectWeapon();
			return true;
		}

		public void SetButtonPush(XUIButton button, bool flag){
			if (button == null) return;
			XUITextPro text = (XUITextPro)button.GetUIObject("Name");
			if (flag) text.SetColor("#CFAB8D");
			else text.SetColor("#FFF5F2");
		}

		//==============================================================
		public void ShowMapList(){
			int selectMapId = OperationSystem.Singleton.GetMapId();
			List<MapCfg> mapCfgs = TableMgr.Singleton.tables.MapCfgMgr.DataList;

			for (int index = 0; index < mapCfgs.Count; ++index){
				MapCfg mapCfg = mapCfgs[index];
				XUIButton button = ShowMap(mapCfg, index);

				if(selectMapId == 0 && index == 0)
					OnSelectMap(button);
				else if(selectMapId == mapCfg.Id)
					OnSelectMap(button);
			}
		}

		public XUIButton ShowMap(MapCfg mapCfg, int index){
			IXUIObject item = uiBehaviour.m_SelectMapList.GetItemByIndexOrAdd(index);
			XUIButton button = (XUIButton)item.GetUIObject("Button");
			button.Data = mapCfg.Id;
			button.RegisterClickEventHandler(OnMapPointClick);
			return button;
		}

		public bool OnMapPointClick(IXUIObject obj){
			OnSelectMap((XUIButton)obj);
			return true;
		}

		//==============================================================
		public void ShowMapRankList(int mapId) {

			Debug.Log("ShowMapRankList mapId" + mapId);

			List<MapRankCfg> allList = TableMgr.Singleton.tables.MapRankCfgMgr.DataList;
			List<MapRankCfg> rankCfgList = new List<MapRankCfg>();
			foreach (var it in allList) 
				if (it.MapId == mapId)
					rankCfgList.Add(it);

			Debug.Log("allList Count" + allList.Count);
			Debug.Log("rankCfgList Count" + rankCfgList.Count);

			int selectMapRank = OperationSystem.Singleton.GetMapRank(mapId);
			for (int index =0;index < rankCfgList.Count;++index) {
				MapRankCfg rankCfg = rankCfgList[index];
				XUIButton button = ShowMapRank(rankCfg,index);
				if (selectMapRank == rankCfg.Rank)
					OnSelectMapRank(button);
			}
		}

		public XUIButton ShowMapRank(MapRankCfg rankCfg,int index) {
			IXUIObject item = uiBehaviour.m_RankList.GetItemByIndexOrAdd(index);
			XUIButton button = (XUIButton)item.GetUIObject("Button");
			button.Data = rankCfg.Rank;

			XUITextPro text = (XUITextPro)item.GetUIObject("Name");
			text.SetText(rankCfg.Name);

			bool IsPass = MapRankSystem.Singleton.GetMapPass(rankCfg.Condition);
			if (IsPass){
				IXUIObject lockObj = item.GetUIObject("Lock");
				lockObj.SetVisible(false);
				button.RegisterClickEventHandler(OnMapRankPointClick);
				button.scale = 1.1f;
				text.SetColor("#FFF5F2");
			}
			else {
				button.RegisterClickEventHandler(OnMapRankLockPointClick);
				button.scale = 1.0f;
				text.SetColor("#C9CDCF");
			}

			return button;
		}

		public bool OnMapRankLockPointClick(IXUIObject obj) {
			DlgMessage.singleton.ShowMessage("请先通过上一难度!");
			return true;
		}

		public bool OnMapRankPointClick(IXUIObject obj){
			OnSelectMapRank((XUIButton)obj);
			return true;
		}

		//==============================================================
		public void ShowRankAttr() {
			MapRankCfg mapRankCfg = TableMgr.Singleton.GetMapRankCfg(mapId , mapRank);
			if (mapRankCfg == null)
				return;
			String[] strs = mapRankCfg.Describe.Split(" ");
			int max = Math.Max(strs.Length, uiBehaviour.m_AttrList.Count);
			for (int index = 0; index < max; ++index) {
				IXUIObject obj = uiBehaviour.m_AttrList.GetItemByIndexOrAdd(index);
				obj.SetVisible(index < strs.Length);
				if (index >= strs.Length)
					continue;
		
				XUITextPro text = (XUITextPro)obj.GetUIObject("Text");
				text.SetText(strs[index]);
			}
		}

		//==============================================================

		XUIButton mapButton;
		int mapId = 0;
		int mapRank = 0;
		public void OnSelectMap(XUIButton button) {

			int newMapId = (int)button.Data;
			if (newMapId == mapId)
				return;
			mapId = newMapId;

			SetButtonPush(mapButton,false);
			mapButton = button;

			OperationSystem.Singleton.SetMapId(mapId);

			ShowMapRankList(mapId);

			SetButtonPush(button,true);
		}

		XUIButton mapRankButton;
		public void OnSelectMapRank(XUIButton button) {
			int newMapRank = (int)button.Data;
			if (newMapRank == mapRank)
				return;
			mapRank = newMapRank;

			SetButtonPush(mapRankButton, false);
			mapRankButton = button;

			
			OperationSystem.Singleton.SetMapRank(mapId, mapRank);

			ShowRankAttr();

			SetButtonPush(button,true);

			Debug.Log("OnSelectMapRank "+ mapRank);
		}

	}
}
