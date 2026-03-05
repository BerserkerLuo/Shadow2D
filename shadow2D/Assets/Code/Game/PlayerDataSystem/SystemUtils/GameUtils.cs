
using DataDefine;
using Table;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSystemData
{
    public class GameUtils
    {

        static Coroutine tempCoroutine;
        static public void StartGame()
        {
            if (null != tempCoroutine)
                return;

            GameStateMgr.Singleton.ChangeGameState(EnumGameState.eState_Battle);
        }


        static public void EndGame(GameResultInfo result)
        {
            if(result.isWin)
                MapRankSystem.Singleton.SetMapPass(result.worldId,result.mapRank);

            CurrencySystem.Singleton.ChangeGold(CalcBattleResultCoin(result));
        }


        static public int CalcBattleResultCoin(GameResultInfo result) {
            int ret = 0;
            ret += result.gameTime;
            ret += result.level * 100;
            ret += result.killCount * 5;
            if (result.isWin)
                ret += 1000;
            return ret;
        }

    }
}
