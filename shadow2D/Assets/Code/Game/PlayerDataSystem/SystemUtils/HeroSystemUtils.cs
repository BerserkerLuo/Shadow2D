using Table;
using System.Collections.Generic;
using Tools;
using ECS;
using UnityEngine;

namespace PlayerSystemData
{
	static class HeroSystemUtils
	{
		public static HeroInfo GetRandHero() {
			HeroSystem heroSystem = SystemUtils.GetHeroSystem();
			if (heroSystem == null)
				return CreateRandHero();

			//HeroInfo retInfo = heroSystem.GetRandHeroInfo();
			//if (retInfo != null)
			//	return retInfo;

			return CreateRandHero();
		}

		public static HeroInfo CreateRandHero() {
			HeroInfo retInfo = new HeroInfo();

			//retInfo.exp = 0;
			//retInfo.professionId = 1;

			//retInfo.gender = LogicUtils.GetRand(0,100) > 50 ? 1 : 0;
			//retInfo.name = RandNameUtil.RandomName(retInfo.gender);

			//retInfo.skillList.Add(10, 1);

			//retInfo.baseAttrMap.Add(AttrType.Str, Util.GetRand(1, 10));
			//retInfo.baseAttrMap.Add(AttrType.Int, Util.GetRand(1, 10));
			//retInfo.baseAttrMap.Add(AttrType.Agi, Util.GetRand(1, 10));
			//retInfo.baseAttrMap.Add(AttrType.Phy, Util.GetRand(1, 10));
			//retInfo.baseAttrMap.Add(AttrType.Spi, Util.GetRand(1, 10));
			//retInfo.baseAttrMap.Add(AttrType.Luck, Util.GetRand(1, 10));

			//retInfo.baseAttrMap.Add(AttrType.HPMax, Util.GetRand(0, 100));
			//retInfo.baseAttrMap.Add(AttrType.MPMax, Util.GetRand(0, 100));
			//retInfo.baseAttrMap.Add(AttrType.Megic, Util.GetRand(1, 10));
			//retInfo.baseAttrMap.Add(AttrType.Atk, Util.GetRand(1, 10));
			//retInfo.baseAttrMap.Add(AttrType.Def, Util.GetRand(1, 10));
			//retInfo.baseAttrMap.Add(AttrType.MegDef, Util.GetRand(1, 10));
			//retInfo.baseAttrMap.Add(AttrType.Speed, Util.GetRand(3, 5));

			//HeroSystem heroSystem = SystemUtils.GetHeroSystem();
			//if (heroSystem != null)
			//	heroSystem.SetRandHeroInfo(retInfo);

			//SystemUtils.SetDataDirty();

			return retInfo;
		}

		public static void OnRecruitHero() {
			HeroSystem heroSystem = SystemUtils.GetHeroSystem();
			if (heroSystem == null)
				return;

			//heroSystem.OnRecruitHero();

			SystemUtils.SetDataDirty();
		}

		public static List<HeroInfo> GetHeroList()
		{
			List<HeroInfo> retList = new List<HeroInfo>();
			GetHeroList(retList);
			return retList;
		}
		public static void GetHeroList(List<HeroInfo> heroList) {
			HeroSystem heroSystem = SystemUtils.GetHeroSystem();
			if (heroSystem == null)
				return;

			//List<HeroInfo> tempList = heroSystem.GetHeroInfos();
			//heroList.AddRange(tempList);
		}

		public static HeroInfo GetHeroInfo(int heroId) {
			HeroSystem heroSystem = SystemUtils.GetHeroSystem();
			if (heroSystem == null)
				return null;

			return null;
		}

		public static bool IsRecruit(){
			HeroSystem heroSystem = SystemUtils.GetHeroSystem();
			if (heroSystem == null)
				return false;
			return false;
		}

		public static void DelHero(int heroId) {
			HeroSystem heroSystem = SystemUtils.GetHeroSystem();
			if (heroSystem == null)
				return;
			
		}
	}
}
