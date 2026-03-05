
using Battle;
using DataDefine;
using Game;
using PlayerSystemData;
using Client.UI;

namespace ECS
{
    internal class GameResultUtil
    {
      
        //=================================================================
        public static void OnGameEnd(ECSWorld ecsWorld, int code)
        {
            GameResultInfo resultInfo = new GameResultInfo();
            resultInfo.isWin = code == 1;
            resultInfo.worldId = MapUtil.GetMapId(ecsWorld);
            resultInfo.mapRank = MapUtil.GetMapRank(ecsWorld);
            resultInfo.gameTime = (int)ecsWorld.Time;

            GameUtils.EndGame(resultInfo);
            DlgGameResult.singleton.OnShowResult(resultInfo);

            LogicUtils.SetGameEnd(ecsWorld);
        }
    }

}
