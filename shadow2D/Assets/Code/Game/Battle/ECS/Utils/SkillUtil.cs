


using Table;
using System.Collections.Generic;
using UnityEngine;
using Tools;

namespace ECS
{
    class SkillUtils
    {
        //=======================================================================

        //获取技能等级[主技能等级
        public static int GetSkillLevel(Entity e, int skillId)
        {
            int level = SkillDataUtil.GetRealSkillLevel(e, skillId);
            if (level != 0)
                return level;

            Entity master = LogicUtils.GetMaster(e);
            if (master == e)
                return level;

            return SkillDataUtil.GetRealSkillLevel(master, skillId);
        }

        //添加技能入口
        public static void AddSkill(Entity e, int skillId, int addLevel = 1) {
            int oldLevel = GetSkillLevel(e, skillId);
            SkillInfo info = SkillDataUtil.AddSkill(e, skillId, addLevel);
            if (info == null || oldLevel == info.skillLevel)
                return;

            int step = 1;
            int minLevlel = oldLevel;
            int MaxLevlel = info.skillLevel;
            if (minLevlel > MaxLevlel){
                Util.Swap(ref minLevlel, ref MaxLevlel);
                step = -1;
            }

            for (int level = minLevlel; level <= MaxLevlel; level++) {
                SkillLvCfg skillLvCfg = TableMgr.Singleton.GetSkillLvCfg(skillId,level);
                if (skillLvCfg == null)
                    continue;
                foreach (int statusId in skillLvCfg.PassiveStatus)
                    StatusUtil.AddStatus(e, statusId, step);
            }
        }

        //=======================================================================

        //打包技能过程数据
        private static SkillUnit MakeSkillUnit(Entity e, SkillProcPrevInfo prevInfo, SkillCfg skillCfg,SkillLvCfg skillLvCfg)
        {
            SkillUnit skillUnit = new SkillUnit();
            skillUnit.skillUnitID = skillCfg.ID;
            skillUnit.state = SKILLSTATE.Idle;
            skillUnit.ownerEid = e.Eid;
            skillUnit.slotId = prevInfo.slotId;
            skillUnit.skillId = skillCfg.ID;
            skillUnit.targetEid = prevInfo.targetEid;
            skillUnit.dire = prevInfo.dire;
            skillUnit.x = prevInfo.x;
            skillUnit.y = prevInfo.y;
            skillUnit.startTime = LogicUtils.GetTime(e);   //这里的时间考究了// 因为需要有定点数  那么 问题来了  我们放在哪里 是个问题
            skillUnit.stateEndTime = skillUnit.startTime;
            skillUnit.skillCfg = skillCfg;
            skillUnit.skillLvCfg = skillLvCfg;

            return skillUnit;
        }

        #region 释放技能，外部调用

        ////对方向释放技能[SlotId]
        //public static bool CastSKillToDireBySlotID(Entity e, int slotId, Vector2 dire)
        //{
        //    int skillId = SkillDataUtil.GetSkillIDBySlotId(e, slotId);
        //    if (skillId == -1)
        //        return false;

        //    CastSKillToDireBySkillId(e, skillId, dire, slotId);
        //    return true;
        //}
        //对方向释放技能[skillId]
        public static void CastSKillToDireBySkillId(Entity e, int skillId, Vector2 dire, int skilLevel = 0, int slotId = -1)
        {
            SkillProcPrevInfo prevInfo = new SkillProcPrevInfo();
            prevInfo.slotId = slotId;
            prevInfo.skillId = skillId;
            prevInfo.skilLevel = skilLevel;
            prevInfo.dire = dire;
            CastSkill(e, prevInfo);
        }

        ////对地点释放技能[SlotId]
        //public static bool CastSKillToPosBySlotId(Entity e, int slotId, float x, float y, float z)
        //{
        //    int skillId = SkillDataUtil.GetSkillIDBySlotId(e, slotId);
        //    if (skillId == -1)
        //        return false;

