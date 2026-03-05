
using System.Collections.Generic;
using Table;
using UnityEngine;

namespace ECS
{
    public enum StoryState
    {
        Idle = 0,       //空闲
        Dialogue = 1,   //剧情对话
        Battle = 2,     //战斗
        Result = 3,     //结算
        Reward = 4,     //奖励
        UnlockEvent = 5,//解锁事件
        Close = 6,      //关闭
         
        //剧情流程: 对话 => 战斗 => 结算 => 奖励 =>解锁事件 => 跳转 => 关闭
    }
}
