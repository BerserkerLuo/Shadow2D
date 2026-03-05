
using Table;
using System.Collections.Generic;
using UILib;
using UnityEngine;
using Game;

namespace ECS
{
    partial class WeaponUtil
    {
        public static void OnUseWeaponSkill(Entity e,Vector2 tPos) {
            WeaponSkillComponent comp = e.GetComponentData<WeaponSkillComponent>();
            if (comp == null)
                return;

            if (Time.time < comp.ReloadEndTime)
                return;

            OnChangeBulletCount(comp);

            SkillUtils.CastSKillToPosBySkillId(e,comp.WeaponSkillId,tPos);

            OnShotBullet(e);
        }

        public static void OnChangeBulletCount(WeaponSkillComponent comp) {
            comp.BulletCount -= 1;
            if (comp.BulletCount <= 0) {
                comp.BulletCount = (int)AttrUtil.GetBulletNum(comp.Entity);
                comp.ReloadEndTime = Time.time + 0.5f;
                UIUtils.ShowBulletReload(0.5f);
            }
                
            UIUtils.ShowBulletCount();
        }

        //==================================================================

        public static void OnShotBullet(Entity e) {
            OnTriggerSkill(e,TriggerType.Frie,e);
        }

        public static void OnBulletHit(Entity e, int skillId,Entity target) {
            Debug.Log($"OnBulletHit {skillId}");
            if (!CheckCanTriggerSkill(e, skillId)) return;
            OnTriggerSkill(e, TriggerType.Hit, target);
        }

        public static void OnBulletKill(Entity e, int skillId, Entity target){
            if (!CheckCanTriggerSkill(e, skillId)) return;
            OnTriggerSkill(e, TriggerType.Kill,target);
        }

        public static void OnBulletCrit(Entity e, int skillId, Entity target){
            if (!CheckCanTriggerSkill(e, skillId)) return;
            OnTriggerSkill(e, TriggerType.Crit,target);
        }

        public static void OnReload(Entity e) {
            OnTriggerSkill(e, TriggerType.Reload, e);
        }

        public static bool CheckCanTriggerSkill(Entity e,int skillId) {
            Entity master = LogicUtils.GetMaster(e);
            WeaponSkillComponent comp = master.GetComponentData<WeaponSkillComponent>();
            if (comp == null)
                return false;

            Debug.Log($"CheckCanTriggerSkill {skillId}");

            SkillCfg skillCfg = TableMgr.Singleton.GetSkillCfg(skillId);
            if (skillCfg == null)
                return false;

            Debug.Log($"CheckCanTriggerSkill weaponSkill{comp.WeaponSkillId} masterSkill {skillCfg.MasterSkill} skillId {skillId}");

            return comp.WeaponSkillId == skillCfg.MasterSkill;
        }
         
        public static void OnTriggerSkill(Entity e,TriggerType type, Entity tar) {

            Debug.Log($"OnTriggerSkill {type}");
            Entity master = LogicUtils.GetMaster(e);
            WeaponSkillComponent comp = master.GetComponentData<WeaponSkillComponent>();
            if (comp == null)
                return;

            WeaponSkillList skillList = comp.SkillCfg.GetValueOrDefault(type, null);
            if (skillList == null)
                return;

            foreach (int skillId in skillList.SkillList)
            {
                Debug.Log($"OnTriggerSkill {type} CastSKill {skillId}");
                SkillUtils.CastSKillToTargetBySkillId(e, skillId, tar.Eid);
            }
        }

        //==================================================================

        public static void OnSetTriggerSkill(Entity e, TriggerType type,int index,int skillId) {
            WeaponSkillComponent comp = e.GetComponentData<WeaponSkillComponent>();
            if (comp == null)
                return;

            OnSetTriggerSkill(comp,type,index,skillId);
        }

        public static void OnSetTriggerSkill(WeaponSkillComponent comp, TriggerType type, int index, int skillId){
            WeaponSkillList skillList = comp.SkillCfg.GetValueOrDefault(type, null);
            if (skillList == null){
                skillList = new WeaponSkillList();
                comp.SkillCfg.Add(type, skillList);
            }

            for (int i = skillList.SkillList.Count; i <= index; ++i)
                skillList.SkillList.Add(0);

            skillList.SkillList[index] = skillId;
        }

        public static int GetTriggerSkillId(WeaponSkillComponent comp, TriggerType type, int index) {
            WeaponSkillList skillList = comp.SkillCfg.GetValueOrDefault(type, null);
            if (skillList == null || skillList.SkillList.Count < index)
                return 0;
            return skillList.SkillList[index];
        }

        public static void OnChangeTriggerSkill(Entity e, TriggerType sType, int sIndex, TriggerType tType, int tIndex) {
            if (tType == sType)
                return;

            WeaponSkillComponent comp = e.GetComponentData<WeaponSkillComponent>();
            if (comp == null)
                return;

            int sSkillId = GetTriggerSkillId(comp,sType,sIndex);
            int tSkillId = GetTriggerSkillId(comp, tType, tIndex);

            OnSetTriggerSkill(comp, tType, tIndex, sSkillId);
            if(tSkillId != 0)
                OnSetTriggerSkill(comp, sType, sIndex, tSkillId);
        }

    }
}
