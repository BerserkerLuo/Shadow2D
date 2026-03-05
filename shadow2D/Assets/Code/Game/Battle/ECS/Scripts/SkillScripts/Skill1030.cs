
using System.Collections.Generic;
using UnityEngine;

namespace ECS.Script
{
    public class Skill1030 : ScriptBase
    {
        public override void OnSkillAttackTarget(Entity e, SkillUnit skillUnit, Entity tar) {
            Vector2 tarPos = LogicUtils.GetPos(tar);
            CreateUtil.CreateMonster(e.EcsWorld, tarPos,1002,e);
        }
    }
}
