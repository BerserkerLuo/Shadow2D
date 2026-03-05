
using System.Collections.Generic;
using UnityEngine;

namespace ECS.Script
{
    public class Skill1010 : ScriptBase
    {
        public override void OnSkillAttackTarget(Entity e, SkillUnit skillUnit, Entity tar) {
            Vector2 tarPos = LogicUtils.GetPos(tar);
            CreateUtil.CreateMonster(e.EcsWorld, tarPos, 1001, e);
        }
    }
}
