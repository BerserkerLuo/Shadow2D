
using System.Collections.Generic;
using UnityEngine;

namespace ECS.Script
{
    public class Skill1020 : ScriptBase
    {
        public override void OnSkillAttackTarget(Entity e, SkillUnit skillUnit, Entity tar) {
            Vector2 tarPos = LogicUtils.GetPos(tar);
            Entity bullet = CreateUtil.CreateBullet(e.EcsWorld, skillUnit.skillLvCfg.BulletID, tarPos, e);
            LogicDataUtils.SetSurroundMove(bullet, tar, 0,0,Vector2.zero);
        }
    }
}