        //    CastSKillToPosBySkillId(e, skillId, x, y, z, slotId);
        //    return true;
        //}
        //对地点释放技能[SkillId]
        public static void CastSKillToPosBySkillId(Entity e, int skillId, Vector2 tPos, int slotId = -1)
        {
            SkillProcPrevInfo prevInfo = new SkillProcPrevInfo();
            prevInfo.slotId = slotId;
            prevInfo.skillId = skillId;
            prevInfo.skilLevel = 1;
            prevInfo.x = tPos.x;
            prevInfo.y = tPos.y;
            prevInfo.z = 0;
            Vector3 pos = LogicUtils.GetPos(e);
            prevInfo.dire = new Vector2(tPos.x - pos.x, tPos.y - pos.y);
            CastSkill(e, prevInfo);
        }

        ////对目标释放技能[SlotId]
        //public static bool CastSKillToTargetBySlotId(Entity e, int slotId, int targetEid)
        //{
        //    int skillId = SkillDataUtil.GetSkillIDBySlotId(e, slotId);
        //    if (skillId == -1)
        //        return false;

        //    CastSKillToTargetBySkillId(e, skillId, targetEid, slotId);
        //    return true;
        //}
        //对目标释放技能[SkillId]
        public static void CastSKillToTargetBySkillId(Entity e, int skillId, int targetEid, int skilLevel = 0, int slotId = -1)
        {
            SkillProcPrevInfo prevInfo = new SkillProcPrevInfo();
            prevInfo.slotId = slotId;
            prevInfo.skillId = skillId;
            prevInfo.skilLevel = skilLevel;
            prevInfo.targetEid = targetEid;

            Entity target = LogicUtils.GetEntity(e.EcsWorld, targetEid);
            if (target != null)
            {
                Vector3 tPos = LogicUtils.GetPos(target);
                Vector3 pos = LogicUtils.GetPos(e);
                prevInfo.dire = new Vector2(tPos.x - pos.x, tPos.y - pos.y);

                prevInfo.x = tPos.x;
                prevInfo.y = tPos.y;
                prevInfo.z = tPos.z;
            }

            CastSkill(e, prevInfo);
        }

        #endregion


        //释放技能
        private static void CastSkill(Entity e, SkillProcPrevInfo prevInfo)
        {
            var skillcom = e.GetComponentData<SkillComponent>();
            if (skillcom == null)
                return;

            int skillId = prevInfo.skillId;
            SkillCfg skillCfg = TableMgr.Singleton.GetSkillCfg(skillId);
            if (skillCfg == null)
                return;

            if (skillCfg.FreeCast)
            {
                if (skillcom.m_freeCastSkillDic.ContainsKey(skillId))
                    return;
            }

            if (!skillCfg.FreeCast && skillcom.m_curCast != null)
                return;

            int level = prevInfo.skilLevel;
            if (level == 0)
                level = SkillDataUtil.GetSkillLevel(LogicUtils.GetMaster(e), skillCfg.MasterSkill);

            SkillLvCfg skillLvCfg = TableMgr.Singleton.GetSkillLvCfg(skillId,level);
            if (skillLvCfg == null)
                return;

            bool canCast = ScriptUtils.CheckSkillCanCast(e, skillId);
            if (!canCast)
                return;

            //cd检测
            bool isInCd = SkillDataUtil.IsInCD(e, skillId);
            if (isInCd)
                return;

            //技能消耗 
            //bool costSucc = SkillCostUtils.TryDoSkillCost(e, skillId);
            //if (!costSucc)
            //    return;

            var skillunit = MakeSkillUnit(e, prevInfo, skillCfg, skillLvCfg);
            if (skillCfg.FreeCast)
                skillcom.m_freeCastSkillDic.Add(skillId, skillunit);
            else
                skillcom.m_curCast = skillunit;

            //开始cd计算
            SkillDataUtil.StartCD(e, skillId);

            //技能开始
            OnStart(skillunit, e);
        }


