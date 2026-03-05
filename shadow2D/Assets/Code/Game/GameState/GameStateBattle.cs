
using System.Collections;
using Battle;
using Client.UI;
using PlayerSystemData;
using UnityEngine;
using Table;
namespace Game
{
    public class GameStateBattle : GameStateBase
    {
        protected override string SceneName { get {
                int mapId = OperationSystem.Singleton.GetMapId();
                MapCfg mapCfg = TableMgr.Singleton.GetMapCfg(mapId);
                if(mapCfg == null)
                    return "Map2";
                return mapCfg.MapScene;
            } 
        }

        public override IEnumerator LoadAsync()
        {
            yield return UnityGameEntry.Instance.StartCoroutine(base.LoadAsync());

            yield return null;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            BattleMgr.Singleton.StartGame();

            DlgFlyText.singleton.SetVisible(true);
            DlgMain.singleton.SetVisible(true);
            DlgGameControl.singleton.SetVisible(true);

            //DlgFlyText.singleton.ShowFlyText(new Vector3(10, 0, 1), "-654", 1);
        }

        public override void OnLeave()
        {
            base.OnLeave();

            DlgFlyText.singleton.SetVisible(false);
            DlgMain.singleton.SetVisible(false);
            DlgGameControl.singleton.SetVisible(false);
        } 

        public override void OnLaterLeave()
        {
            base.OnLaterLeave();
        }

        public override void Update()
        {
            BattleMgr.Singleton.Update();
        }
        public override void LateUpdate()
        {
            BattleMgr.Singleton.LateUpdate();
        }
    }
}
