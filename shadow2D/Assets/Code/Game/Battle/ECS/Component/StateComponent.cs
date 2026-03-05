
using System.Collections.Generic;

namespace ECS
{
    public enum eState { 
        eState_God = 1,  //无敌
        eState_Root = 2, //定身
        eState_Dash = 3, //冲锋
        eState_Jump = 4, //跳跃
        eState_Absorb = 5, //吸引
        eState_BanSkill = 6,
    }

    //State组件
    public class StateComponent : Component
    {
        public Dictionary<int, int> m_dictStates= new Dictionary<int, int>();
    }
}