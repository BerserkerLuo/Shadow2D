
using Table;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ECS
{
    internal class DamageUtils
    {
        //=======================================================================================================================

        //索敌
        public static List<Entity> GetSkillTargetList(Entity e, SkillUnit skillUnit)
        {
            List<Entity> retList = null;

            int searchWay = skillUnit.skillLvCfg.SearchWay;

            if (searchWay == SearchAreaType.None)
                return retList;

            //单体
            if (searchWay == SearchAreaType.Single)
            {
                retList = new List<Entity>();
                Entity target = LogicUtils.GetEntity(e.EcsWorld, skillUnit.targetEid);
                if (target != null) retList.Add(target);
            }
            //圆形
            else if (searchWay == SearchAreaType.Circle)
            {
                retList = SearchTargetUtils.SearchEnemyByRange(e, skillUnit.skillLvCfg.HitRange, skillUnit.skillLvCfg.MaxTar);
            }

            return retList;
        }

        //计算技能伤害
        public static float GetSkillDamage(Entity sE, Entity tE, SkillUnit skillUnit)
        {
            float damage = skillUnit.skillLvCfg.Damage;

            if (LogicUtils.GetRand(0, 100) < AttrUtil.GetCrit(sE)){
                damage = damage * (1.5f + AttrUtil.GetCritEffect(sE));
                WeaponUtil.OnBulletCrit(sE, skillUnit.skillId, tE);
            }

            if (damage < 0) damage = 0;
            
            return damage;
        }

        //=======================================================================================================================

        //尝试应用 技能效果 到 技能范围内的目标 (伤害,Status ...)
        public static void TryApplySkillEffectToTargets(Entity e, SkillUnit skillUnit)
        {
            Debug.Log($"TryApplySkillEffectToTargets {skillUnit.skillCfg.ID}");

            List<Entity> targetList = GetSkillTargetList(e, skillUnit);
            if (targetList == null || targetList.Count == 0)
                return;

            Debug.Log($"TryApplySkillEffectToTargets2 {skillUnit.skillCfg.ID}");

            foreach (var target in targetList)
            {
                ScriptUtils.OnSkillAttackTarget(e, skillUnit, target);

                applyApplySkillEffectToOne(e, skillUnit, target);

                WeaponUtil.OnBulletHit(e,skillUnit.skillId,target);

                if (skillUnit.skillCfg.HitEffect != "")
                    EffectUtils.ShowEffect(skillUnit.skillCfg.HitEffect,LogicUtils.GetPos(target),2);
            }
        }

        //应用 技能伤害 到 单体
        private static void applyApplySkillEffectToOne(Entity srcE, SkillUnit skillUnit, Entity targetE)
        {
            float hurt = GetSkillDamage(srcE, targetE, skillUnit);

            float realChange = ChangeHP(targetE, -hurt, srcE);

            Debug.Log($"SkillDamage skillId{skillUnit.skillId} hurt {hurt}");

            if (LogicUtils.IsDead(targetE)){
                WeaponUtil.OnBulletKill(srcE,skillUnit.skillCfg.MasterSkill,targetE);
                return;
            }

            //还没死就尝试添加技能附带Buff
            foreach (int statusId in skillUnit.skillLvCfg.Status)
                StatusUtil.AddStatus(targetE, statusId);
        }

        //扣血
        public static float ChangeHP(Entity e, float ChangeValue, Entity atker = null, bool isShowText = true)
        {
            if (ChangeValue < 0f && LogicUtils.IsGodState(e))
                return 0f;

            //DebugUtils.Log("ChangeHP :", ChangeValue, " EntityHP:", AttrUtil.GetHP(e));

            //实际改变attr值
            float realChangeValue = AttrUtil.ChangeHP(e, ChangeValue);

            //扣血表现
            float realChangeValueAbs = Math.Abs(realChangeValue);
            if (realChangeValueAbs > 1 && isShowText)
            {
                Vector3 dire = Vector3.zero;
                if (atker != null)
                    dire = LogicUtils.GetPos(e) - LogicUtils.GetPos(atker);

                UIUtils.ShowDamageText(LogicUtils.GetPos(e),realChangeValue, dire);

                AnimationUtil.OnHit(e);
            }

            if (AttrUtil.GetHP(e) > 0)
                return realChangeValue;

            // 目标死亡的处理
            LogicUtils.KillEntity(e);
            StatisticsUtil.OnSkillMonster(atker,e);

            return realChangeValue;
        }

    }
}
