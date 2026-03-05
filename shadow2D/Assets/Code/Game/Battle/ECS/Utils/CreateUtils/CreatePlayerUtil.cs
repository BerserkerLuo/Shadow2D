
using PlayerSystemData;
using Table;
using UnityEngine;

namespace ECS
{
    partial class CreateUtil
    {
        public static Entity CreateHero(ECSWorld world,Vector3 bronPos,int heroId,int  weaponId) {

            Debug.Log("DEVLOG CreateHero 1");

            HeroCfg HeroCfg = TableMgr.Singleton.GetHeroCfg(heroId);
            if (HeroCfg == null)
                return null;

            Debug.Log($"DEVLOG CreateHero 2 [{weaponId}]");

            WeaponCfg weaponCfg = TableMgr.Singleton.GetWeaponCfg(weaponId);
            if (weaponCfg == null)
                return null;

            Debug.Log("DEVLOG CreateHero 3");


            Entity e = CreateEntity(world);
            e.AddComponent<SkillComponent>();
            e.AddComponent<StatusComponent>();
            e.AddComponent<AutoMoveComponent>();
            e.AddComponent<ExpComponent>();
            e.AddComponent<BackpackComponent>();

            WeaponSkillComponent weaponSkillComponent = e.AddComponent<WeaponSkillComponent>();
            weaponSkillComponent.WeaponId = weaponId;
            weaponSkillComponent.WeaponSkillId = weaponCfg.Skill;

            InitEntityTypeComponent(e,EnumEntityType.eHero, HeroCfg.ID);
            InitAvatarComponent(e, HeroCfg.Avatar);
            InitRVOComonent(e,bronPos,0.3f,1);
            InitFactionComponent(e, FactionId.Player);

            //AvatarDataUtil.AddHealthBarToEntity(e,1);
            
            e.AddComponent<AttrComponent>();
            InitHeroAttr(e, HeroCfg);
            InitWeaponAttr(e, weaponCfg);

            //e.AddComponent<AIComponent>();
            //AIDataUtils.SetAITreeType(e, AITreeType.AITreeHero);

            foreach (int skillId in HeroCfg.Skill)
                SkillUtils.AddSkill(e, skillId);
            SkillUtils.AddSkill(e, weaponCfg.Skill);

            AvatarDataUtil.AddWeaponToEntity(e,weaponCfg.Avatar);

            LogicUtils.SetPos(e,bronPos);

            AnimationUtil.OnBron(e);

            Debug.Log($"DEVLOG CreateHero [{AttrUtil.GetHP(e)}]");

            //UIUtils.SetName(e,heroInfo.name);

            return e;
        }

        public static void InitHeroAttr(Entity e, HeroCfg heroCfg){
            AttrDataUtil.AddAttr(e, AttrUtil.BaseAttrRowID, AttrType.HPMax, heroCfg.HP, 0);
            AttrDataUtil.AddAttr(e, AttrUtil.BaseAttrRowID, AttrType.MoveSpeed, heroCfg.Speed, 0);
            AttrDataUtil.AddAttr(e, AttrUtil.BaseAttrRowID, AttrType.PickUpRange, 5, 0);
            AttrDataUtil.RefreshAttr(e);
        }

        public static void InitWeaponAttr(Entity e,WeaponCfg weaponCfg) {

            int RowID = AttrUtil.WeaponAttrRowID;
            AttrDataUtil.AddAttr(e, RowID, AttrType.Atk, weaponCfg.Damage, 0);
            AttrDataUtil.AddAttr(e, RowID, AttrType.Crit, weaponCfg.Crit, 0);
            //AttrDataUtil.AddAttr(e, RowID, AttrType.BulletPenet, weaponCfg.Penetrate, 0);
            //AttrDataUtil.AddAttr(e, RowID, AttrType.BulletCatapult, weaponCfg.Catapult, 0);
            AttrDataUtil.AddAttr(e, RowID, AttrType.ShotNum, weaponCfg.ShotNum, 0);
            AttrDataUtil.AddAttr(e, RowID, AttrType.BulletNum, weaponCfg.BulletNum, 0);
            AttrDataUtil.RefreshAttr(e);
        }
    }
}