        public static void StepUpdateSkillUnit(SkillComponent skillcom)
        {
            Entity entity = skillcom.Entity;
            float curtime = LogicUtils.GetTime(entity);

            _StepUpdateOneSkillUnit(entity, skillcom.m_curCast, curtime);

            var freeCastSkillDic = skillcom.m_freeCastSkillDic;
            if (freeCastSkillDic.Count <= 0)
                return;

            foreach (var pair in freeCastSkillDic) {
                _StepUpdateOneSkillUnit(entity, pair.Value, curtime);
            }

            if (skillcom.m_endList.Count <= 0)
                return;

            foreach (int skillId in skillcom.m_endList)
                skillcom.m_freeCastSkillDic.Remove(skillId);
            skillcom.m_endList.Clear();
        }

        //逻辑帧 刷新
        public static void _StepUpdateOneSkillUnit(Entity entity, SkillUnit skillUnit, float curtime)
        {
            //Debug.LogError($"StepUpdateSkillUnit:{skillcom.GetEntity().Eid}");

            if (skillUnit == null)
                return;

            //空闲状态就什么都不做
            if (skillUnit.state == SKILLSTATE.Idle)
                return;

            //当前阶段没有结束
            if (curtime < skillUnit.stateEndTime)
                return;

            //这里本质上是个状态机
            //当前阶段结束 开始下一个阶段
            switch (skillUnit.state)
            {
                case SKILLSTATE.BeforShot: OnShotBegin(skillUnit, entity); break;
                case SKILLSTATE.Shot: OnFinishEnd(skillUnit, entity); break;
            }
        }

        //前摇
        public static void OnStart(SkillUnit skillUnit, Entity e)
        {
            //DebugUtils.Log("OnSkillStart "+skillUnit.skillId);

            skillUnit.state = SKILLSTATE.BeforShot;
            ScriptUtils.OnSkillStart(skillUnit, e);
            AnimationUtil.Attack(e, skillUnit.dire);

            LogicDataUtils.SetPauseMove(e, true);

            float curStepTime = skillUnit.skillCfg.StartTime;
            if (curStepTime == 0)
                OnShotBegin(skillUnit, e);
            else
                skillUnit.stateEndTime = LogicUtils.GetTime(e) + curStepTime;
        }

        //出手关键帧事件
        public static void OnAtkFrame(Entity e)
        {
            SkillComponent skillCom = e.GetComponentData<SkillComponent>();
            if (skillCom == null || skillCom.m_curCast == null)
                return;

            if (skillCom.m_curCast.state != SKILLSTATE.BeforShot)
                return;

            OnShotBegin(skillCom.m_curCast,e);
        }

        //出手
        public static void OnShotBegin(SkillUnit skillUnit, Entity e)
        {
            skillUnit.state = SKILLSTATE.Shot;
            ScriptUtils.OnSkillShotBegin(skillUnit, e);

            DamageUtils.TryApplySkillEffectToTargets(e, skillUnit);

            ShootBulletUtils.TryShootSkillBullet(e, skillUnit, new Vector2(skillUnit.x, skillUnit.y));

            float curStepTime = skillUnit.skillCfg.FinishTime;
            if (curStepTime == 0)
                OnFinishEnd(skillUnit, e);
            else
                skillUnit.stateEndTime = LogicUtils.GetTime(e) + curStepTime;
        }

        //后摇结束
        public static void OnFinishEnd(SkillUnit skillUnit, Entity e)
        {
            LogicDataUtils.SetPauseMove(e, false);

            skillUnit.state = SKILLSTATE.End;
            ScriptUtils.OnSkillFinishEnd(skillUnit, e);

            //List<int> ContinueSkills = skillUnit.skillCfg.ContinueSkill;
            //foreach (int skillId in ContinueSkills)
            //    CastSKillToPosBySkillId(e, skillId, skillUnit.x, skillUnit.y, skillUnit.z);

            var skillcom = e.GetComponentData<SkillComponent>();
            if (skillcom == null)
                return;

            if (skillUnit.skillCfg.FreeCast)
            {
                skillcom.m_endList.Add(skillUnit.skillId);
            }
            else
            {
                skillcom.m_curCast = null;
            }
        }
    }
}
