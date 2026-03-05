
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

namespace ECS.Script
{

    public class ScriptBase
    {
        public virtual void Init() {}


        //////////////////////////////////////////////////////////////////////
        ///SKILL 技能
        //////////////////////////////////////////////////////////////////////
        public virtual bool OnSkillCheckCanCast(Entity e, int skillId) {return true;}    
        public virtual void OnSkillStart(Entity e, SkillUnit skillUnit){}
        public virtual void OnSkillChannelBegin(Entity e, SkillUnit skillUnit) { }
        public virtual void OnSkillShotBegin(Entity e, SkillUnit skillUnit) { }
        public virtual void OnSkillFinishBegin(Entity e, SkillUnit skillUnit) { }
        public virtual void OnSkillFinishEnd(Entity e, SkillUnit skillUnit) { }
        public virtual void OnSkillAttackTarget(Entity e, SkillUnit skillUnit, Entity tar) { }
        public virtual bool OnShootBullet(Entity e, SkillUnit skillUnit, Vector2 tarPos) { return false; }

        //////////////////////////////////////////////////////////////////////
        ///BULLET 子弹 
        //////////////////////////////////////////////////////////////////////
        public virtual void OnBulletCollide(Entity e, Entity tar) { }
        public virtual void OnBulletArriveDestination(Entity e) { }


        //////////////////////////////////////////////////////////////////////
        ///Status 状态
        //////////////////////////////////////////////////////////////////////
        public virtual void OnStatusStepUpdate(Entity e, StatusInfo info) { }
        public virtual void OnStatusAddLayer(Entity e, int statusId, int addLayer) { }
        public virtual void OnStatusBegin(Entity e, int statusId) { }
        public virtual void OnStatusEnd(Entity e, StatusInfo info) { }


    }
}
