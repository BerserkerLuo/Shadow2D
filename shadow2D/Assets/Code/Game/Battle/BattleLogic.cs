

using ECS;
using Tool;
using UnityEngine;
using DataDefine;
using PlayerSystemData;
using Pathfinding.RVO;

namespace Battle
{
    public class BattleLogic
    {
        public ECSWorld ECSWorld { get { return m_ecsWorld; } }

        static BattleLogic()
        {
            //注册component
        }

        public BattleLogic()
        {
            m_ecsWorld = new ECSWorld();

            RegAllComponent();
            AddAllSystem();
            CreateGlobalEntity();
        }
         
        public void RegAllComponent(){
            m_ecsWorld.RegComponent(typeof(BasicComponent));
            m_ecsWorld.RegComponent(typeof(AvatarComponent));
            m_ecsWorld.RegComponent(typeof(AttrComponent));
            m_ecsWorld.RegComponent(typeof(SkillComponent));
            m_ecsWorld.RegComponent(typeof(StatusComponent));
            m_ecsWorld.RegComponent(typeof(StateComponent));
            m_ecsWorld.RegComponent(typeof(AIComponent));
            m_ecsWorld.RegComponent(typeof(BulletComponent));
            m_ecsWorld.RegComponent(typeof(AutoMoveComponent));
            m_ecsWorld.RegComponent(typeof(LifeTimeComponent));
            m_ecsWorld.RegComponent(typeof(DropComponent));
            
        }

        public void AddAllSystem() {
            m_ecsWorld.AddSystem(new RVOSystem(m_ecsWorld));
            m_ecsWorld.AddSystem(new CameraFollowSystem(m_ecsWorld));
            m_ecsWorld.AddSystem(new InputSystem(m_ecsWorld));
            m_ecsWorld.AddSystem(new SkillSystem(m_ecsWorld));
            m_ecsWorld.AddSystem(new StatusSystem(m_ecsWorld));
            m_ecsWorld.AddSystem(new AutoMoveSystem(m_ecsWorld));
            m_ecsWorld.AddSystem(new EntityClassificationSystem(m_ecsWorld));
            m_ecsWorld.AddSystem(new AISystem(m_ecsWorld));
            m_ecsWorld.AddSystem(new BulletSystem(m_ecsWorld));
            m_ecsWorld.AddSystem(new SuspendSystem(m_ecsWorld));
            m_ecsWorld.AddSystem(new DeadSystem(m_ecsWorld));
            m_ecsWorld.AddSystem(new GameResultSystem(m_ecsWorld));
            m_ecsWorld.AddSystem(new WaveSystem(m_ecsWorld));
            m_ecsWorld.AddSystem(new PickupSystem(m_ecsWorld));
        }

        public void CreateGlobalEntity() {
            m_ecsWorld.GlobalEntity = CreateUtil.CreateEntity(m_ecsWorld);
            m_ecsWorld.GlobalEntity.AddComponent<PlayerComponent>();
            m_ecsWorld.GlobalEntity.AddComponent<EntityClassificationComponent>();
            m_ecsWorld.GlobalEntity.AddComponent<SelectObjectComponent>();
            m_ecsWorld.GlobalEntity.AddComponent<SuspendComponent>();
            m_ecsWorld.GlobalEntity.AddComponent<DelayDeadComponent>();
            m_ecsWorld.GlobalEntity.AddComponent<MapComponent>();
            m_ecsWorld.GlobalEntity.AddComponent<WaveComponent>();
            m_ecsWorld.GlobalEntity.AddComponent<GlobalComponent>();
        }

        public void StartGame(GameStartInfo SInfo){
            ECSObjectPoolMgr.Clear();
            RVOMgr.Singleton.Init();
            MapUtil.InitMapComponent(m_ecsWorld,SInfo.MapId,SInfo.RankId);
            WaveUtil.InitWave(m_ecsWorld);

            Entity e = CreateUtil.CreateHero(m_ecsWorld, Vector3.zero, SInfo.HeroId, SInfo.WeaponId);
            LogicUtils.SetPlayerEntity(m_ecsWorld,e);

            Debug.Log("DEVLOG StartGame");

            //AvatarDataUtil.AddWeaponToEntity(e, itemCfg.Prefab);
            //EquipUtil.LoadEquip(e,EquipPort.Weapon,gameStartInfo.WeaponId);

            //WeaponUtil.OnSetTriggerSkill(e,Game.TriggerType.Hit,0, 1030);

            //Vector3 pos = LogicUtils.GetSurroundPlayerPos(m_ecsWorld, 10, 5);
            //CreateUtil.CreateMonster(m_ecsWorld, pos, 10);
        }


        public void OnGameEnd(){
            ECSObjectPoolMgr.Clear();
            RVOMgr.Singleton.DllAllAgent();
        }

        public void Update()
        {
            RVOUtil.UpDateRVO();
            m_ecsWorld.UpdateGameTime(Time.time);
            m_ecsWorld.Update();
        }

        public void LateUpdate()
        {
            m_ecsWorld.UpdateGameTime(Time.time);
            m_ecsWorld.LateUpdate();
        }

        public void FixedUpdate()
        {
            
            m_ecsWorld.UpdateGameTime(Time.time);
            m_ecsWorld.FixedUpdate();
        }

        private ECSWorld m_ecsWorld = null;

    }
}
