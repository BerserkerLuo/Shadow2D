

using Client.UI;
using ECS;
using System.Collections;

namespace Game
{
    public class GameStateLobby : GameStateBase
    {
        public override void OnEnter()
        {
            base.OnEnter();

            DebugUtils.DebugLog("GameStateLobby Enter");

            DlgLobby.singleton.SetVisible(true);

            CursorManager.Instance.ChangToNormalCurser();
        }

        public static IEnumerator ShowMainUI()
        {
            yield return 3;
        }

        public override void OnLeave()
        {
            DlgLobby.singleton.SetVisible(false);
            base.OnLeave();

            //PlayerDataSystemMgr.Singleton.SaveData();
        }

        public override void Update()
        {
            //PlayerDataSystemMgr.Singleton.Update();
        }

    }
}
