using System;
using System.Collections.Generic;
using UnityEngine;

public enum EGotoType
{
    Random,     //随机跳转 (根据剧情的weight 随机选择,如果只有一条就根据 [权重 / 100] 的概率决定是[执行]还是[跳过] )
    Options,    //选项 (弹出选项框 只有一个 就必然执行)
    RandomRole, //队员随机选择 (从执行 GUID小的分支开始 执行到尽头 会回到此处 执行下一条)
}
public enum ECompType
{
    Battle,     //战斗
    Reward,     //奖励
    EventUnlock,//事件解锁
    EventEnd,   //事件结束
    EndResult,  //世界结算   
}

[Serializable]
public class IntIntPair
{
    public IntIntPair(int k, int v,int v2) { Key = k; Value = v; value2 = v2; }
    public int Key;
    public int Value;
    public int value2 = 100; //随机概率 = value2/100
}

public class IntPair
{
    public IntPair(int k, int v) { Key = k; Value = v; }
    public int Key;
    public int Value;
}

//剧情节点数据
[Serializable]
public class StoryNodeInfo {
    public int Id;
    public int eventId;
    public int weight = 100; 
    public string Title; 
    public string Text;
    public List<int> GoList = new();
    public int GoType;
    public List<IntIntPair> EnemyList = new();  //敌人列表
    public List<IntIntPair> RewardList = new(); //奖励列表
    public List<int> ULEIdList = new(); //解锁事件ID
    public bool IsEnd = false;
    public bool isEventEnd = false;
    public Vector2 pos;
}
     
//剧情数据
[Serializable]
public class EventInfo {
    public int Id;
    public int StartId;
    public string Name;
    public string Desc;
    public int r, g, b;
}

[Serializable]
public class StoryInfo {
    public List<EventInfo> EventList = new ();
    public List<StoryNodeInfo> NodeList = new ();
}
