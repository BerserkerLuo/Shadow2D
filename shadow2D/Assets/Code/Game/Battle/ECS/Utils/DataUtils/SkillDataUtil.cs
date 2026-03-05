using Table;
using System.Collections.Generic;
using static UnityEngine.EventSystems.EventTrigger;
using System;

namespace ECS
{

    class SkillDataUtil
    {
        #region<---技能数据--->

        public static SkillInfo GetSkillInfo(SkillComponent skillcom,int skillId,bool notExistAdd = false) {
            SkillInfo retInfo = null;
            skillcom.SkillInfos.TryGetValue(skillId,out retInfo);
            if (retInfo == null && notExistAdd){
                retInfo = new SkillInfo();
                retInfo.skillId = skillId;
                retInfo.skillCfg = SkillCfgUtil.GetSkillCfg(skillId);
                skillcom.SkillInfos.Add(skillId, retInfo);
            }
            return retInfo;
        }

        // 初始化技能等级
        public static SkillInfo AddSkill(Entity entity, int skillId, int addLevel = 1)
        {
            SkillComponent skillcom = entity.GetComponentData<SkillComponent>();
            if (skillcom == null)
                return null;

            SkillInfo skillInfo = GetSkillInfo(skillcom, skillId,true);
            AddSkillLevel(skillcom,skillInfo, addLevel);

            UpdateCDUnit(entity, skillId, skillInfo.skillLevel);

            return skillInfo;
        }

        //添加技能等级
        public static void AddSkillLevel(SkillComponent skillcom, SkillInfo skillInfo, int addLevel = 1)
        {
            int TargetLevel = skillInfo.skillLevel + addLevel;

            SkillLvCfg skilllvCfg = TableMgr.Singleton.GetSkillLvCfg(skillInfo.skillId, TargetLevel);
            if (skilllvCfg != null) {
                skillInfo.skillLevel = TargetLevel;
                skillInfo.skillLvCfg = skilllvCfg;
                return;
            }

            if (addLevel > 1){
                AddSkillLevel(skillcom, skillInfo, addLevel - 1);
                return;
            }

            skillInfo.skillLevel = 0;
            skillcom.SkillInfos.Remove(skillInfo.skillId);
            skillcom.m_dicID2CD.Remove(skillInfo.skillId);
        }

        //获取真实技能等级
        public static int GetRealSkillLevel(Entity entity, int skillId)
        {
            SkillComponent skillcom = entity.GetComponentData<SkillComponent>();
            if (skillcom == null)
                return 0;

            SkillCfg skillCfg = TableMgr.Singleton.GetSkillCfg(skillId);
            if (skillCfg == null)
                return 0;

            if (skillCfg.MasterSkill != skillId)
                skillId = skillCfg.MasterSkill;

            SkillInfo skillInfo = GetSkillInfo(skillcom, skillId);
            if (skillInfo == null)
                return 0;
            return skillInfo.skillLevel;
        }

        //获取技能等级 [我希望在没有获得技能的情况下也可以把技能释放出去 所以默认返回1 -- 23-09-10 berserker ]
        public static int GetSkillLevel(Entity entity, int skillId)
        {
            int level = GetRealSkillLevel(entity, skillId);
            if (level < 1)
                return 1;
            return level;
        }


        //获得一个可用的技能 [CD 消耗]
        public static int GetFreeSkill(Entity e,int targetType)
        {
            SkillComponent skillcom = e.GetComponentData<SkillComponent>();
            if (skillcom == null)
                return -1;

            foreach (var pair in skillcom.SkillInfos) {
                if (pair.Value.skillCfg.TargetType != targetType)
                    continue;
                if (!CheckSkillCanCast(e, pair.Key))
                    continue;
                return pair.Key;
            }
            return -1;
        }

        public static void SetBaseNormalAtkId(Entity e,int skillId) {
            SkillComponent skillcom = e.GetComponentData<SkillComponent>();
            if (skillcom == null)
                return;
            skillcom.BaseNormalAtkId = skillId;
        }

        public static void SetWeaponNormalAtkId(Entity e, int skillId){
            SkillComponent skillcom = e.GetComponentData<SkillComponent>();
            if (skillcom == null)
                return;
            skillcom.WeaponNormalAtkId = skillId;
        }

        public static int GetNormalAtkId(SkillComponent skillcom)        {
            if (skillcom.WeaponNormalAtkId != 0)
                return skillcom.WeaponNormalAtkId;
            return skillcom.BaseNormalAtkId;
        }
        public static List<SkillInfo> GetCanUseSkillList(Entity e, int targetType) {
            List<SkillInfo> retList = new List<SkillInfo>();
            SkillComponent skillcom = e.GetComponentData<SkillComponent>();
            if (skillcom == null)
                return retList;

            int normalAtkId = GetNormalAtkId(skillcom);

            foreach (var it in skillcom.SkillInfos){
                SkillCfg skillCfg = it.Value.skillCfg;

                if (skillCfg.TargetType != targetType)
                    continue;

                if (skillCfg.SkillType == 2)
                    continue;

                if (skillCfg.SkillType == 3 && skillCfg.ID != normalAtkId)
                    continue;

                if (!CheckSkillCanCast(e, it.Key))
                    continue;

                retList.Add(it.Value);
            }
            return retList;
        }

        #endregion

