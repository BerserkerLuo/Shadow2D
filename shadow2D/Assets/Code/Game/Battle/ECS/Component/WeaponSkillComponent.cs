using Game;
using System.Collections;
using System.Collections.Generic;
using UILib;
using UnityEngine;

namespace ECS
{ 

    public class WeaponSkillComponent : Component {
        public int WeaponId = 0;
        public int WeaponSkillId = 0;
        public Dictionary<TriggerType, WeaponSkillList> SkillCfg = new Dictionary<TriggerType, WeaponSkillList>();
        public Dictionary<int, int> skillBuffMap = new Dictionary<int, int>();
        public int BulletCount = 0;
        public float ReloadEndTime = 0;
    } 
}