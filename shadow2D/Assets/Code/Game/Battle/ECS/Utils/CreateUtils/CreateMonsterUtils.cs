
using Table;
using UnityEngine;

namespace ECS
{
    partial class CreateUtil
    {
        public static Entity CreateMonster(ECSWorld world, Vector3 bronPos, int xmlId,Entity master = null)
        {
            MonsterCfg monsterCfg = TableMgr.Singleton.GetMonsterCfg(xmlId);
            if (monsterCfg == null)
                return null;

            Entity e = CreateEntity(world);

            InitEntityTypeComponent(e, EnumEntityType.eMonster,monsterCfg.ID);
            InitAvatarComponent(e, monsterCfg.Prefab);
            InitRVOComonent(e, bronPos,0.3f,2,monsterCfg.RVOLayer);

            int factionId = master != null ? FactionUtil.GetFaction(master) : FactionId.Monster;
            InitFactionComponent(e, factionId);

            if( monsterCfg.LifeTime > 0)
                InitLifeTimeComponent(e, monsterCfg.LifeTime);

            if (master != null)
                InitMasterComponent(e,LogicUtils.GetMaster(master));

            e.AddComponent<AttrComponent>();
            e.AddComponent<StatusComponent>();
            e.AddComponent<AutoMoveComponent>();
            e.AddComponent<SkillComponent>();
            e.AddComponent<AIComponent>();

            //AvatarDataUtil.AddHealthBarToEntity(e,2);

            foreach (var skillId in monsterCfg.SkillList)
                SkillUtils.AddSkill(e, skillId);

            AIDataUtils.SetAITreeType(e,AITreeType.AITreeMonster);

            InitMonsterAttr(e, monsterCfg);

            LogicUtils.SetPos(e, bronPos);

            UIUtils.SetName(e, monsterCfg.Name);

            AnimationUtil.OnBron(e);

            return e;
        }

        public static void InitMonsterAttr(Entity e, MonsterCfg monsterCfg) {

            AttrDataUtil.AddAttr(e, AttrUtil.BaseAttrRowID, AttrType.HPMax, monsterCfg.HP, 0);
            AttrDataUtil.AddAttr(e, AttrUtil.BaseAttrRowID, AttrType.MoveSpeed, monsterCfg.Speed, 0);
            AttrDataUtil.RefreshAttr(e);
        }
    }
}
