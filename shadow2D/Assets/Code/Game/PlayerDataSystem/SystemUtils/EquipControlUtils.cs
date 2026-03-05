using Table;
using System.Collections.Generic;
using Tools;
using ECS;
using UnityEngine;

namespace PlayerSystemData
{
	static class EquipControlUtils
	{
		public static bool OnEquipItem(int heroId, int itemId) {
			HeroInfo heroInfo = HeroSystemUtils.GetHeroInfo(heroId);
			if (heroInfo == null)
				return false;
			return OnEquipItem(heroInfo,itemId);
		}

		public static bool OnEquipItem(HeroInfo heroInfo, int itemId) {
			ItemCfg itemCfg = TableMgr.Singleton.GetItemCfg(itemId);
			if (itemCfg == null || itemCfg.ItemType != 2)
				return false;

			ItemInfo itemInfo = BackpakcSystemUtils.GetItemInfoByItemID(itemId);
			if (itemInfo == null)
				return false;

			EquipPort port = GetEquipPort(heroInfo,itemCfg);
			//int oldId = heroInfo.equipList.GetValueOrDefault(port, -1);
			//if (oldId != -1)
			//	BackpakcSystemUtils.TryChangeItemNum(oldId, 1);

			//BackpakcSystemUtils.TryChangeItemNum(itemId, -1);

			//heroInfo.equipList.Remove(port);
			//heroInfo.equipList.Add(port,itemId); 

			SystemUtils.SetDataDirty();

			return true;
		}

		public static EquipPort GetEquipPort(HeroInfo heroInfo,int itemId) {
			ItemCfg itemCfg = TableMgr.Singleton.GetItemCfg(itemId);
			if (itemCfg == null || itemCfg.ItemType != 2)
				return EquipPort.Error;
			return GetEquipPort(heroInfo, itemCfg);
		}

		public static EquipPort GetEquipPort(HeroInfo heroInfo, ItemCfg itemCfg) {
			EquipPort port = EquipPort.Weapon;
			//if (itemCfg.SubType == 2)
			//	port = GetEquipPort(heroInfo.equipList, EquipPort.Armor1, EquipPort.Armor2);
			//else if (itemCfg.SubType == 3)
			//	port = GetEquipPort(heroInfo.equipList, EquipPort.Annex1, EquipPort.Annex2, EquipPort.Annex3);
			return port;
		}

		public static EquipPort GetEquipPort(Dictionary<EquipPort, int> equipList, params EquipPort[] ports) {

			foreach (EquipPort port in ports) {
				int oldId = equipList.GetValueOrDefault(port, -1);
				if (oldId == -1)
					return port;

				ItemCfg itemCfg = TableMgr.Singleton.GetItemCfg(oldId);
				if (itemCfg == null)
					return port;
			}

			return ports[ports.Length - 1];
		}

		public static void OnUnLoadItem(int heroId,int index) {
			HeroInfo heroInfo = HeroSystemUtils.GetHeroInfo(heroId);
			if (heroInfo == null)
				return;

			//int oldId = heroInfo.equipList.GetValueOrDefault((EquipPort)index,-1);
			//if (oldId == -1)
			//	return;

			//BackpakcSystemUtils.TryChangeItemNum(oldId, 1);
			//heroInfo.equipList.Remove((EquipPort)index);

			SystemUtils.SetDataDirty();
		}
	}
}
