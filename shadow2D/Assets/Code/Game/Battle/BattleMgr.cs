
using Tool;
using ECS;
using DataDefine;
using PlayerSystemData;
using Game;

namespace Battle
{
    class BattleMgr : SingletonBase<BattleMgr>
    {
        public BattleMgr(){}

        public BattleLogic GetBattleLogic(){
            return m_battleLogic;
        }

        public ECSWorld GetEcsWorld() {
            if (m_battleLogic == null)
                return null;
            return m_battleLogic.ECSWorld;
        }

        public void StartGame()
        {
            m_battleLogic = new BattleLogic();

            GameStartInfo SInfo = new GameStartInfo();

            SInfo.HeroId = OperationSystem.Singleton.GetHeroId();
            SInfo.WeaponId = OperationSystem.Singleton.GetWeaponId();
            SInfo.MapId = OperationSystem.Singleton.GetMapId();
            SInfo.RankId = OperationSystem.Singleton.GetMapRank(SInfo.MapId);

            DebugUtils.Log("heroId {} WeaponId {} mapId{} RankId {}", SInfo.HeroId, SInfo.WeaponId, SInfo.MapId, SInfo.RankId);

            m_battleLogic.StartGame(SInfo);
        }

        public void EndGame()
        {
            m_battleLogic.OnGameEnd();
            m_battleLogic = null;

            GameStateMgr.Singleton.ChangeGameState(EnumGameState.eState_Lobby);
        }

        public void FixedUpdate()
        {
            if (m_battleLogic != null) m_battleLogic.FixedUpdate();
        }

        public void Update()
        {
            if (m_battleLogic != null) m_battleLogic.Update();
        }

        public void LateUpdate()
        {
            if (m_battleLogic != null) m_battleLogic.LateUpdate();
        }

        private BattleLogic m_battleLogic = null;
    }
}