        #region<---技能cd--->
        public static void UpdateCDUnit(Entity e, int skillId, int level)
        {
            SkillComponent skillcom = e.GetComponentData<SkillComponent>();
            if (skillcom == null)
                return;

            SkillLvCfg skillLvCfg = TableMgr.Singleton.GetSkillLvCfg(skillId, level);
            if (skillLvCfg == null)
                return;

            float baseCD = skillLvCfg.CD;

            SkillCDUnit skillCDUnit = new SkillCDUnit();
            skillCDUnit.cd = baseCD;
            skillCDUnit.groupId = skillId;

            skillcom.m_dicID2CD[skillId] = skillCDUnit;
        }

        public static void StartCD(Entity e, int skillId)
        {
            SkillComponent skillcom = e.GetComponentData<SkillComponent>();
            if (skillcom == null)
                return;

            SkillCDUnit skillCDUnit;
            bool exist = skillcom.m_dicID2CD.TryGetValue(skillId, out skillCDUnit);
            if (!exist)
                return;

            skillCDUnit.startTime = LogicUtils.GetTime(e);
            //var cdper = 1 - AttrUtil.GetCdReduce(e);
            //if (cdper <= 0)
            //    cdper = 0f;

            int cdper = 1;

            skillCDUnit.endTime = skillCDUnit.startTime + skillCDUnit.cd * cdper;

            //cd组处理
            int srcGroupId = skillCDUnit.groupId;

            foreach (int tmpSkillId in skillcom.m_dicID2CD.Keys)
            {
                SkillCDUnit tmpCDUnit = skillcom.m_dicID2CD[tmpSkillId];
                int tmpGroupId = tmpCDUnit.groupId;
                if (tmpGroupId != srcGroupId)
                    continue;

                if (tmpCDUnit.endTime > skillCDUnit.endTime)
                    continue;

                tmpCDUnit.endTime = skillCDUnit.endTime;
            }
        }

        //是否在cd中
        public static bool IsInCD(Entity e, int skillId)
        {
            SkillComponent skillcom = e.GetComponentData<SkillComponent>();
            if (skillcom == null)
                return true;

            SkillCDUnit skillCDUnit;
            bool exist = skillcom.m_dicID2CD.TryGetValue(skillId, out skillCDUnit);
            if (!exist)
                return false;

            float endTime = skillCDUnit.endTime;
            float nowTime = LogicUtils.GetTime(e);
            if (nowTime < endTime)
                return true;

            return false;
        }

        //改变cd时间
        public static void ChangeCD(Entity e, int skillId, float cdTime)
        {
            if (cdTime < 0)
                cdTime = 0f;

            SkillComponent skillcom = e.GetComponentData<SkillComponent>();
            if (skillcom == null)
                return;

            SkillCDUnit skillCDUnit;
            bool exist = skillcom.m_dicID2CD.TryGetValue(skillId, out skillCDUnit);
            if (!exist)
                return;

            skillCDUnit.cd = cdTime;
        }

        //获取cd时间
        public static float GetCD(Entity e, int skillId)
        {
            float retCD = 0f;

            SkillComponent skillcom = e.GetComponentData<SkillComponent>();
            if (skillcom == null)
                return retCD;

            SkillCDUnit skillCDUnit;
            bool exist = skillcom.m_dicID2CD.TryGetValue(skillId, out skillCDUnit);
            if (!exist)
                return retCD;

            retCD = skillCDUnit.cd;

            return retCD;
        }
        #endregion

        public static bool CheckSkillCanCast(Entity e, int skillId)
        {
            //cd检测
            bool isInCd = IsInCD(e, skillId);
            if (isInCd)
                return false;

            return true;
        }

        public static List<KeyValuePair<int, float>> _SkillDistList = new List<KeyValuePair<int, float>>();
        public static void ClacSkillDistList(SkillComponent skillcom, int targetType) {
            _SkillDistList.Clear();
            int normalAtkId = GetNormalAtkId(skillcom);
            foreach (var it in skillcom.SkillInfos){
                SkillCfg skillCfg = it.Value.skillCfg;
                if (skillCfg.TargetType != targetType)
                    continue;
                if (skillCfg.SkillType == 3 && skillCfg.ID != normalAtkId)
                    continue;

                SkillLvCfg skillLvCfg = it.Value.skillLvCfg;
                _SkillDistList.Add(new KeyValuePair<int, float>(skillCfg.MasterSkill, skillLvCfg.CastRange));
            }

            _SkillDistList.Sort((a, b) =>a.Value.CompareTo(b.Value));
        }

        public static float GetShortestSkillCastDistance(Entity e, int targetType){
            SkillComponent skillcom = e.GetComponentData<SkillComponent>();
            if (skillcom == null) return 0f;
            return GetShortestSkillCastDistance(skillcom, targetType);
        }
        public static float GetShortestSkillCastDistance(SkillComponent skillcom, int targetType){
            ClacSkillDistList(skillcom, targetType);
            if (_SkillDistList.Count == 0) return 0;
            return _SkillDistList[0].Value;
        }

        public static float GetFarthestSkillCastDistance(Entity e, int targetType){
            SkillComponent skillcom = e.GetComponentData<SkillComponent>();
            if (skillcom == null) return 1f;
            float dist = GetFarthestSkillCastDistance(skillcom, targetType);
            if (dist < 0)
                return 1f;
            return dist;
        }
        public static float GetFarthestSkillCastDistance(SkillComponent skillcom, int targetType){
            ClacSkillDistList(skillcom, targetType);
            if (_SkillDistList.Count == 0) return 0;
            return _SkillDistList[_SkillDistList.Count-1].Value;
        }
    }

}
