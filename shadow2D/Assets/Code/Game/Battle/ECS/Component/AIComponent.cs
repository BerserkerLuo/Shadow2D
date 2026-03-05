
using UnityEngine;

namespace ECS
{
    enum AIState { 
        AI_Idle,       //空闲
        AI_MoveControl,//移动控制
        AI_Alert,   //警觉
        AI_Battle,  //战斗(对敌方释放技能)
        AI_Support, //支援(对友军释放技能)
        AI_Loss,    //丢失目标

    }

    internal class AIComponent : Component
    {
        //AItree 的类型
        public int treeType = AITreeType.AITreeNone;


        //思考间隔
        public float ThinkInterval = 0.2f;
        //上次思考时间
        public float LastThinkTime = 0;

        //AI状态
        public AIState AIState = AIState.AI_Idle;
        //状态开始时间
        public float AIStateStartTime = -999;

        //目标  (攻击目标 or 支援目标)
        public Entity Target;

        //跟随目标点
        public Vector3 FollowPos;
        //跟随保持距离
        public float SqrKeepDistance;

        //即将释放的技能ID
        public int UseSkillId;

        //驻守点
        public Vector3 GuardPos;
        //警戒范围
        public float GuardRange = 100;
    }
}

