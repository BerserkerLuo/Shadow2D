


using System.Collections.Generic;
using Table;
using UnityEngine;

namespace ECS
{
    internal class CheckCanUseSkillToTarget : ATNodeLeaf
    {
        public override bool Run(Entity e)
        {
            DebugUtils.OnAIThink("CheckCanUseSkillToTarget EID {} {}", e.Eid);

            Entity target = AIDataUtils.GetTarget(e);
            if (target == null || LogicUtils.IsDead(target))
            {
                DebugUtils.Log("CheckCanUseSkillToTarget NoTarget EID {} ", e.Eid);
                return false;
            }

            int targetType = FactionUtil.IsEnemy(target, e) ? (int)SkillTargetType.Enemy : (int)SkillTargetType.Frend;
            List<SkillInfo> skilList = SkillDataUtil.GetCanUseSkillList(e, targetType);
            if (skilList.Count == 0){
                DebugUtils.OnAIThink("CheckCanUseSkillToTarget skilList.Count Eid {}  SkillCount {} targetType {}", e.Eid, skilList.Count, targetType);
                return false;
            }

            Vector3 targetPos = LogicUtils.GetPos(target);
            float sqrDist = LogicUtils.GetSqrDistance(LogicUtils.GetPos(e), targetPos);

            foreach (var skilInfo in skilList) {

                float skillRange = skilInfo.skillLvCfg.CastRange;
                if (sqrDist > skillRange * skillRange)
                    continue;
                AIDataUtils.SetUseSkillId(e, skilInfo.skillId);
                return true;
            }

            DebugUtils.OnAIThink("CheckCanUseSkillToTarget No EID {} {}", e.Eid);

            return false;
        }
    }
}
